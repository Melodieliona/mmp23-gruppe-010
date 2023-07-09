using Defence;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace Player
{
    public class ShopController : MonoBehaviour
    {
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
        private static readonly int Color = Shader.PropertyToID("_Color");
        private static readonly int Thickness = Shader.PropertyToID("_Thickness");

        [Header("Items")]
        [SerializeField] private GameObject cannon1Prefab;
        [SerializeField] private int cannon1Cost = 10;
        [SerializeField] private GameObject cannon2Prefab;
        [SerializeField] private int cannon2Cost = 20;
        [SerializeField] private GameObject krakenPrefab;
        [SerializeField] private int krakenCost = 50;
        [SerializeField] private GameObject cannon1Transparent;
        [SerializeField] private GameObject cannon2Transparent;
        [SerializeField] private GameObject krakenTransparent;

        private SpriteRenderer cannon1TransparentRenderer;
        private SpriteRenderer cannon2TransparentRenderer;
        private SpriteRenderer krakenTransparentRenderer;
        private SpriteRenderer radiusRenderer;
        private MeshRenderer cursorRenderer;

        [Header("Grid")]
        [SerializeField] private GameObject map;
        [SerializeField] private Tilemap grassTiles;
        [SerializeField] private Tilemap waterTiles;
        [SerializeField] private GameObject placementGrid;
        [SerializeField] private GameObject cursor;
        [SerializeField] private GameObject radius;
        [SerializeField] private float fadeDuration = 0.25f;
        private Grid mapGrid;

        [Header("References")]
        private PlayerController playerController;
        private GameObject[] obstacleList;
        private Material gridMaterial;
        private Material cursorMaterial;
        private Material radiusMaterial;

        private ShopItem selectedShopItem;
        private bool canPlaceItem = false;

        private const float MinOpacity = 0f;
        private const float MaxOpacity = 1f;
        private bool fadeIn = false;
        private bool fadeOut = false;

        private void Start()
        {
            mapGrid = map.GetComponent<Grid>();
            playerController = FindObjectOfType<PlayerController>();
            obstacleList = GameObject.FindGameObjectsWithTag("Obstacles");

            cannon1TransparentRenderer = cannon1Transparent.GetComponent<SpriteRenderer>();
            cannon2TransparentRenderer = cannon2Transparent.GetComponent<SpriteRenderer>();
            krakenTransparentRenderer = krakenTransparent.GetComponent<SpriteRenderer>();
            radiusRenderer = radius.GetComponent<SpriteRenderer>();
            cursorRenderer = cursor.GetComponent<MeshRenderer>();

            // Instantiate the material to prevent the changes to stay even after the game ended
            var meshRenderer = placementGrid.GetComponent<MeshRenderer>();
            gridMaterial = Instantiate(meshRenderer.sharedMaterial);
            gridMaterial.SetFloat(Opacity, MinOpacity);
            meshRenderer.material = gridMaterial;

            cursorMaterial = Instantiate(cursorRenderer.sharedMaterial);
            cursorMaterial.SetFloat(Opacity, MinOpacity);
            cursorRenderer.material = cursorMaterial;

            radiusMaterial = Instantiate(radiusRenderer.sharedMaterial);
            radiusMaterial.SetFloat(Opacity, MinOpacity);
            radiusRenderer.material = radiusMaterial;
        }

        private void OnEnable()
        {
            ShopItem[] items =
            {
                new(cannon1Prefab, 1, cannon1Cost, TileType.Land, cannon1Prefab.GetComponent<CanonController>()),
                new(cannon2Prefab, 2, cannon2Cost, TileType.Land, cannon2Prefab.GetComponent<CanonController>()),
                new(krakenPrefab, 3, krakenCost, TileType.Water, krakenPrefab.GetComponent<KrakenController>()),
                new(cannon1Prefab, 4, cannon1Cost, TileType.Land, cannon1Prefab.GetComponent<CanonController>())
            };

            foreach (ShopItem item in items)
            {
                Button button = GetComponent<UIDocument>().rootVisualElement.Q<Button>("ShopButton" + item.GetSlot());
                button.clicked += () => { selectedShopItem = selectedShopItem == item ? null : item; };
            }
        }

        private void Update()
        {
            CalculateGridAlpha();
            ShowIndicator();

            if (selectedShopItem != null)
            {
                IsMouseInGrid();
                ActivateGrid();
                CheckForPlacement();
                canPlaceItem = true;
            }
            else
            {
                DeactivateGrid();
                canPlaceItem = false;
            }
        }

        private void CheckForPlacement()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Tilemap selectedTiles = selectedShopItem.GetTileType() != TileType.Water ? grassTiles : waterTiles;
            Vector3Int cellPosition = selectedTiles.WorldToCell(mousePosition);
            if (!IsEligible(cellPosition) || CheckForObstacle(mousePosition)) return;

            int weaponCost = selectedShopItem.GetCost();
            if (!playerController.RemoveGold(weaponCost))
            {
                Debug.Log("Too expensive! You can't afford it");
                return;
            }

            DeactivateGrid();

            Vector3 cellCenter = selectedTiles.GetCellCenterWorld(cellPosition);
            GameObject weaponPrefab = selectedShopItem.GetPrefab();
            Instantiate(weaponPrefab, cellCenter, weaponPrefab.transform.rotation);
            grassTiles.SetColliderType(cellPosition, Tile.ColliderType.None);

            selectedShopItem = null;
            canPlaceItem = false;
            cannon1Transparent.transform.position = new Vector2(50, 0);
            cannon2Transparent.transform.position = new Vector2(50, 0);
            krakenTransparent.transform.position = new Vector2(50, 0);
        }

        private bool IsEligible(Vector3Int cellPosition)
        {
            if (selectedShopItem == null) return false;

            Tilemap tilemap = selectedShopItem.GetTileType() != TileType.Water ? grassTiles : waterTiles;
            return tilemap.GetColliderType(cellPosition) == Tile.ColliderType.Sprite;
        }

        private bool CheckForObstacle(Vector3 mousePosition)
        {
            return obstacleList.Any(obstacle => obstacle.GetComponent<BoxCollider2D>().OverlapPoint(mousePosition));
        }

        private void ShowIndicator()
        {
            if (!canPlaceItem || selectedShopItem == null) return;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition;
            Vector3 cellCenter;
            if (selectedShopItem != null && selectedShopItem.GetTileType() != TileType.Water)
            {
                cellPosition = grassTiles.WorldToCell(mousePosition);
                cellCenter = grassTiles.GetCellCenterWorld(cellPosition);
            }
            else
            {
                cellPosition = waterTiles.WorldToCell(mousePosition);
                cellCenter = waterTiles.GetCellCenterWorld(cellPosition);
            }

            if (IsEligible(cellPosition) && !CheckForObstacle(mousePosition))
            {
                //cursor follows the mouse
                cursor.transform.position = cellCenter;
                //Set the radius of the item
                radius.transform.position = cellCenter;
                //show the preview of the item
                cannon1TransparentRenderer.color = new Color(1f, 1f, 1f, 110 / 255f);
                cannon2TransparentRenderer.color = new Color(1f, 1f, 1f, 110 / 255f);
                krakenTransparentRenderer.color = new Color(1f, 1f, 1f, 70 / 255f);
                cursorMaterial.SetColor(Color, new Color(1f, 1f, 1f, 1f));
                radiusMaterial.SetColor(Color, new Color(1f, 1f, 1f, 1f));
            }
            else
            {
                cannon1TransparentRenderer.color = new Color(1f, 0f, 0f, 110 / 255f);
                cannon2TransparentRenderer.color = new Color(1f, 0f, 0f, 110 / 255f);
                krakenTransparentRenderer.color = new Color(1f, 0f, 0f, 110 / 255f);
                cursorMaterial.SetColor(Color, new Color(1f, 0.3f, 0.3f, 1f));
                radiusMaterial.SetColor(Color, new Color(1f, 0.3f, 0.3f, 1f));
            }

            switch (selectedShopItem.GetSlot())
            {
                case 1:
                    cannon1Transparent.transform.position = cellCenter;
                    break;
                case 2:
                    cannon2Transparent.transform.position = cellCenter;
                    break;
                case 3:
                    krakenTransparent.transform.position = cellCenter;
                    break;
            }

            cursor.transform.position = cellCenter;
            radius.transform.position = cellCenter;
            var currentRange = selectedShopItem.GetRange() * 12;
            radius.transform.localScale = new Vector3(currentRange, currentRange, currentRange);
            radiusMaterial.SetFloat(Thickness, currentRange >= 24 ? 0.05f : 0.1f);
        }

        private void ActivateGrid()
        {
            fadeIn = true;
            fadeOut = false;
        }

        private void DeactivateGrid()
        {
            fadeIn = false;
            fadeOut = true;
        }

        private void CalculateGridAlpha()
        {
            float alpha = gridMaterial.GetFloat(Opacity);

            if (fadeIn && alpha <= MaxOpacity)
            {
                alpha += Time.deltaTime / fadeDuration;
                gridMaterial.SetFloat(Opacity, alpha);
                cursorMaterial.SetFloat(Opacity, alpha);
                radiusMaterial.SetFloat(Opacity, alpha);

                if (alpha >= MaxOpacity)
                {
                    fadeIn = false;
                }
            }

            if (fadeOut && alpha >= MinOpacity)
            {
                alpha -= Time.deltaTime / fadeDuration;
                gridMaterial.SetFloat(Opacity, alpha);
                cursorMaterial.SetFloat(Opacity, alpha);
                radiusMaterial.SetFloat(Opacity, alpha);

                if (alpha <= MinOpacity)
                {
                    fadeOut = false;
                }
            }
        }

        private void IsMouseInGrid()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mapGrid.GetComponent<BoxCollider2D>().OverlapPoint(mousePosition)) return;

            Vector2 offScreen = new Vector2(50, 0); // Some value not visible on the screen
            cursor.transform.position = offScreen;
            radius.transform.position = offScreen;
            cannon1Transparent.transform.position = offScreen;
            cannon2Transparent.transform.position = offScreen;
            krakenTransparent.transform.position = offScreen;
        }
    }
}
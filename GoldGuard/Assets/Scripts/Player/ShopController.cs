using Defence;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace Player
{
    public class ShopController : MonoBehaviour
    {
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");

        [Header("Items")]
        [SerializeField] private GameObject cannon1Prefab;
        [SerializeField] private int cannon1Cost = 10;
        [SerializeField] private GameObject cannon2Prefab;
        [SerializeField] private int cannon2Cost = 20;
        [SerializeField] private GameObject krakenPrefab;
        [SerializeField] private int krakenCost = 50;

        [Header("Grid")]
        [SerializeField] private Tilemap grassTiles;
        [SerializeField] private Tilemap waterTiles;
        [SerializeField] private GameObject placementGrid;
        [SerializeField] private GameObject cursor;
        [SerializeField] private GameObject radius;
        [SerializeField] private float fadeDuration = 0.25f;

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
            playerController = FindObjectOfType<PlayerController>();
            obstacleList = GameObject.FindGameObjectsWithTag("Obstacles");

            // Instantiate the material to prevent the changes to stay even after the game ended
            var meshRenderer = placementGrid.GetComponent<MeshRenderer>();
            gridMaterial = Instantiate(meshRenderer.sharedMaterial);
            gridMaterial.SetFloat(Opacity, MinOpacity);
            meshRenderer.material = gridMaterial;

            var cursorRenderer = cursor.GetComponent<MeshRenderer>();
            cursorMaterial = Instantiate(cursorRenderer.sharedMaterial);
            cursorMaterial.SetFloat(Opacity, MinOpacity);
            cursorRenderer.material = cursorMaterial;

            var radiusRenderer = radius.GetComponent<SpriteRenderer>();
            radiusMaterial = Instantiate(radiusRenderer.sharedMaterial);
            radiusMaterial.SetFloat(Opacity, MinOpacity);
            radiusRenderer.material = radiusMaterial;
        }

        private void OnEnable()
        {
            var cannon1Range = cannon1Prefab.GetComponent<CanonController>().GetRange();
            var cannon2Range = cannon2Prefab.GetComponent<CanonController>().GetRange();
            var krakenRange = krakenPrefab.GetComponent<KrakenController>().GetRange();
            //Add 4th item here

            ShopItem[] items =
            {
                new(cannon1Prefab, 1, cannon1Cost, TileType.Land, cannon1Range),
                new(cannon2Prefab, 2, cannon2Cost, TileType.Land, cannon2Range),
                new(krakenPrefab, 3, krakenCost, TileType.Water, krakenRange),
                new(cannon1Prefab, 4, cannon1Cost, TileType.Land, cannon1Range)
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
        }

        private bool IsEligible(Vector3Int cellPosition)
        {
            if (selectedShopItem == null) return false;

            Tilemap tilemap = selectedShopItem.GetTileType() != TileType.Water ? grassTiles : waterTiles;
            return tilemap.GetColliderType(cellPosition) == Tile.ColliderType.Sprite;
        }

        private bool CheckForObstacle(Vector3 mousePosition)
        {
            return obstacleList
                .Select(obstacle => obstacle.GetComponent<BoxCollider2D>().OverlapPoint(mousePosition))
                .FirstOrDefault();
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

            if (IsEligible(cellPosition))
            {
                cursor.transform.position = cellCenter;
                radius.transform.position = cellCenter;
                var currentRange = selectedShopItem.GetRange() * 12;
                radius.transform.localScale = new Vector3(currentRange, currentRange, currentRange);
            }
            else
            {
                Vector2 offScreen = new Vector2(2500, 2500); // Some value not visible on the screen
                cursor.transform.position = offScreen;
                radius.transform.position = offScreen;
            }
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
    }
}
using System.Linq;
using Player.ShopItems;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace Player
{
    public class ShopController : MonoBehaviour
    {
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
        private static readonly int Color = Shader.PropertyToID("_Color");
        private static readonly int Thickness = Shader.PropertyToID("_Thickness");

        private const float MinOpacity = 0f;
        private const float MaxOpacity = 1f;

        [Header("Grid")]
        [SerializeField] private GameObject map;
        [SerializeField] private Tilemap landTiles;
        [SerializeField] private Tilemap waterTiles;
        [SerializeField] private GameObject placementGrid;
        [SerializeField] private GameObject cursor;
        [SerializeField] private GameObject radius;
        [SerializeField] private float fadeDuration = 0.25f;

        private PlayerController playerController;
        private Grid mapGrid;
        private GameObject[] obstacleList;
        private Material gridMaterial;
        private Material cursorMaterial;
        private Material radiusMaterial;
        private SpriteRenderer radiusRenderer;
        private MeshRenderer cursorRenderer;

        private ShopItem selectedShopItem;
        private bool canPlaceItem = false;

        private bool fadeIn = false;
        private bool fadeOut = false;

        private void Start()
        {
            mapGrid = map.GetComponent<Grid>();
            playerController = FindObjectOfType<PlayerController>();
            obstacleList = GameObject.FindGameObjectsWithTag("Obstacles");

            // Instantiate the material to prevent the changes to stay even after the game ended
            var meshRenderer = placementGrid.GetComponent<MeshRenderer>();
            gridMaterial = Instantiate(meshRenderer.sharedMaterial);
            gridMaterial.SetFloat(Opacity, MinOpacity);
            meshRenderer.material = gridMaterial;

            cursorRenderer = cursor.GetComponent<MeshRenderer>();
            cursorMaterial = Instantiate(cursorRenderer.sharedMaterial);
            cursorMaterial.SetFloat(Opacity, MinOpacity);
            cursorRenderer.material = cursorMaterial;

            radiusRenderer = radius.GetComponent<SpriteRenderer>();
            radiusMaterial = Instantiate(radiusRenderer.sharedMaterial);
            radiusMaterial.SetFloat(Opacity, MinOpacity);
            radiusRenderer.material = radiusMaterial;

            InitShopItems();
        }

        private void InitShopItems()
        {
            ShopItem[] items =
            {
                new Cannon1Item(),
                new Cannon2Item(),
                new KrakenItem(),
                new Slot4Item()
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
                CheckMousePosition();
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
            Tilemap selectedTiles = selectedShopItem.GetTileType() != TileType.Water ? landTiles : waterTiles;
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
            landTiles.SetColliderType(cellPosition, Tile.ColliderType.None);

            selectedShopItem.GetTransparent().transform.position = new Vector2(50, 0);
            selectedShopItem = null;
            canPlaceItem = false;
        }

        private bool IsEligible(Vector3Int cellPosition)
        {
            if (selectedShopItem == null) return false;

            Tilemap tilemap = selectedShopItem.GetTileType() != TileType.Water ? landTiles : waterTiles;
            return tilemap.GetColliderType(cellPosition) == Tile.ColliderType.Sprite;
        }

        private bool CheckForObstacle(Vector3 mousePosition)
        {
            return obstacleList.Any(obstacle => obstacle.GetComponent<BoxCollider2D>().OverlapPoint(mousePosition));
        }

        /// <summary>
        /// Check if the defence placement indicator should be activated.
        /// </summary>
        private void ShowIndicator()
        {
            if (!canPlaceItem || selectedShopItem == null) return;

            Tilemap tiles = selectedShopItem.GetTileType() == TileType.Land ? landTiles : waterTiles;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tiles.WorldToCell(mousePosition);
            Vector3 cellCenter = tiles.GetCellCenterWorld(cellPosition);

            if (IsEligible(cellPosition) && !CheckForObstacle(mousePosition))
            {
                //cursor follows the mouse
                cursor.transform.position = cellCenter;
                //Set the radius of the item
                radius.transform.position = cellCenter;
                //show the preview of the item
                selectedShopItem.GetTransparentRenderer().color = new Color(1f, 1f, 1f, 110 / 255f);
                cursorMaterial.SetColor(Color, new Color(1f, 1f, 1f, 1f));
                radiusMaterial.SetColor(Color, new Color(1f, 1f, 1f, 1f));
            }
            else
            {
                selectedShopItem.GetTransparentRenderer().color = new Color(1f, 0f, 0f, 110 / 255f);
                cursorMaterial.SetColor(Color, new Color(1f, 0.3f, 0.3f, 1f));
                radiusMaterial.SetColor(Color, new Color(1f, 0.3f, 0.3f, 1f));
            }

            selectedShopItem.GetTransparent().transform.position = cellCenter;
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

        /// <summary>
        /// Checks if the cursor is on the main grid.
        /// If not then reset the position of all indicators to a value off the screen.
        /// <!---->
        /// This is to fix a visual issue.
        /// </summary>
        private void CheckMousePosition()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mapGrid.GetComponent<BoxCollider2D>().OverlapPoint(mousePosition)) return;

            Vector2 offScreen = new Vector2(50, 0); // Some value not visible on the screen
            cursor.transform.position = offScreen;
            radius.transform.position = offScreen;
            selectedShopItem.GetTransparent().transform.position = offScreen;
        }
    }
}
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace Player
{
    public class ShopController : MonoBehaviour
    {
        [Header("References")]
        private PlayerController playerController;

        private Button shopButton1;
        private Button shopButton2;
        private Button shopButton3;
        private Button shopButton4;

        public Tilemap grassTiles;
        public Tilemap waterTiles;

        public GameObject cannon1Prefab;
        public GameObject cannon2Prefab;
        public GameObject krakenPrefab;

        public GameObject placementGrid;
        private Material gridMaterial;
        private Material cursorMaterial;
        public GameObject cursor;

        private GameObject[] obstacleList;

        [Header("Attributes")]
        private int currentlySelected = 0;
        private int cannon1Cost = 10;
        private int cannon2Cost = 20;
        private int krakenCost = 50;

        private bool canPlaceItem = false;
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");

        // Values for the grid fading
        private const float FadeDuration = 1f;
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
        }

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            shopButton1 = root.Q<Button>("ShopButton1");
            shopButton2 = root.Q<Button>("ShopButton2");
            shopButton3 = root.Q<Button>("ShopButton3");
            shopButton4 = root.Q<Button>("ShopButton4");

            shopButton1.clicked += () => { ShopButton_clicked(1); };
            shopButton2.clicked += () => { ShopButton_clicked(2); };
            shopButton3.clicked += () => { ShopButton_clicked(3); };
            shopButton4.clicked += () => { ShopButton_clicked(4); };
        }

        private void ShopButton_clicked(int number)
        {
            currentlySelected = number;
        }

        private void Update()
        {
            CalculateGridAlpha();

            if (currentlySelected > 0)
            {
                // A button was pressed in the shop
                ActivateGrid();
                PlaceOnGrass();
                canPlaceItem = true;
            }

            if (canPlaceItem)
            {
                ShowIndicator();
            }
        }

        private void PlaceOnGrass()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = currentlySelected != 3 ? grassTiles.WorldToCell(mousePosition) : waterTiles.WorldToCell(mousePosition);
            if (!IsEligible(cellPosition) || CheckForObstacle(mousePosition)) return;

            GameObject weaponPrefab;
            int weaponCost;

            switch (currentlySelected)
            {
                case 1:
                    weaponPrefab = cannon1Prefab;
                    weaponCost = cannon1Cost;
                    break;
                case 2:
                    weaponPrefab = cannon2Prefab;
                    weaponCost = cannon2Cost;
                    break;
                case 3:
                    weaponPrefab = krakenPrefab;
                    weaponCost = krakenCost;
                    break;
                default:
                    throw new NullReferenceException("Player has not selected");
            }

            if (!playerController.RemoveGold(weaponCost))
            {
                Debug.Log("Too expensive! You can't afford it");
                return;
            }

            DeactivateGrid();
            currentlySelected = 0;
            canPlaceItem = false;

            Vector3 cellCenter = currentlySelected != 3 ? grassTiles.GetCellCenterWorld(cellPosition) : waterTiles.GetCellCenterWorld(cellPosition);
            Instantiate(weaponPrefab, cellCenter, weaponPrefab.transform.rotation);
            grassTiles.SetColliderType(cellPosition, Tile.ColliderType.None);
        }

        private bool IsEligible(Vector3Int cellPosition)
        {
            Tilemap tilemap = currentlySelected != 3 ? grassTiles : waterTiles;
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
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition;
            Vector3 cellCenter;
            if (currentlySelected != 3)
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
                alpha += Time.deltaTime / FadeDuration;
                gridMaterial.SetFloat(Opacity, alpha);
                cursorMaterial.SetFloat(Opacity, alpha);

                if (alpha >= MaxOpacity)
                {
                    fadeIn = false;
                }
            }

            if (fadeOut && alpha >= MinOpacity)
            {
                alpha -= Time.deltaTime / FadeDuration;
                gridMaterial.SetFloat(Opacity, alpha);
                cursorMaterial.SetFloat(Opacity, alpha);

                if (alpha <= MinOpacity)
                {
                    fadeOut = false;
                }
            }
        }
    }
}
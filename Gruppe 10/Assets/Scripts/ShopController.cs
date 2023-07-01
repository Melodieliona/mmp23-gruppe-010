using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class ShopController : MonoBehaviour
{
    [Header("References")]
    private PlayerManager playerManager;

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

    [Header("Attributes")]
    private int currentlySelected = 0;
    private int cannon1Cost = 10;
    private int cannon2Cost = 20;
    private int krakenCost = 50;

    private bool canPlaceItem = false;

    //Values for the grid fading
    private float fadeDuration = 1f;
    private float minOpacity = 0;
    private float maxOpacity = 1f;


    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();

        //Instantiate the material to prevent the changes to stay even after the game ended
        var meshRenderer = placementGrid.GetComponent<MeshRenderer>();
        gridMaterial = Instantiate(meshRenderer.sharedMaterial);
        meshRenderer.material = gridMaterial;
        var cursorRenderer = cursor.GetComponent<MeshRenderer>();
        cursorMaterial = Instantiate(cursorRenderer.sharedMaterial);
        cursorRenderer.material = cursorMaterial;
        cursorMaterial.SetFloat("_Opacity", 0f);
        gridMaterial.SetFloat("_Opacity", 0f);
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
        if (currentlySelected > 0)
        {
            // A button was pressed in the shop
            ActivateGrid();
            PlaceOnGrass();
            canPlaceItem = true;
        }
        if (canPlaceItem)
        {
            showIndicator();
        }
    }

    private void PlaceOnGrass()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition;
        if (currentlySelected != 3)
        {
            cellPosition = grassTiles.WorldToCell(mousePosition);
        }
        else
        {
            cellPosition = waterTiles.WorldToCell(mousePosition);
        }

        if (!IsEligible(cellPosition)) return;

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

        if (!playerManager.RemoveGold(weaponCost))
        {
            Debug.Log("Too expensive! You can't afford it");
            return;
        }

        Vector3 cellCenter;
        if (currentlySelected != 3)
        {
            cellCenter = grassTiles.GetCellCenterWorld(cellPosition);
        }
        else
        {
            cellCenter = waterTiles.GetCellCenterWorld(cellPosition);
        }

        Instantiate(weaponPrefab, cellCenter, weaponPrefab.transform.rotation);
        grassTiles.SetColliderType(cellPosition, Tile.ColliderType.None);
        canPlaceItem = false;
        DeactivateGrid();
        currentlySelected = 0;
    }

    private bool IsEligible(Vector3Int cellPosition)
    {
        if (currentlySelected != 3)
        {
            return grassTiles.GetColliderType(cellPosition) == Tile.ColliderType.Sprite;
        }
        else
        {
            return waterTiles.GetColliderType(cellPosition) == Tile.ColliderType.Sprite;
        }
    }

    private void showIndicator()
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
        //gridMaterial.SetFloat("_Opacity", 0.8f);
        StartCoroutine(FadeIn());
    }

    private void DeactivateGrid()
    {
        //gridMaterial.SetFloat("_Opacity", 0.1f);

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float currentOpacity = minOpacity;
        while (currentOpacity < maxOpacity - 0.05)
        {
            currentOpacity += Time.deltaTime / fadeDuration;
            gridMaterial.SetFloat("_Opacity", currentOpacity);
            cursorMaterial.SetFloat("_Opacity", currentOpacity);
            Color colour;
            Tilemap currentTiles;
            if (currentlySelected != 3)
            {
                currentTiles = waterTiles;
            }
            else
            {
                currentTiles = grassTiles;
            }
            colour = currentTiles.color;
            colour.a = 1 - currentOpacity;
            //currentTiles.color = colour;
            yield return null;
        }

        currentOpacity = maxOpacity;
        gridMaterial.SetFloat("_Opacity", currentOpacity);
        cursorMaterial.SetFloat("_Opacity", currentOpacity);
    }

    private IEnumerator FadeOut()
    {
        float currentOpacity = maxOpacity;
        while (currentOpacity > minOpacity + 0.05)
        {
            currentOpacity -= Time.deltaTime / fadeDuration;
            gridMaterial.SetFloat("_Opacity", currentOpacity);
            cursorMaterial.SetFloat("_Opacity", currentOpacity);
            Color colour;
            Tilemap currentTiles;
            if (currentlySelected != 3)
            {
                currentTiles = waterTiles;
            }
            else
            {
                currentTiles = grassTiles;
            }
            colour = currentTiles.color;
            colour.a = 1 - currentOpacity;
            //currentTiles.color = colour;

            print(currentOpacity);

            yield return null;
        }

        currentOpacity = minOpacity;
        gridMaterial.SetFloat("_Opacity", currentOpacity);
        cursorMaterial.SetFloat("_Opacity", currentOpacity);
    }
}
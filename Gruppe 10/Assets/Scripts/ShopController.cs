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

    private Button cannon1Button;
    private Button cannon2Button;
    private Button cannon3Button;
    private Button cannon4Button;

    public Tilemap grassTiles;
    public Tilemap waterTiles;

    public GameObject cannon1Prefab;
    public GameObject cannon2Prefab;

    public GameObject placementGrid;
    private Material gridMaterial;
    private Material cursorMaterial;
    public GameObject cursor;

    [Header("Attributes")]
    private int lastSelected = 0;
    private int cannon1Cost = 10;
    private int cannon2Cost = 20;

    private bool canPlaceItem = false;

    //Values for the grid fading
    private float fadeDuration = 1f;
    private float minOpacity = 0f;
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

        cannon1Button = root.Q<Button>("Cannon1Button");
        cannon2Button = root.Q<Button>("Cannon2Button");
        cannon3Button = root.Q<Button>("Cannon3Button");
        cannon4Button = root.Q<Button>("Cannon4Button");

        cannon1Button.clicked += () => { CannonButton_clicked(1); };
        cannon2Button.clicked += () => { CannonButton_clicked(2); };
        cannon3Button.clicked += () => { CannonButton_clicked(3); };
        cannon4Button.clicked += () => { CannonButton_clicked(4); };
    }

    private void CannonButton_clicked(int number)
    {
        lastSelected = number;
        Debug.Log("Button " + number);
    }

    private void Update()
    {
        if (lastSelected > 0)
        {
            // A button was pressed in the shop
            ActivateGrid();
            PlaceOnGrass();
            canPlaceItem = true;
        }
        if(canPlaceItem)
        {
            showIndicator();
        }
    }

    private void PlaceOnGrass()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = grassTiles.WorldToCell(mousePosition);

        if (!IsEligible(cellPosition)) return;

        GameObject cannonPrefab;
        int cannonCost;

        switch (lastSelected)
        {
            case 1:
                cannonPrefab = cannon1Prefab;
                cannonCost = cannon1Cost;
                break;
            case 2:
                cannonPrefab = cannon2Prefab;
                cannonCost = cannon2Cost;
                break;
            default:
                throw new NullReferenceException("Player has not selected");
        }

        if (!playerManager.RemoveGold(cannonCost))
        {
            Debug.Log("Too expensive! You can't afford it");
            return;
        }

        Vector3 cellCenter = grassTiles.GetCellCenterWorld(cellPosition);
        Instantiate(cannonPrefab, cellCenter, cannonPrefab.transform.rotation);
        lastSelected = 0;
        grassTiles.SetColliderType(cellPosition, Tile.ColliderType.None);
        canPlaceItem = false;
        DeactivateGrid();
    }

    private bool IsEligible(Vector3Int cellPosition)
    {
        return grassTiles.GetColliderType(cellPosition) == Tile.ColliderType.Sprite;
    }

    private void showIndicator()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = grassTiles.WorldToCell(mousePosition);
        Vector3 cellCenter = grassTiles.GetCellCenterWorld(cellPosition);
        if(IsEligible(cellPosition))
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
        while (currentOpacity < maxOpacity)
        {
            currentOpacity += Time.deltaTime / fadeDuration;
            gridMaterial.SetFloat("_Opacity", currentOpacity);
            cursorMaterial.SetFloat("_Opacity", currentOpacity);
            yield return null;
        }

        currentOpacity = maxOpacity;
        gridMaterial.SetFloat("_Opacity", currentOpacity);
        cursorMaterial.SetFloat("_Opacity", currentOpacity);
    }

    private IEnumerator FadeOut()
    {
        float currentOpacity = maxOpacity;
        while (currentOpacity > minOpacity)
        {
            currentOpacity -= Time.deltaTime / fadeDuration;
            gridMaterial.SetFloat("_Opacity", currentOpacity);
            cursorMaterial.SetFloat("_Opacity", currentOpacity);
            yield return null;
        }

        currentOpacity = minOpacity;
        gridMaterial.SetFloat("_Opacity", currentOpacity);
        cursorMaterial.SetFloat("_Opacity", currentOpacity);
    }


}
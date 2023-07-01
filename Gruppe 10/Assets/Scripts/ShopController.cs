using System;
using System.Collections;
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
    public Material gridMaterial;

    [Header("Attributes")]
    private int lastSelected = 0;
    private int cannon1Cost = 10;
    private int cannon2Cost = 20;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        DeactivateGrid();
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
        }
    }

    private void PlaceOnGrass()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = grassTiles.WorldToCell(mousePosition);

        if (!IsOccupied(cellPosition)) return;

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
        DeactivateGrid();
    }

    private bool IsOccupied(Vector3Int cellPosition)
    {
        return grassTiles.GetColliderType(cellPosition) == Tile.ColliderType.Sprite;
    }

    private void ActivateGrid()
    {
        placementGrid.SetActive(true);
    }

    private void DeactivateGrid()
    {
        placementGrid.SetActive(false);
    }
}
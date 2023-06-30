using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class ShopController : MonoBehaviour
{
    private PlayerManager playerManager;

    private Button Cannon1Button;
    private Button Cannon2Button;
    private Button Cannon3Button;
    private Button Cannon4Button;

    private int lastSelected = 0;

    public Tilemap GrassTiles;
    public Tilemap WaterTiles;

    public GameObject Cannon1Prefab;


    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Cannon1Button = root.Q<Button>("Cannon1Button");
        Cannon2Button = root.Q<Button>("Cannon2Button");
        Cannon3Button = root.Q<Button>("Cannon3Button");
        Cannon4Button = root.Q<Button>("Cannon4Button");


        Cannon1Button.clicked += () =>
        {
            CannonButton_clicked(1);
        };
        Cannon2Button.clicked += () =>
        {
            CannonButton_clicked(2);
        };
        Cannon3Button.clicked += () =>
        {
            CannonButton_clicked(3);
        };
        Cannon4Button.clicked += () =>
        {
            CannonButton_clicked(4);
        };
    }

    private void CannonButton_clicked(int number)
    {
        lastSelected = number;
        Debug.Log("Button " + number);
    }

    private void Update()
    {
        if(lastSelected > 0)
        {
            //A button was pressed in the shop
            PlaceOnGrass();
        }
    }

    private void PlaceOnGrass()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = GrassTiles.WorldToCell(mousePosition);

            Vector3 cellCenter = GrassTiles.GetCellCenterWorld(cellPosition);

            Instantiate(Cannon1Prefab, cellCenter, Cannon1Prefab.transform.rotation);

            Debug.Log(cellPosition);

        }
    }
}

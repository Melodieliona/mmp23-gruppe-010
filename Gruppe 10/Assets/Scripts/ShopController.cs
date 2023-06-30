using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class ShopController : MonoBehaviour
{
    [Header("References")]
    private PlayerManager PlayerManager;

    private Button Cannon1Button;
    private Button Cannon2Button;
    private Button Cannon3Button;
    private Button Cannon4Button;

    public Tilemap GrassTiles;
    public Tilemap WaterTiles;

    public GameObject Cannon1Prefab;
    public GameObject Cannon2Prefab;

    [Header("Attributes")]
    private int lastSelected = 0;
    private int cannon1Cost = 10;
    private int cannon2Cost = 20;


    private void Start()
    {
        PlayerManager = FindObjectOfType<PlayerManager>();
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

            switch(lastSelected)
            {
                case 1:
                    if (CheckIfOccupied(cellPosition))
                    {
                        if (PlayerManager.RemoveGold(cannon1Cost))
                        {
                            Instantiate(Cannon1Prefab, cellCenter, Cannon1Prefab.transform.rotation);
                            lastSelected = 0;
                            GrassTiles.SetColliderType(cellPosition, Tile.ColliderType.None);
                        }
                        else
                        {
                            Debug.Log("Too expensive! You can't afford it");
                        }
                    } 
                    break;
                case 2:
                    if (CheckIfOccupied(cellPosition))
                    {
                        if (PlayerManager.RemoveGold(cannon2Cost))
                        {
                            Instantiate(Cannon2Prefab, cellCenter, Cannon2Prefab.transform.rotation);
                            lastSelected = 0;
                            GrassTiles.SetColliderType(cellPosition, Tile.ColliderType.None);
                        }
                        else
                        {
                            Debug.Log("Too expensive! You can't afford it");
                        }
                    }
                    break;

            }
        }
    }

    private bool CheckIfOccupied(Vector3Int cellPosition)
    {
        if(GrassTiles.GetColliderType(cellPosition) == Tile.ColliderType.Sprite)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] towerPrefabs;

    private int selectedTower = 0;

    public GameObject GetSelectedTower()
    {
        return towerPrefabs[selectedTower];
    }
}
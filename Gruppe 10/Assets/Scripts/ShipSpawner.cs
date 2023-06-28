using UnityEngine;
using UnityEngine.Events;

public class ShipSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform shipsPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Attributes")]
    [SerializeField] private int startingShips = 5;
    [SerializeField] private float shipsPerSecond = 0.5f;

    [Header("Events")]
    public static readonly UnityEvent ReachTreasureChestEvent = new();

    private PlayerManager playerManagerScript;

    private bool waveActive = true;

    private int currentWave = 1;
    private int shipsAlive;
    private int shipsLeftToSpawn;
    private float timeSinceLastSpawn;

    private void Awake()
    {
        ReachTreasureChestEvent.AddListener(ReachTreasureChest);
        playerManagerScript = FindObjectOfType<PlayerManager>();
    }

    private void Start()
    {
        StartWave();
    }

    /// <summary>
    /// Attempts to spawn a new pirate ship if the current wave is active and enough time has passed since the last spawn.
    /// </summary>
    private void Update()
    {
        if (!waveActive) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (shipsLeftToSpawn > 0 && timeSinceLastSpawn >= (1f / shipsPerSecond))
        {
            SpawnShip();
        }

        if (shipsAlive == 0 && shipsLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void StartWave()
    {
        waveActive = true;
        shipsLeftToSpawn = ShipsPerWave();
    }

    private void EndWave()
    {
        waveActive = false;
        timeSinceLastSpawn = 0f;
    }

    private void SpawnShip()
    {
        Instantiate(shipsPrefab, spawnPoint.position, spawnPoint.rotation);
        shipsLeftToSpawn--;
        shipsAlive++;
        timeSinceLastSpawn = 0f;
    }

    /// <summary>
    /// Called, whenever a pirate ship has reached the treasure chest.
    /// </summary>
    private void ReachTreasureChest()
    {
        shipsAlive--;
        playerManagerScript.RemoveHealthPoints(1);
        Debug.Log("Ship has reached the gold. New HP: " + playerManagerScript.GetHealthPoints());
    }

    /// <summary>
    /// Calculates the amount of ships which are to be spawned in the current wave.
    /// </summary>
    /// <returns>The amount of ships</returns>
    private int ShipsPerWave()
    {
        return Mathf.RoundToInt(startingShips * Mathf.Pow(currentWave, 0.75f));
    }
}
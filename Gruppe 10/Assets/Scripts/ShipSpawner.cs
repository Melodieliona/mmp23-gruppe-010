using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ShipSpawner : MonoBehaviour
{
    public Transform shipsPrefab;
    public Transform spawnPoint;

    public GameObject playerManager;
    private PlayerManager playerManagerScript;


    public int startingShips = 5;
    public float shipsPerSecond = 0.5f;

    public static UnityEvent onReachTreasureChest = new UnityEvent();

    private bool waveActive = true;

    private int currentWave = 1;
    private int shipsLeftToSpawn;
    private int shipsAlive;

    private float timeSinceLastSpawn;

    void Awake()
    {
        onReachTreasureChest.AddListener(ReachTreasureChest);
    }

    void Start()
    {
        this.waveActive = true;
        this.shipsLeftToSpawn = ShipsPerWave();
        playerManagerScript = GameObject.FindObjectOfType<PlayerManager>();
    }

    /// <summary>
    /// Attempts to spawn a new pirate ship if the current wave is active and enough
    /// time has passed since the last spawn.
    /// </summary>
    void Update()
    {
        if (!waveActive) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (shipsLeftToSpawn > 0 && timeSinceLastSpawn >= (1f / shipsPerSecond))
        {
            SpawnShip();
        }
    }

    private void SpawnShip()
    {
        Instantiate(shipsPrefab, spawnPoint.position, spawnPoint.rotation);
        this.shipsLeftToSpawn--;
        this.shipsAlive++;
        this.timeSinceLastSpawn = 0f;
    }

    /// <summary>
    /// Called, whenever a pirate ship has reached the treasure chest.
    /// </summary>
    private void ReachTreasureChest()
    {
        Debug.Log("Ship has reached the gold");
        this.shipsAlive--;
        playerManagerScript.RemoveHP(1);
        Debug.Log(playerManagerScript.healthPoints);
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

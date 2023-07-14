using System.Collections;
using Player;
using Sound;
using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    public class ShipSpawner : MonoBehaviour
    {
        public static readonly UnityEvent ReachTreasureChestEvent = new();
        public static readonly UnityEvent EnemyDestroyEvent = new();
        private readonly UnityEvent waveChangeEvent = new();

        [Header("References")]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GameObject normalShip;
        [SerializeField] private GameObject fastShip;
        [SerializeField] private GameObject durableShip;

        private const int StartingShips = 10;
        private const int TimeBetweenWaves = 10;
        private float ShipsPerSecond = 2f;
        private int timerCounter;

        private PlayerController playerController;
        private GUIManager guiManager;
        private SoundEffectsPlayer soundEffect;

        private bool waveActive = true;

        private int currentWave = 0;
        private int shipsAlive;
        private int shipsLeftToSpawn;
        private float timeSinceLastSpawn;

        private void Awake()
        {
            playerController = FindObjectOfType<PlayerController>();
            guiManager = FindObjectOfType<GUIManager>();
            soundEffect = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundEffectsPlayer>();

            ReachTreasureChestEvent.AddListener(ReachTreasureChest);
            EnemyDestroyEvent.AddListener(EnemyDestroyed);
            waveChangeEvent.AddListener(guiManager.UpdateWave);
        }

        private void Start()
        {
            StartCoroutine(StartWave());
        }

        /// <summary>
        /// Attempts to spawn a new pirate ship if the current wave is active and enough time has passed since the last spawn.
        /// </summary>
        private void Update()
        {
            if (!waveActive || currentWave == 0) return;

            timeSinceLastSpawn += Time.deltaTime;

            if (shipsLeftToSpawn > 0 && timeSinceLastSpawn >= (1f / ShipsPerSecond))
            {
                SpawnShip();
            }

            if (shipsLeftToSpawn <= 0 && shipsAlive <= 0)
            {
                EndWave();
            }
        }

        private IEnumerator StartWave()
        {
            guiManager.ShowTimer();
            soundEffect.PlayBackgroundMusic(soundEffect.settingTime);
            for (timerCounter = TimeBetweenWaves; timerCounter >= 0; timerCounter--)
            {
                float percentageOfMax = (float)timerCounter / TimeBetweenWaves;
                guiManager.UpdateTimer(timerCounter, percentageOfMax);
                yield return new WaitForSeconds(1);
            }

            soundEffect.PlayBackgroundMusic(soundEffect.background);
            guiManager.HideTimer();
            currentWave++;
            waveChangeEvent.Invoke();
            shipsLeftToSpawn = ShipsPerWave();
            ShipsPerSecond += currentWave * 0.2f;
            waveActive = true;
        }

        private void EndWave()
        {
            waveActive = false;
            timeSinceLastSpawn = 0f;
            StartCoroutine(StartWave());
        }

        private void SpawnShip()
        {
            //GameObject ship = Random.Range(0, 10) < 5 ? normalShip : fastShip;
            GameObject ship;

            int randomValue = Random.Range(0, 10);
            if (randomValue < 5 - currentWave * 0.5)
            {
                ship = normalShip;
            }
            else if (randomValue >= 5 - currentWave * 0.5 && randomValue < 8 - currentWave * 0.25)
            {
                ship = fastShip;
            }
            else
            {
                ship = durableShip;
            }

            Instantiate(ship, spawnPoint.position, spawnPoint.rotation);

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
            soundEffect.PlaySfx(soundEffect.hp);
            playerController.RemoveHealthPoints(1);
            Debug.Log("Ship has reached the gold. New HP: " + playerController.GetHealthPoints());
        }

        /// <summary>
        /// Called, whenever a pirate ship is destroyed by a cannon.
        /// </summary>
        private void EnemyDestroyed()
        {
            shipsAlive--;
        }

        /// <summary>
        /// Calculates the amount of ships which are to be spawned in the current wave.
        /// </summary>
        /// <returns>The amount of ships</returns>
        private int ShipsPerWave()
        {
            return Mathf.RoundToInt(StartingShips * Mathf.Pow(currentWave, 2f));
        }

        public int GetCurrentWave()
        {
            return currentWave;
        }
    }
}
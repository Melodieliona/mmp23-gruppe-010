using System.Collections;
using System.Reflection.Emit;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Enemy
{
    public class ShipSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform shipsPrefab;
        [SerializeField] private Transform spawnPoint;

        [Header("Attributes")]
        [SerializeField] private int startingShips = 5;
        [SerializeField] private float shipsPerSecond = 0.5f;
        [SerializeField] private int timeBetweenWaves = 20;
        [SerializeField] private int timerCounter;

        [Header("Events")]
        public static readonly UnityEvent ReachTreasureChestEvent = new();
        public static readonly UnityEvent EnemyDestroyEvent = new();
        private readonly UnityEvent waveChangeEvent = new();

        private PlayerController playerController;
        private GUIManager guiManager;

        private bool waveActive = true;

        private int currentWave = 0;
        private int shipsAlive;
        private int shipsLeftToSpawn;
        private float timeSinceLastSpawn;
        SoundEffectsPlayer soundEffect;


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

            if (shipsLeftToSpawn > 0 && timeSinceLastSpawn >= (1f / shipsPerSecond))
            {
                SpawnShip();
            }

            if (shipsLeftToSpawn == 0 && shipsAlive == 0)
            {
                EndWave();
            }
        }

        private IEnumerator StartWave()
        {
            guiManager.ShowTimer();
            soundEffect.PlayBackgroundMusic(soundEffect.settingTime);
            for (timerCounter = timeBetweenWaves; timerCounter >= 0; timerCounter--)
            {
                float percentageOfMax = (float)timerCounter / timeBetweenWaves;
                guiManager.UpdateTimer(timerCounter, percentageOfMax);
                yield return new WaitForSeconds(1);
            }
            soundEffect.PlayBackgroundMusic(soundEffect.background);
            guiManager.HideTimer();
            currentWave++;
            waveChangeEvent.Invoke();
            shipsLeftToSpawn = ShipsPerWave();
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
            soundEffect.PlaySFX(soundEffect.hp);
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
            return Mathf.RoundToInt(startingShips * Mathf.Pow(currentWave, 0.75f));
        }

        public int GetCurrentWave()
        {
            return currentWave;
        }
    }
}
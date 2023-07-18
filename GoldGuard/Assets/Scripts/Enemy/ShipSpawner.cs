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

        private const int StartingShips = 3;
        private const int TimeBetweenWaves = 10;
        private float shipsPerSecond = 0.5f;
        private int timerCounter;

        private PlayerController playerController;
        private GUIManager guiManager;
        private SoundEffectsPlayer soundEffect;

        private bool waveActive = true;

        private int currentWave = 0;
        private GameObject[] shipList;
        private int shipsLeftToSpawn;
        private float timeSinceLastSpawn;

        private void Awake()
        {
            playerController = FindObjectOfType<PlayerController>();
            guiManager = FindObjectOfType<GUIManager>();
            soundEffect = SoundEffectsPlayer.Instance;

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
            
            shipList = GameObject.FindGameObjectsWithTag("PirateShip");

            if (shipsLeftToSpawn <= 0 && shipList.Length <= 0)
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

            soundEffect.PlaySfx(soundEffect.horn);
            soundEffect.PlayBackgroundMusic(soundEffect.backgroundGame);
            guiManager.HideTimer();

            currentWave++;
            waveChangeEvent.Invoke();
            shipsLeftToSpawn = ShipsPerWave();
            Debug.Log("Wave: " + currentWave + ", Ships left: " + shipsLeftToSpawn);
            shipsPerSecond += currentWave * 0.1f;
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
            GameObject ship;
            int randomValue = Random.Range(0, 100);
            int normalShipOdds = 100 - currentWave * 5;
            int fastShipOdds = normalShipOdds + currentWave * 2;

            if (randomValue < normalShipOdds)
            {
                ship = normalShip;
            }
            else if (randomValue >= normalShipOdds && randomValue < fastShipOdds)
            {
                ship = fastShip;
            }
            else
            {
                ship = durableShip;
            }
            
            // Change HP depending on the wave
            GameObject currentShip = Instantiate(ship, spawnPoint.position, spawnPoint.rotation);
            currentShip.GetComponent<PirateShipController>().MultiplyHp(1 + currentWave * 0.125f);

            //shipList.Add(currentShip);
            shipsLeftToSpawn--;
            timeSinceLastSpawn = 0f;
        }

        /// <summary>
        /// Called, whenever a pirate ship has reached the treasure chest.
        /// </summary>
        private void ReachTreasureChest()
        {
            soundEffect.PlaySfx(soundEffect.hp);
            playerController.RemoveHealthPoints(1);
            Debug.Log("Ship has reached the gold. New HP: " + playerController.GetHealthPoints());
        }

        /// <summary>
        /// Called, whenever a pirate ship is destroyed by a cannon.
        /// </summary>
        private void EnemyDestroyed()
        {
            soundEffect.PlaySfx(soundEffect.coins);
        }

        /// <summary>
        /// Calculates the amount of ships which are to be spawned in the current wave.
        /// </summary>
        /// <returns>The amount of ships</returns>
        private int ShipsPerWave()
        {
            int factor = currentWave < 5 ? 3 : 5;
            return StartingShips + currentWave * factor;
        }

        public int GetCurrentWave()
        {
            return currentWave;
        }
    }
}
using Player;
using Sound;
using UnityEngine;

namespace Defence
{
    public abstract class CannonController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CannonType cannonType;
        [SerializeField] private GameObject cannonUI;
        [SerializeField] private LayerMask enemyMask;
        [SerializeField] private Transform turretRotationPoint;
        [SerializeField] private Transform firingPoint;
        [SerializeField] private GameObject bulletPrefab;

        private const int MaxLevel = 3;
        private CannonLevel currentLevel;

        private Transform target;
        private float timeUntilFire;
        private SoundEffectsPlayer soundEffect;

        private PlayerController playerController;
        private ShopController shopController;
        private GameObject eventSystem;

        private void Awake()
        {
            soundEffect = SoundEffectsPlayer.Instance;
        }

        private void Start()
        {
            currentLevel = GetBaseStats();

            playerController = FindObjectOfType<PlayerController>();
            shopController = FindObjectOfType<ShopController>();
            eventSystem = GameObject.Find("EventSystemUI");

            cannonUI.GetComponent<Canvas>().worldCamera = Camera.main;
        }

        private void Update()
        {
            UpdateUpgradeText(currentLevel, GetNextLevel(), playerController.GetGold(), CalculateValue());
            
            if (target == null)
            {
                FindTarget();
                return;
            }

            RotateTowardsTarget();

            if (!IsTargetInRange())
            {
                target = null;
                return;
            }

            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / currentLevel.GetBps())
            {
                Shoot();
                soundEffect.PlaySfx(soundEffect.cannon);
                timeUntilFire = 0f;
            }
        }

        private void Shoot()
        {
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            BulletController bulletScript = bulletObj.GetComponent<BulletController>();
            bulletScript.SetStrong(cannonType == CannonType.Heavy);
            bulletScript.SetDamage(currentLevel.GetDamage());
            bulletScript.SetTarget(target);
        }

        private void FindTarget()
        {
            Vector2 position = transform.position;
            RaycastHit2D result = Physics2D.CircleCast(position, currentLevel.GetRange(), position, 0f, enemyMask);
            target = result.transform;
        }

        private bool IsTargetInRange()
        {
            Vector2 cannonPosition = transform.position;
            Vector2 targetPosition = target.position;
            float dX = cannonPosition.x - targetPosition.x;
            float dY = cannonPosition.y - targetPosition.y;

            return dX * dX + dY * dY <= currentLevel.GetRange() * currentLevel.GetRange();
        }

        private void RotateTowardsTarget()
        {
            Vector2 cannonPos = transform.position;
            Vector2 targetPos = target.position;
            float angle = Mathf.Atan2(targetPos.y - cannonPos.y, targetPos.x - cannonPos.x) * Mathf.Rad2Deg + 90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, 400 * Time.deltaTime);
        }

        public void UpgradeCannon()
        {
            // Whether cannon can be upgraded is already checked when opening the update UI
            CannonLevel nextLevel = GetNextLevel();
            int cost = nextLevel.GetCost();
            if (cost > playerController.GetGold())
            {
                Debug.Log("Too expensive! The upgrade cost is: " + cost + " Gold");
                return;
            }

            playerController.RemoveGold(cost);
            shopController.ChangeRadiusSizeAndPos(nextLevel.GetRange(), gameObject.transform.position);
            ChangeSprite(nextLevel.GetLevel());
            DeactivateUpgradeUI();
            currentLevel = nextLevel;
            Debug.Log("Upgraded cannon! " + nextLevel);
        }

        public void SellCannon()
        {
            shopController.RemoveCannon(gameObject.transform.position);
            Destroy(gameObject);
            DeactivateUpgradeUI();
            shopController.GetLandTiles().RefreshAllTiles();
            playerController.AddGold(CalculateValue());
            Debug.Log("Sold the cannon for: " + CalculateValue() + " Gold");
        }

        /// <summary>
        /// Gets the stats the cannon has when first placed.
        /// </summary>
        /// <returns>The stats the cannon has when first placed</returns>
        protected abstract CannonLevel GetBaseStats();

        /// <summary>
        /// Gets the stats after the cannon has has been upgraded for the first time.
        /// </summary>
        /// <returns>The stats after the cannon has has been upgraded for the first time</returns>
        protected abstract CannonLevel GetFirstUpgrade();

        /// <summary>
        /// Gets the stats after the cannon has has been upgraded for the second time.
        /// </summary>
        /// <returns>The stats after the cannon has has been upgraded for the second time</returns>
        protected abstract CannonLevel GetSecondUpgrade();

        /// <summary>
        /// Updates the text in the upgrade UI with the correct values 
        /// </summary>
        /// <param name="currentLevel">The cannon's current level</param>
        /// <param name="nextLevel">The level that will be upgraded to</param>
        /// <param name="gold">The amount of gold the player has</param>
        /// <param name="cannonValue">The cannon's value</param>
        protected abstract void UpdateUpgradeText(CannonLevel currentLevel, CannonLevel nextLevel, int gold, int cannonValue);

        /// <summary>
        /// Changes the sprite of the cannon to that of the given level
        /// </summary>
        /// <param name="level">The level of the sprite to use</param>
        protected abstract void ChangeSprite(int level);

        public CannonLevel GetCurrentLevel()
        {
            return currentLevel;
        }

        private CannonLevel GetNextLevel()
        {
            return currentLevel.GetLevel() switch
            {
                1 => GetFirstUpgrade(),
                2 => GetSecondUpgrade(),
                _ => null
            };
        }

        /// <summary>
        /// Calculates the value of the cannon including its upgrades
        /// </summary>
        /// <returns>The amount of gold that the next update level costs</returns>
        private int CalculateValue()
        {
            return Mathf.RoundToInt(currentLevel.GetCost() / 2f);
        }

        public void ActivateUpgradeUI()
        {
            if (currentLevel == null) return;
            if (currentLevel.GetLevel() == MaxLevel)
            {
                Debug.Log("Max Cannon level reached (Level " + MaxLevel + ")");
                return;
            }

            eventSystem.SetActive(false);
            cannonUI.SetActive(true);
            shopController.ChangeRadiusOpacity(1f);
            shopController.ChangeRadiusSizeAndPos(currentLevel.GetRange(), gameObject.transform.position);
        }

        public void DeactivateUpgradeUI()
        {
            cannonUI.SetActive(false);
            shopController.ChangeRadiusOpacity(0f);
            eventSystem.SetActive(true);
        }
    }
}
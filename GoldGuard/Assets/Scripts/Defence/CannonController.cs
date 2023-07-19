using Player;
using Sound;
using UnityEngine;

namespace Defence
{
    public abstract class CannonController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform turretRotationPoint;
        [SerializeField] private LayerMask enemyMask;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firingPoint;
        [SerializeField] private CannonType cannonType;
        [SerializeField] private GameObject cannonUI;

        private const int MaxLevel = 3;
        private CannonStats currentLevel;

        private Transform target;
        private float timeUntilFire;
        private SoundEffectsPlayer soundEffect;
        private bool isUIOpen = false;

        private PlayerController playerController;
        private ShopController shopController;

        /// <summary>
        /// Gets the stats the cannon has when first placed.
        /// </summary>
        /// <returns>The stats the cannon has when first placed</returns>
        protected abstract CannonStats GetBaseStats();

        /// <summary>
        /// Gets the stats after the cannon has has been upgraded for the first time.
        /// </summary>
        /// <returns>The stats after the cannon has has been upgraded for the first time</returns>
        protected abstract CannonStats GetFirstUpgrade();

        /// <summary>
        /// Gets the stats after the cannon has has been upgraded for the second time.
        /// </summary>
        /// <returns>The stats after the cannon has has been upgraded for the second time</returns>
        protected abstract CannonStats GetSecondUpgrade();

        public CannonStats GetCurrentLevel()
        {
            return currentLevel;
        }

        private CannonStats GetNextLevel()
        {
            return currentLevel.GetLevel() switch
            {
                1 => GetFirstUpgrade(),
                2 => GetSecondUpgrade(),
                _ => null
            };
        }

        private void Awake()
        {
            soundEffect = SoundEffectsPlayer.Instance;
        }

        private void Start()
        {
            currentLevel = GetBaseStats();

            playerController = FindObjectOfType<PlayerController>();
            shopController = FindObjectOfType<ShopController>();

            cannonUI.GetComponent<Canvas>().worldCamera = Camera.main;
        }

        private void Update()
        {
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
            bulletScript.SetTarget(target);
            bulletScript.SetStrong(cannonType == CannonType.Heavy);
        }

        private void FindTarget()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, currentLevel.GetRange(), transform.position, 0f, enemyMask);
            if (hits.Length > 0)
            {
                target = hits[0].transform;
            }
        }

        private bool IsTargetInRange()
        {
            return Vector2.Distance(target.position, transform.position) <= currentLevel.GetRange();
        }

        private void RotateTowardsTarget()
        {
            Vector2 cannonPos = transform.position;
            Vector2 targetPos = target.position;
            float angle = Mathf.Atan2(targetPos.y - cannonPos.y, targetPos.x - cannonPos.x) * Mathf.Rad2Deg + 90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, currentLevel.GetRotationSpeed() * Time.deltaTime);
        }

        private void OnMouseDown()
        {
            if (!isUIOpen)
            {
                cannonUI.SetActive(true);
                isUIOpen = true;
                shopController.ChangeRadiusOpacity(1f);
                shopController.ChangeRadiusSizeAndPos(currentLevel.GetRange(), gameObject.transform.position);
            }
            else
            {
                cannonUI.SetActive(false);
                isUIOpen = false;
                shopController.ChangeRadiusOpacity(0f);
            }
        }

        public void UpgradeCannon()
        {
            if (currentLevel.GetLevel() == MaxLevel)
            {
                Debug.Log("Max Cannon level reached (Level " + MaxLevel + ")");
                return;
            }

            CannonStats nextLevel = GetNextLevel();
            int cost = nextLevel.GetCost();
            if (cost > playerController.GetGold())
            {
                Debug.Log("Too expensive! The upgrade cost is: " + cost + " Gold");
                return;
            }

            playerController.RemoveGold(cost);
            currentLevel = nextLevel;
            Debug.Log("Upgraded cannon! " + nextLevel);
        }

        public void SellCannon()
        {
            playerController.AddGold(CalculateValue());
            Debug.Log("Sold the cannon for: " + CalculateValue() + " Gold");
            Destroy(gameObject);
            shopController.GetLandTiles().RefreshAllTiles();
            shopController.ChangeRadiusOpacity(0f);
        }

        /// <summary>
        /// Calculates the value of the cannon including its upgrades
        /// </summary>
        /// <returns>The amount of gold that the next update level costs</returns>
        private int CalculateValue()
        {
            return Mathf.RoundToInt(currentLevel.GetCost() / 2f);
        }
    }
}
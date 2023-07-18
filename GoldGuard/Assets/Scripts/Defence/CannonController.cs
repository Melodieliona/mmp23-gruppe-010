using Player;
using Player.ShopItems;
using Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Defence
{
    public class CannonController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform turretRotationPoint;
        [SerializeField] private LayerMask enemyMask;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject bulletStrongPrefab;
        [SerializeField] private Transform firingPoint;
        [SerializeField] private int cannonType;
        [SerializeField] private GameObject canonUI;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button sellButton;

        [Header("Attribute")]
        [SerializeField] private float targetingRange;
        [SerializeField] private int upgradeCost = 100;
        [SerializeField] private float RotationSpeed = 400f;
        [SerializeField] private float Bps = 1f;

        // Default attributes of the turret (with no upgrades)
        private float bpsDefault;
        private float targetingRangeDefault;
        private int canonLevel = 1;
        private int canonWorth;
        private int canonDefaultCost = 50;

        private Transform target;
        private float timeUntilFire;
        private SoundEffectsPlayer soundEffect;
        private bool isUIOpen = false;

        private PlayerController playerController;

        private void Awake()
        {
            soundEffect = SoundEffectsPlayer.Instance;
        }

        private void Start()
        {
            bpsDefault = Bps;
            targetingRangeDefault = targetingRange;
            playerController = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {
            if (target == null)
            {
                FindTarget();
                return;
            }

            RotateTowardsTarget();

            if (!CheckTargetIsInRange())
            {
                target = null;
                return;
            }

            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / Bps)
            {
                Shoot();
                soundEffect.PlaySfx(soundEffect.cannon);
                timeUntilFire = 0f;
            }
        }

        private void Shoot()
        {
            GameObject currentPrefab = cannonType != 2 ? bulletPrefab : bulletStrongPrefab;
            GameObject bulletObj = Instantiate(currentPrefab, firingPoint.position, Quaternion.identity);
            BulletController bulletScript = bulletObj.GetComponent<BulletController>();
            bulletScript.SetTarget(target);
            if (cannonType == 2) bulletScript.SetStrong(true);
        }

        private void FindTarget()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, transform.position, 0f, enemyMask);

            if (hits.Length > 0)
            {
                target = hits[0].transform;
            }
        }

        private bool CheckTargetIsInRange()
        {
            return Vector2.Distance(target.position, transform.position) <= targetingRange;
        }

        private void RotateTowardsTarget()
        {
            Vector2 cannonPos = transform.position;
            Vector2 targetPos = target.position;
            float angle = Mathf.Atan2(targetPos.y - cannonPos.y, targetPos.x - cannonPos.x) * Mathf.Rad2Deg + 90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, RotationSpeed * Time.deltaTime);
            //turretRotationPoint.rotation = targetRotation;
        }

        private void OnMouseDown()
        {
            if (!isUIOpen)
            {
                OpenUI();
            }
            else
            {
                CloseUI();
            }
        }

        private void OpenUI()
        {
            canonUI.SetActive(true);
            isUIOpen = true;
        }

        private void CloseUI()
        {
            canonUI.SetActive(false);
            isUIOpen = false;
        }

        public void UpgradeCanon()
        {
            if(canonLevel >= 5) {
                Debug.Log("Max Canon level reached (Level 5)");
                return;
            }

            if(CalculateCost() > playerController.GetGold()) {
                Debug.Log("Too expensive! The upgrade cost is: " + CalculateCost() + " Gold");
                return;
            }

            playerController.RemoveGold(CalculateCost());
            canonLevel++;
            Bps = CalculateBps();
            targetingRange = CalculateRange();

            Debug.Log("Canon Level: " + canonLevel);

        }

        public void SellCanon()
        {
            playerController.AddGold(CalculateWorth());
            Debug.Log("Sold the canon for: " + CalculateWorth() + " Gold");
            Destroy(gameObject);
        }

        //Calculate cost of each upgrade
        private int CalculateCost() 
        {
            return (canonDefaultCost + canonLevel*50);
        }

        //Calculate worth of the turret including its upgrades
        private int CalculateWorth() 
        {
            return Mathf.RoundToInt(CalculateCost()/2);
        }

        private float CalculateBps() 
        {
            return bpsDefault * Mathf.Pow(canonLevel, 0.5f);
        }
        private float CalculateRange() 
        {
            return targetingRangeDefault * Mathf.Pow(canonLevel, 0.4f);
        }

        public float GetRange()
        {
            return targetingRange;
        }
    }
}
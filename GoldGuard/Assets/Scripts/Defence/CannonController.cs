using Sound;
using UnityEngine;

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
        [SerializeField] private float targetingRange;
        [SerializeField] private int cannonType;

        private const float RotationSpeed = 400f;
        private const float Bps = 1f;

        private Transform target;
        private float timeUntilFire;
        private SoundEffectsPlayer soundEffect;

        private void Awake()
        {
            soundEffect = SoundEffectsPlayer.Instance;
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

        public float GetRange()
        {
            return targetingRange;
        }
    }
}
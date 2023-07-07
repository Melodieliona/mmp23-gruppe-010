using Enemy;
using UnityEngine;

namespace Defence
{
    public class KrakenController : MonoBehaviour
    {
        [Header("References")]
        //[SerializeField] private LayerMask enemyMask;
        //[SerializeField] private Rigidbody2D rb;

        [Header("Attribute")]
        //[SerializeField] private float targetingRange = 1f;
        [SerializeField] private float damage = 5f;
        [SerializeField] private float damageUntilDestroyed = 25f;

        //private Transform target;
        //private float timeUntilAttack;

        private void Update()
        {
            //if (target == null)
            //{
            //    FindTarget();
            //    return;
            //}
            //
            //if (!CheckTargetIsInRange())
            //{
            //    target = null;
            //    return;
            //}
            //
            //timeUntilAttack += Time.deltaTime;
            //if (timeUntilAttack >= 1f / damage)
            //{
            //    Attack();
            //    timeUntilAttack = 0f;
            //}
        }

        private void Attack()
        {
            Debug.Log("Attack");
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("collision");
            if (other == null || other.gameObject == null)
            {
                return;
            }

            PirateShipController ship = other.gameObject.GetComponent<PirateShipController>();
            if (ship == null)
            {
                return;
            }

            ship.Damage(damage);
            damageUntilDestroyed -= damage;
            if(damageUntilDestroyed <= 0f)
            {
                Destroy(gameObject);
            }
        }

        private void FindTarget()
        {
            //RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, transform.position, 0f, enemyMask);
            //
            //if (hits.Length > 0)
            //{
            //    target = hits[0].transform;
            //}
        }

        private bool CheckTargetIsInRange()
        {
            //return Vector2.Distance(target.position, transform.position) <= targetingRange;
            return true;
        }
    }
}
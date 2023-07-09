using Enemy;
using UnityEngine;

namespace Defence
{
    public class BulletController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D rb;

        private const float BulletSpeed = 10f;
        private const int BulletDamage = 5;

        private Transform target;

        public void SetTarget(Transform target)
        {
            this.target = target;
            Destroy(gameObject, 10.0f);
        }

        private void FixedUpdate()
        {
            if (!target) return;

            Vector2 direction = (target.position - transform.position).normalized; // Direction to target
            rb.velocity = direction * BulletSpeed; // Recalculate target position
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other == null || other.gameObject == null)
            {
                return;
            }

            PirateShipController ship = other.gameObject.GetComponent<PirateShipController>();
            if (ship == null)
            {
                return;
            }

            ship.Damage(BulletDamage);
            Destroy(gameObject);
        }
    }
}
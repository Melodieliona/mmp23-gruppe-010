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

        public void SetTarget(Transform target)
        {
            if (target == null) return;

            Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
            rb.velocity = direction * BulletSpeed;
            Destroy(gameObject, 10f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other == null || other.gameObject == null)
            {
                return;
            }

            PirateShipController ship = other.gameObject.GetComponent<PirateShipController>();
            if (ship != null)
            {
                ship.Damage(BulletDamage);
                Destroy(gameObject);
            }
        }
    }
}
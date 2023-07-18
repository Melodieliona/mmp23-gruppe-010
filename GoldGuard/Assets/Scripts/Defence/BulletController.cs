using Enemy;
using UnityEngine;

namespace Defence
{
    public class BulletController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D rb;

        private float bulletSpeed = 10f;
        private int bulletDamage = 5;

        private bool isStrong = false;
        private int shipCount = 0;
        private const int MaxShips = 3;

        public void SetTarget(Transform target)
        {
            if (target == null) return;

            if (isStrong)
            {
                bulletSpeed *= 1.1f;
                bulletDamage *= 2;
            }

            Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
            rb.velocity = direction * bulletSpeed;
            Destroy(gameObject, 10f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other == null || other.gameObject == null) return;

            PirateShipController ship = other.gameObject.GetComponent<PirateShipController>();
            if (ship == null) return;

            ship.Damage(bulletDamage);
            if (isStrong && shipCount < MaxShips)
            {
                shipCount++;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetStrong(bool newIsStrong)
        {
            isStrong = newIsStrong;
        }
    }
}
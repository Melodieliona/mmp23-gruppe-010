using Enemy;
using UnityEngine;

namespace Defence
{
    public class BulletController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D rb;

        private float speed = 10f;
        private float damage;

        private bool isStrong = false;
        private int shipCount = 0;
        private const int MaxShips = 3;

        public void SetStrong(bool newIsStrong)
        {
            isStrong = newIsStrong;
            speed *= 1.1f;
        }

        public void SetDamage(float bulletDamage)
        {
            damage = bulletDamage;
        }

        public void SetTarget(Transform target)
        {
            if (target == null) return;

            Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
            rb.velocity = direction * speed;
            Destroy(gameObject, 10f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other == null || other.gameObject == null) return;

            PirateShipController ship = other.gameObject.GetComponent<PirateShipController>();
            if (ship == null) return;

            ship.Damage(damage);
            if (isStrong && shipCount < MaxShips)
            {
                shipCount++;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
using Enemy;
using UnityEngine;

namespace Defence
{
    public class BulletController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D rb;

        private float BulletSpeed = 10f;
        private int BulletDamage = 5;
        private bool isStrong = false;
        private const int maxShips = 3;
        private int shipCount = 0;

        public void SetTarget(Transform target)
        {
            if (target == null) return;

            if (isStrong)
            {
                BulletSpeed *= 1.1f;
                BulletDamage *= 2;
            }

            Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
            rb.velocity = direction * BulletSpeed;
            Destroy(gameObject, 10f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other == null || other.gameObject == null) return;

            PirateShipController ship = other.gameObject.GetComponent<PirateShipController>();
            if (ship == null) return;

            ship.Damage(BulletDamage);
            if (isStrong && shipCount < maxShips)
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
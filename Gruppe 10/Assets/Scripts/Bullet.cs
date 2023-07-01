using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;

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
        rb.velocity = direction * bulletSpeed; // Recalculate target position
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other == null || other.gameObject == null)
        {
            return;
        }

        PirateShip ship = other.gameObject.GetComponent<PirateShip>();
        if (ship == null)
        {
            return;
        }

        ship.Damage(bulletDamage);
        Destroy(gameObject);
    }
}
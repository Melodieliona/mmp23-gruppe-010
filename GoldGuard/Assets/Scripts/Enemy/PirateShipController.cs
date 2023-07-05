using Player;
using UnityEngine;

namespace Enemy
{
    public class PirateShipController : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private float speed = 3f;
        [SerializeField] private double health = 2;
        [SerializeField] private int scoreValue = 1;
        [SerializeField] private int goldValue = 1;

        public Sprite[] sprites;

        [Header("References")]
        private PlayerController playerController;

        private Transform target;
        private int waypointIndex = 0;
        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            playerController = FindObjectOfType<PlayerController>();

            target = Waypoints.points[0];
            spriteRenderer = GetComponent<SpriteRenderer>();
            SetSpriteForDirection(target.position - transform.position);
        }

        /// <summary>
        /// Moves the ship in the direction of the targeted waypoint.
        /// On reaching the target, the next waypoint is set.
        /// </summary>
        private void Update()
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * (speed * Time.deltaTime), Space.World);

            if (Vector2.Distance(target.position, transform.position) <= 0.05f)
            {
                GetNextWaypoint();
                SetSpriteForDirection(target.position - transform.position);
            }
        }

        /// <summary>
        /// Attempts to get the next waypoint to target.
        /// If there are no more waypoints available, the ship has reached the treasure chest and will therefore be destroyed,
        /// invoking <see cref="ShipSpawner.ReachTreasureChestEvent"/>.
        /// </summary>
        private void GetNextWaypoint()
        {
            waypointIndex++;

            if (waypointIndex >= Waypoints.points.Length)
            {
                ShipSpawner.ReachTreasureChestEvent.Invoke();
                Destroy(gameObject);
                return;
            }

            target = Waypoints.points[waypointIndex];
        }

        public void Damage(double amount)
        {
            health -= amount;

            if (health <= 0)
            {
                ShipSpawner.EnemyDestroyEvent.Invoke();
                Destroy(gameObject);
                playerController.AddScore(scoreValue);
                playerController.AddGold(goldValue);
            }
        }

        public double GetHealth()
        {
            return health;
        }

        private void SetSpriteForDirection(Vector2 direction)
        {
            // Find the correct sprite base on the direction angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            int spriteIndex = 0; // Right

            switch (angle)
            {
                case >= 45f and < 135f:
                    spriteIndex = 1; // Down
                    break;
                case >= 135f:
                case < -135f:
                    spriteIndex = 2; // Left
                    break;
                case >= -135f and < -45f:
                    spriteIndex = 3; // Up
                    break;
            }

            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }
}
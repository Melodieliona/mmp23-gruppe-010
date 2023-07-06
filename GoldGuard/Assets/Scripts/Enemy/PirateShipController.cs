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

        [Header("References")]
        private PlayerController playerController;

        private Transform target;
        private int waypointIndex = 0;
        private AnimationClip[] animationClips;
        private Animator animator;
        private float directionX;
        private float directionY;

        private void Start()
        {
            playerController = FindObjectOfType<PlayerController>();

            target = Waypoints.points[0];
            animator = GetComponent<Animator>();
            SetAnimationForDirection(target.position - transform.position);
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
                SetAnimationForDirection(target.position - transform.position);
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

        private void SetAnimationForDirection(Vector2 direction)
        {
            direction.Normalize();
            directionX = Mathf.RoundToInt(direction.x);
            directionY = Mathf.RoundToInt(direction.y);

            animator.SetFloat("directionX", directionX);
            animator.SetFloat("directionY", directionY);
        }
    }
}
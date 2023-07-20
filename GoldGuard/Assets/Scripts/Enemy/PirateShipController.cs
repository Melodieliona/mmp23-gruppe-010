using Player;
using UnityEngine;

namespace Enemy
{
    public class PirateShipController : MonoBehaviour
    {
        private static readonly int DirectionX = Animator.StringToHash("directionX");
        private static readonly int DirectionY = Animator.StringToHash("directionY");

        [Header("References")]
        private PlayerController playerController;

        [Header("Attributes")]
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private double currentHealth;

        private double maxHealth;
        private readonly float speed;
        private readonly int scoreValue;
        private readonly int goldValue;

        protected PirateShipController(double maxHealth, float speed, int scoreValue, int goldValue)
        {
            this.maxHealth = maxHealth;
            this.speed = speed;
            this.scoreValue = scoreValue;
            this.goldValue = goldValue;
        }

        private int waypointIndex = 0;
        private Transform target;

        private AnimationClip[] animationClips;
        private Animator animator;

        private void Start()
        {
            playerController = FindObjectOfType<PlayerController>();

            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth, maxHealth);

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

        private void SetAnimationForDirection(Vector2 direction)
        {
            direction.Normalize();
            animator.SetFloat(DirectionX, Mathf.RoundToInt(direction.x));
            animator.SetFloat(DirectionY, Mathf.RoundToInt(direction.y));
        }

        public void Damage(double amount)
        {
            currentHealth -= amount;
            healthBar.SetHealth(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                ShipSpawner.EnemyDestroyEvent.Invoke();
                Destroy(gameObject);
                playerController.AddScore(scoreValue);
                playerController.AddGold(goldValue);
            }
        }

        public void MultiplyHp(float factor)
        {
            currentHealth *= factor;
            maxHealth *= factor;
        }

        public double GetMaxHealth()
        {
            return maxHealth;
        }
    }
}
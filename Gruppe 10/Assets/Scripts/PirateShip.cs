using UnityEngine;

public class PirateShip : MonoBehaviour, IDamageable
{
    [Header("Attributes")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private double health = 2;
    [SerializeField] private int scoreValue = 1;
    [SerializeField] private int goldValue = 1;

    public Sprite[] sprites;

    [Header("References")]
    private PlayerManager playerManager;

    private Transform target;
    private int waypointIndex = 0;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();

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
            playerManager.AddScore(scoreValue);
            playerManager.AddGold(goldValue);
        }
    }

    public double GetHealth()
    {
        return health;
    }

    /// Sets the sprite based on the direction.
    private void SetSpriteForDirection(Vector2 direction)
    {
        // Suche das entsprechende Sprite basierend auf dem Winkel
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        int spriteIndex = 0;

        switch (angle)
        {
            case >= 45f and < 135f:
                spriteIndex = 1; // nach unten
                break;
            case >= 135f:
            case < -135f:
                spriteIndex = 2; // nach links
                break;
            case >= -135f and < -45f:
                spriteIndex = 3; // nach oben
                break;
        }

        spriteRenderer.sprite = sprites[spriteIndex];
    }
}
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class PirateShip : MonoBehaviour
{
    [Header("Attributes")] 
    [SerializeField] private float speed = 3f;

    private Transform target;
    private int waypointIndex = 0;

    private void Start()
    {
        target = Waypoints.points[0];
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
}
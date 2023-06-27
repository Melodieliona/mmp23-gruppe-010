using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class PirateShip : MonoBehaviour
{
    public float speed = 3f;

    private Transform target;
    private int waypointIndex = 0;

    void Start()
    {
        this.target = Waypoints.points[0];
    }

    /// <summary>
    /// Moves the ship in the direction of the targeted waypoint.
    /// On reaching the target, the next waypoint is set.
    /// </summary>
    void Update()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (Vector2.Distance(target.position, transform.position) <= 0.05f)
        {
            GetNextWaypoint();
        }
    }

    /// <summary>
    /// Attempts to get the next waypoint to target.
    /// If there are no more waypoints available, the ship has reached the treasure chest
    /// and will therefore be destroyed, invoking <see cref="ShipSpawner.onReachTreasureChest"/>.
    /// </summary>
    private void GetNextWaypoint()
    {
        this.waypointIndex++;

        if (waypointIndex >= Waypoints.points.Length)
        {
            ShipSpawner.onReachTreasureChest.Invoke();
            Destroy(gameObject);
            return;
        }

        this.target = Waypoints.points[waypointIndex];
    }
}

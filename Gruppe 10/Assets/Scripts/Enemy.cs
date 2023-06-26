using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1.5f;

    private Transform target;
    private int waypointIndex = 0;

    void Start()
    {
        this.target = Waypoints.points[waypointIndex];
    }

    void Update()
    {
        Vector2 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector2.Distance(transform.position, target.position) <= 0.1f)
        {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length)
        {
            Destroy(gameObject);
            return;
        }

        this.waypointIndex++;
        this.target = Waypoints.points[waypointIndex];
    }
}

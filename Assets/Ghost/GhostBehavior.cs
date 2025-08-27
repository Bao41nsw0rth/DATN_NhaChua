using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    public PathGroup pathGroup; // Nhóm waypoint
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float waypointThreshold = 0.1f;

    [Header("Detection Zone")]
    public Collider detectionZone; // Vùng trigger 3D để ghost kích hoạt

    private int currentWaypointIndex = 0;
    private bool isActive = false;

    void Start()
    {
        if (pathGroup == null || pathGroup.waypoints.Count == 0)
        {
            Debug.LogError("PathGroup chưa gán waypoint!");
            enabled = false;
            return;
        }

        transform.position = pathGroup.waypoints[0].position;
    }

    void Update()
    {
        if (isActive)
            Patrol();
    }

    void Patrol()
    {
        Transform targetWaypoint = pathGroup.waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
        RotateTowards(direction);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % pathGroup.waypoints.Count;
        }
    }

    void RotateTowards(Vector3 direction)
    {
        Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);
        if (flatDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isActive = true;
    }

    // Vẽ Gizmos cho Scene view
    private void OnDrawGizmos()
    {
        // Vẽ vùng trigger nếu có
        if (detectionZone != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(detectionZone.bounds.center, detectionZone.bounds.size);
        }

        // Vẽ đường waypoint
        if (pathGroup != null && pathGroup.waypoints.Count > 1)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < pathGroup.waypoints.Count - 1; i++)
            {
                if (pathGroup.waypoints[i] != null && pathGroup.waypoints[i + 1] != null)
                {
                    Gizmos.DrawLine(pathGroup.waypoints[i].position, pathGroup.waypoints[i + 1].position);

                    // Vẽ mũi tên nhỏ chỉ hướng
                    Vector3 dir = (pathGroup.waypoints[i + 1].position - pathGroup.waypoints[i].position).normalized;
                    Vector3 arrowPos = pathGroup.waypoints[i].position + dir * 0.5f;
                    Gizmos.DrawRay(arrowPos, dir * 0.5f);
                }
            }
        }
    }
}

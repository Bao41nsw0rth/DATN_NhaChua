using UnityEngine;

public class GhostPatrol : MonoBehaviour
{
    public PathGroup pathGroup;
    public float speed = 3f;
    public float rotationSpeed = 5f;
    public float waypointThreshold = 0.1f;

    public string playerTag = "Player"; // tag của Player

    private int currentWaypointIndex = 0;

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
        if (pathGroup == null || pathGroup.waypoints.Count == 0) return;

        Transform targetWaypoint = pathGroup.waypoints[currentWaypointIndex];

        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Quay mặt theo hướng đi
        if (direction.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % pathGroup.waypoints.Count;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        PlayerHistory playerHistory = other.GetComponent<PlayerHistory>();
        if (playerHistory != null)
        {
            // Đặt Player về vị trí cách 15 giây trước
            other.transform.position = playerHistory.GetPositionAgo();
            // Reset velocity nếu có Rigidbody
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}

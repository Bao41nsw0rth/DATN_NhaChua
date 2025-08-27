using UnityEngine;

public class GhostPatrol : MonoBehaviour
{
    public PathGroup pathGroup;
    public float speed = 10f;
    public float rotationSpeed = 5f;
    public float waypointThreshold = 0.1f;

    [Header("Player Detection")]
    public string playerTag = "Player";
    public float viewAngle = 180f; // Góc nhìn
    public float viewDistance = 50f; // Khoảng cách nhìn

    private int currentWaypointIndex = 0;
    private Transform player;
    private bool isChasing = false;

    void Start()
    {
        if (pathGroup == null || pathGroup.waypoints.Count == 0)
        {
            Debug.LogError("PathGroup chưa gán waypoint!");
            enabled = false;
            return;
        }

        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;
        transform.position = pathGroup.waypoints[0].position;
    }

    void Update()
    {
        if (player == null) return;

        if (CanSeePlayer())
        {
            isChasing = true;
        }
        else if (isChasing)
        {
            // Nếu đang đuổi nhưng mất dấu, quay lại waypoint gần nhất
            isChasing = false;
            currentWaypointIndex = FindNearestWaypointIndex();
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (pathGroup == null || pathGroup.waypoints.Count == 0) return;

        Transform targetWaypoint = pathGroup.waypoints[currentWaypointIndex];

        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        RotateTowards(direction);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % pathGroup.waypoints.Count;
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        RotateTowards(direction);
    }

void RotateTowards(Vector3 direction)
{
    Vector3 flatDirection = new Vector3(direction.x, 0, direction.z); // Xoá trục Y
    if (flatDirection.sqrMagnitude > 0.001f)
    {
        Quaternion targetRotation = Quaternion.LookRotation(flatDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position);
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > viewDistance) return false;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < viewAngle * 0.5f; // vì 180° chia hai mỗi bên 90°
    }

    int FindNearestWaypointIndex()
    {
        int nearestIndex = 0;
        float minDistance = float.MaxValue;

        for (int i = 0; i < pathGroup.waypoints.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, pathGroup.waypoints[i].position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearestIndex = i;
            }
        }

        return nearestIndex;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        PlayerHistory playerHistory = other.GetComponent<PlayerHistory>();
        if (playerHistory != null)
        {
            other.transform.position = playerHistory.GetPositionAgo();
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Hiển thị tầm nhìn khi chọn Ghost trong Scene
        Gizmos.color = Color.red;

        // Vẽ bán kính tầm nhìn
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        // Vẽ hình quạt 2 đường biên (góc nhìn)
        Vector3 forward = transform.forward;
        Quaternion leftRayRotation = Quaternion.Euler(0, -viewAngle / 2, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, viewAngle / 2, 0);

        Vector3 leftRay = leftRayRotation * forward * viewDistance;
        Vector3 rightRay = rightRayRotation * forward * viewDistance;

        Gizmos.DrawLine(transform.position, transform.position + leftRay);
        Gizmos.DrawLine(transform.position, transform.position + rightRay);
    }
    void LateUpdate()
    {
        // Khóa trục X
        Vector3 euler = transform.eulerAngles;
        euler.x = 90f;
        transform.eulerAngles = euler;
    }
}

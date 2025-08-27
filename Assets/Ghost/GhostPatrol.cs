using UnityEngine;

public class GhostPatrol : MonoBehaviour
{
    public PathGroup pathGroup;
    public float speed = 10f;
    public float rotationSpeed = 5f;
    public float waypointThreshold = 0.1f;

    [Header("Return settings")]
    [Tooltip("Tốc độ khi ma di chuyển trở lại đường tuần tra. Nếu <= 0 thì bằng `speed`.")]
    public float returnSpeed = 0f;

    [Header("Player Detection")]
    public string playerTag = "Player";
    public float viewAngle = 180f; // Góc nhìn
    public float viewDistance = 50f; // Khoảng cách nhìn

    private int currentWaypointIndex = 0;
    private Transform player;
    private bool isChasing = false;

    // Biến lưu vị trí gần nhất trên đoạn
    private Vector3 nearestPointOnPath;

    // Trạng thái trả về đường tuần tra
    private bool returningToPath = false;
    private Vector3 returnTarget;
    private int chosenWaypointAfterReturn = 0;

    // Dùng để biết hướng di chuyển trước đó (tránh quay ngược)
    private Vector3 lastPosition;
    private Vector3 lastMovementDir = Vector3.forward;

    void Start()
    {
        if (pathGroup == null || pathGroup.waypoints.Count == 0)
        {
            Debug.LogError("PathGroup chưa gán waypoint!");
            enabled = false;
            return;
        }

        if (returnSpeed <= 0f) returnSpeed = speed;

        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;
        transform.position = pathGroup.waypoints[0].position;

        lastPosition = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        // Cập nhật hướng di chuyển trước đó (từ frame trước)
        Vector3 frameMove = transform.position - lastPosition;
        if (frameMove.sqrMagnitude > 0.0001f)
            lastMovementDir = frameMove.normalized;

        if (CanSeePlayer())
        {
            // Nếu nhìn thấy player -> truy đuổi (hủy việc đang trở lại đường)
            isChasing = true;
            returningToPath = false;
        }
        else if (isChasing)
        {
            // Nếu đang đuổi nhưng mất dấu, bắt đầu quay lại đường tuần tra một cách mượt
            isChasing = false;

            float tOnSegment;
            int nearestIndex = FindNearestWaypointIndex(out tOnSegment); // Lưu nearestPointOnPath trong hàm

            int nextIndex = (nearestIndex + 1) % pathGroup.waypoints.Count;
            Vector3 segmentDir = (pathGroup.waypoints[nextIndex].position - pathGroup.waypoints[nearestIndex].position).normalized;

            // Quyết định có nên tiếp tục hướng của đoạn (nextIndex) hay quay về nearestIndex
            bool preferNext = false;

            // 1) Nếu hướng di chuyển trước đó cùng chiều với đoạn -> ưu tiên next
            float dotSeg = Vector3.Dot(lastMovementDir, segmentDir);

            if (dotSeg > 0.1f)
            {
                preferNext = true;
            }
            else
            {
                // 2) Nếu điểm gần nhất nằm gần đầu đoạn (t > 0.5) -> ưu tiên next
                if (tOnSegment > 0.5f) preferNext = true;
            }

            int chosen = preferNext ? nextIndex : nearestIndex;

            // Bắt đầu di chuyển từ từ tới điểm gần nhất trên đoạn (không teleport)
            returnTarget = nearestPointOnPath;
            chosenWaypointAfterReturn = chosen;
            returningToPath = true;
        }

        // Hành vi chính: truy đuổi, trả về đường, hoặc tuần tra
        if (isChasing)
        {
            ChasePlayer();
        }
        else if (returningToPath)
        {
            ReturnToPath();
        }
        else
        {
            Patrol();
        }

        // Cập nhật lastPosition cuối frame
        lastPosition = transform.position;
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

    void ReturnToPath()
    {
        // Di chuyển mượt về điểm gần nhất trên đoạn
        Vector3 direction = (returnTarget - transform.position);
        float dist = direction.magnitude;

        if (dist <= waypointThreshold)
        {
            // Đã về tới điểm trên đường, kết thúc trả về và tiếp tục tuần tra
            returningToPath = false;
            currentWaypointIndex = chosenWaypointAfterReturn;
            return;
        }

        Vector3 dirNorm = direction.normalized;
        transform.position = Vector3.MoveTowards(transform.position, returnTarget, returnSpeed * Time.deltaTime);
        RotateTowards(dirNorm);
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

    // Trả về index waypoint đầu của đoạn và giá trị t (0..1) vị trí trên đoạn
    int FindNearestWaypointIndex(out float outT)
    {
        int nearestIndex = 0;
        float minDistance = float.MaxValue;
        outT = 0f;

        for (int i = 0; i < pathGroup.waypoints.Count - 1; i++)
        {
            Vector3 a = pathGroup.waypoints[i].position;
            Vector3 b = pathGroup.waypoints[i + 1].position;

            float t;
            Vector3 closestPoint = GetClosestPointOnSegment(a, b, transform.position, out t);
            float dist = Vector3.Distance(transform.position, closestPoint);

            if (dist < minDistance)
            {
                minDistance = dist;
                nearestIndex = i;
                nearestPointOnPath = closestPoint; // Lưu điểm gần nhất
                outT = t;
            }
        }

        return nearestIndex;
    }

    Vector3 GetClosestPointOnSegment(Vector3 a, Vector3 b, Vector3 point, out float t)
    {
        Vector3 ab = b - a;
        float denom = ab.sqrMagnitude;
        if (denom == 0f)
        {
            t = 0f;
            return a;
        }

        t = Vector3.Dot(point - a, ab) / denom;
        t = Mathf.Clamp01(t);
        return a + t * ab;
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

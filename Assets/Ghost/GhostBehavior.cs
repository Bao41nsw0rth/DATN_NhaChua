using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    [Header("Path Settings")]
    public PathGroup pathGroup;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float waypointThreshold = 0.1f;

    [Header("Detection Settings")]
    public string playerTag = "Player";
    public float detectAngle = 90f;    // Góc nhìn (vùng 1)
    public float detectDistance = 10f; // Phạm vi nhìn thấy Player (vùng 1)
    public float chaseDistance = 5f;   // Khoảng cách để bắt đầu di chuyển theo waypoint (vùng 2)

    [Header("Animation")]
    public Animator animator;
    public string animIdle = "Idle";  // Animation 1
    public string animRun = "Run";    // Animation 2
    public string animEnd = "End";    // Animation 3

    private Transform player;
    private int currentWaypointIndex = 0;
    private bool isChasing = false;
    private bool isEnding = false;

    void Start()
    {
        if (pathGroup == null || pathGroup.waypoints.Count == 0)
        {
            Debug.LogError("Chưa gán PathGroup hoặc waypoint trống!");
            enabled = false;
            return;
        }

        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;
        if (animator != null) animator.Play(animIdle); // animation 1 khi bắt đầu
    }

    void Update()
    {
        if (isEnding) return; // Nếu đang kết thúc thì không làm gì nữa

        if (!isChasing)
        {
            DetectPlayer();
        }
        else
        {
            Patrol();
        }
    }

    void DetectPlayer()
    {
        if (player == null) return;

        Vector3 dirToPlayer = player.position - transform.position;
        float distance = dirToPlayer.magnitude;

        if (distance <= detectDistance)
        {
            // Kiểm tra góc nhìn
            float angle = Vector3.Angle(transform.forward, dirToPlayer);
            if (angle <= detectAngle * 0.5f)
            {
                // Kích hoạt animation1 (Idle/Hù dọa)
                if (animator != null) animator.Play(animIdle);

                // Nếu Player vào vùng 2 → bắt đầu di chuyển theo waypoint
                if (distance <= chaseDistance)
                {
                    isChasing = true;
                    if (animator != null) animator.Play(animRun); // animation 2
                }
            }
        }
    }

    void Patrol()
    {
        if (pathGroup == null || pathGroup.waypoints.Count == 0) return;

        Transform target = pathGroup.waypoints[currentWaypointIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        RotateTowards(direction);

        if (Vector3.Distance(transform.position, target.position) < waypointThreshold)
        {
            currentWaypointIndex++;

            // Nếu tới waypoint cuối → phát animation3 rồi hủy
            if (currentWaypointIndex >= pathGroup.waypoints.Count)
            {
                StartCoroutine(EndSequence());
            }
        }
    }

    System.Collections.IEnumerator EndSequence()
    {
        isEnding = true;
        if (animator != null) animator.Play(animEnd); // animation 3
        // Đợi animation 3 chạy xong (giả sử dài 2 giây)
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
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

    private void OnDrawGizmosSelected()
    {
        // Vẽ vùng phát hiện (vùng 1)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectDistance);

        // Vẽ vùng di chuyển (vùng 2)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        // Vẽ góc nhìn
        Vector3 forward = transform.forward;
        Quaternion leftRayRotation = Quaternion.Euler(0, -detectAngle / 2, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, detectAngle / 2, 0);

        Vector3 leftRay = leftRayRotation * forward * detectDistance;
        Vector3 rightRay = rightRayRotation * forward * detectDistance;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftRay);
        Gizmos.DrawLine(transform.position, transform.position + rightRay);
    }
}

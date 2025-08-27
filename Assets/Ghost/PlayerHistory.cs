using UnityEngine;

public class PlayerHistory : MonoBehaviour
{
    [Tooltip("Thời gian choáng khi trở về vị trí ban đầu (giây)")]
    public float stunDuration = 2f;

    private Vector3 initialPosition;
    private bool isStunned = false;
    private float stunEndTime;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (isStunned)
        {
            if (Time.time >= stunEndTime)
            {
                isStunned = false;
                // Bật lại script di chuyển nếu cần
                // GetComponent<PlayerMovement>().enabled = true;
            }
        }
    }

    // Quay về vị trí ban đầu
    public void ReturnToInitialPosition()
    {
        transform.position = initialPosition;
        Stun(stunDuration);
    }

    // Gây choáng
    private void Stun(float duration)
    {
        isStunned = true;
        stunEndTime = Time.time + duration;
        // GetComponent<PlayerMovement>().enabled = false;
    }

    // Giữ lại hàm này để tránh lỗi (trả về vị trí ban đầu)
    public Vector3 GetPositionAgo()
    {
        return initialPosition;
    }
}

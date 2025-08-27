using UnityEngine;

public class GhostDisappear : MonoBehaviour
{
    public Transform player;       // G?n Player vào ðây trong Inspector
    public float disappearDistance = 5f;  // Kho?ng cách ð? bi?n m?t

    void Update()
    {
        if (player == null) return;

        // Tính kho?ng cách gi?a Player và Ghost
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < disappearDistance)
        {
            Destroy(gameObject);
        }
    }
}

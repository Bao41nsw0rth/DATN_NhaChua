using UnityEngine;

public class GhostDisappear : MonoBehaviour
{
    public Transform player;       // G?n Player v�o ��y trong Inspector
    public float disappearDistance = 5f;  // Kho?ng c�ch �? bi?n m?t

    void Update()
    {
        if (player == null) return;

        // T�nh kho?ng c�ch gi?a Player v� Ghost
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < disappearDistance)
        {
            Destroy(gameObject);
        }
    }
}

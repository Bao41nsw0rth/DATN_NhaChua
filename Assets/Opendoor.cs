using UnityEngine;

public class RotateOnCollision : MonoBehaviour
{
    private bool isRotating = false; // Trạng thái quay
    private float rotationSpeed = 500f; // Tốc độ quay (độ/giây)
    private float rotationTimer = 0f; // Bộ đếm thời gian
    private float rotationDuration; // Thời gian quay (ngẫu nhiên từ 1 đến 4 giây)

    void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra nếu va chạm với GameObject có tag "keyT"
        if (collision.gameObject.CompareTag("keyT"))
        {
            isRotating = true; // Bắt đầu quay
            rotationDuration = Random.Range(1f, 4f); // Chọn ngẫu nhiên thời gian quay từ 1 đến 4 giây
            rotationTimer = 0f; // Reset bộ đếm
        }
    }

    void Update()
    {
        if (isRotating)
        {
            // Quay GameObject quanh trục Y (hoặc trục bạn muốn)
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

            // Tăng bộ đếm thời gian
            rotationTimer += Time.deltaTime;

            // Dừng quay khi đạt thời gian ngẫu nhiên
            if (rotationTimer >= rotationDuration)
            {
                isRotating = false;
            }
        }
    }
}
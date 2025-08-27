using UnityEngine;

public class RotateOnCollision : MonoBehaviour
{
    private bool isRotating = false; // Trạng thái quay
    private float rotationSpeed = 50f; // Tốc độ quay (độ/giây)
    private float rotationTimer = 0f; // Bộ đếm thời gian
    private float rotationDuration = 2f; // Thời gian quay cố định (2 giây)

    void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra nếu va chạm với GameObject có tag "keyT"
        if (collision.gameObject.CompareTag("keyT"))
        {
            isRotating = true; // Bắt đầu quay
            rotationTimer = 0f; // Đặt lại bộ đếm thời gian
        }
    }

    void Update()
    {
        if (isRotating)
        {
            // Quay GameObject quanh trục Y
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

            // Tăng bộ đếm thời gian
            rotationTimer += Time.deltaTime;

            // Dừng quay khi đạt thời gian cố định
            if (rotationTimer >= rotationDuration)
            {
                isRotating = false;
            }
        }
    }
}
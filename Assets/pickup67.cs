using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class DragAndDrop : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private Camera mainCamera;
    private Vector3 offset;
    private float zCoord;
    private bool isDragging = false;

    void Start()
    {
        // Lấy tham chiếu đến Rigidbody và Collider
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        mainCamera = Camera.main;

        // Đảm bảo Rigidbody sử dụng trọng lực ban đầu
        rb.useGravity = true;
    }

    void Update()
    {
        // Kiểm tra nhấn chuột phải để bắt đầu giữ vật phẩm
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Kiểm tra xem chuột có chạm vào vật phẩm không
            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                isDragging = true;
                rb.useGravity = false; // Tắt trọng lực khi giữ
                rb.isKinematic = true; // Chuyển sang kinematic để di chuyển mượt mà

                // Tính toán offset để vật phẩm "giữ trên tay" (theo chuột)
                zCoord = mainCamera.WorldToScreenPoint(transform.position).z;
                offset = transform.position - GetMouseWorldPos();
            }
        }

        // Thả chuột phải để thả vật phẩm
        if (Input.GetMouseButtonUp(1) && isDragging)
        {
            isDragging = false;
            rb.useGravity = true; // Bật lại trọng lực để vật phẩm rơi
            rb.isKinematic = false; // Tắt kinematic để vật phẩm chịu lực vật lý
        }
    }

    void FixedUpdate()
    {
        // Di chuyển vật phẩm theo chuột khi đang giữ
        if (isDragging)
        {
            Vector3 newPos = GetMouseWorldPos() + offset;
            rb.MovePosition(newPos); // Di chuyển mượt mà theo vị trí chuột
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        // Lấy vị trí chuột trong không gian 3D
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    // Xử lý va chạm khi vật phẩm được thả
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Vật phẩm {gameObject.name} đã va chạm với {collision.gameObject.name}");
    }
}
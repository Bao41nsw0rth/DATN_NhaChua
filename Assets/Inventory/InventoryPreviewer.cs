using UnityEngine;

public class InventoryPreviewer : MonoBehaviour
{
    [SerializeField] private float speed = 5f;         // Tốc độ xoay khi kéo chuột
    [SerializeField] private float returnSpeed = 2f;   // Tốc độ quay về ban đầu
    
    public Transform previewRoot;
    private GameObject currentItem;
    private Quaternion previewRotation;

    private Vector3 lastMousePosition;
    private bool isDragging = false;

    public void ShowItem(GameObject itemPrefab, Quaternion rotation)
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
        }

        currentItem = Instantiate(itemPrefab, previewRoot); 
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = rotation;

        previewRotation = rotation;
    }

    private void Update()
    {
        if (currentItem == null) return;

        if (Input.GetMouseButtonDown(0)) // Bắt đầu giữ chuột
        {
            lastMousePosition = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButton(0)) // Khi đang giữ chuột
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            // Xoay item theo trục Y (ngang) và X (dọc)
            float rotX = -delta.y * speed; 
            float rotY = -delta.x * speed;

            currentItem.transform.Rotate(Vector3.up, rotY, Space.World);
            currentItem.transform.Rotate(Vector3.right, rotX, Space.World);

            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0)) // Thả chuột
        {
            isDragging = false;
        }

        // Khi không giữ chuột => từ từ quay về gốc
        if (!isDragging)
        {
            currentItem.transform.localRotation = Quaternion.Slerp(
                currentItem.transform.localRotation,
                previewRotation,
                Time.deltaTime * returnSpeed
            );
        }
    }
}
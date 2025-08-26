using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    // Tham chiếu đến GameObject cần bật/tắt
    public GameObject targetObject;

    // Start được gọi khi script khởi tạo
    void Start()
    {
        // Đảm bảo GameObject đã được gán
        if (targetObject == null)
        {
            Debug.LogError("Target GameObject chưa được gán!");
        }
    }

    // Update được gọi mỗi frame
    void Update()
    {
        // Kiểm tra xem phím E có được nhấn không
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Bật/tắt GameObject (đảo ngược trạng thái hiện tại)
            if (targetObject != null)
            {
                targetObject.SetActive(!targetObject.activeSelf);
            }
        }
    }
}
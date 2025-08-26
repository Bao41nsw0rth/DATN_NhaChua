using UnityEngine;

public class LetterInteraction : MonoBehaviour
{
    public GameObject objectToToggle; // GameObject 11 cần bật/tắt
    public Transform player; // Transform của người chơi
    public float interactionDistance = 3f; // Khoảng cách tối đa để tương tác
    private Camera mainCamera;
    private bool isPlayerNear = false;

    void Start()
    {
        mainCamera = Camera.main;
        // Đảm bảo objectToToggle ban đầu được tắt
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(false);
        }
    }

    void Update()
    {
        // Kiểm tra khoảng cách giữa người chơi và lá thư
        isPlayerNear = Vector3.Distance(player.position, transform.position) <= interactionDistance;

        // Nếu người chơi ở gần
        if (isPlayerNear)
        {
            // Kiểm tra raycast từ con trỏ chuột
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Kiểm tra nếu raycast trúng GameObject lá thư
                if (hit.transform == transform)
                {
                    // Nhấn phím E để bật/tắt GameObject 11
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (objectToToggle != null)
                        {
                            objectToToggle.SetActive(!objectToToggle.activeSelf);
                        }
                    }
                }
            }
        }
        else
        {
            // Nếu người chơi không ở gần, tắt GameObject 11
            if (objectToToggle != null && objectToToggle.activeSelf)
            {
                objectToToggle.SetActive(false);
            }
        }
    }

    // Hiển thị khoảng cách tương tác trong editor (gizmos)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    // Gán GameObject cần xóa trong Inspector
    public GameObject objectToDestroy;

    void Update()
    {
        // Kiểm tra nếu phím P được nhấn
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Xóa GameObject nếu nó tồn tại
            if (objectToDestroy != null)
            {
                Destroy(objectToDestroy);
            }
        }
    }
}
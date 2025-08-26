using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    void Start()
    {
        // Tự động hủy game object sau 3 giây
        Destroy(gameObject, 3f);
    }
}
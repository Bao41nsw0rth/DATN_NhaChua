using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate; // GameObject cần kích hoạt

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu đối tượng va chạm có tag "Player"
        if (other.CompareTag("Player"))
        {
            // Kích hoạt GameObject nếu nó tồn tại
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }
            // Phá hủy GameObject chứa script này
            Destroy(gameObject);
        }
    }
}


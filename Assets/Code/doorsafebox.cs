using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorsafebox : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        // Lấy component Rigidbody của object
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra nếu va chạm với object có tên "tad key"
        if (collision.gameObject.name == "keysafe")
        {
            // Tắt IsKinematic
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }
}

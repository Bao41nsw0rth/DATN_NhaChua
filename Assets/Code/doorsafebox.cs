using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorsafebox : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource audioSource;
    public AudioClip destroySound; // Gán âm thanh phá hủy trong Inspector

    void Start()
    {
        // Lấy component Rigidbody của object
        rb = GetComponent<Rigidbody>();

        // Lấy hoặc thêm AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra nếu va chạm với object có tên "keysafe"
        if (collision.gameObject.name == "keysafe")
        {
            // Tắt IsKinematic
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Phá hủy game object "keysafe"
            Destroy(collision.gameObject);

            // Phát âm thanh phá hủy
            if (destroySound != null && audioSource != null)
            {
                audioSource.PlayOneShot(destroySound);
            }
        }
    }
}
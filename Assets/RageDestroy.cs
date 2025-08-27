using UnityEngine;

public class RageDestroy : MonoBehaviour
{
    public AudioClip rageSound; // Âm thanh sẽ phát
    private AudioSource audioSource;
    public float detectionRange = 7f; // Phạm vi phát hiện 10m
    private bool hasTriggered = false; // Tránh lặp lại trigger

    void Start()
    {
        // Lấy hoặc thêm AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = rageSound;
    }

    void Update()
    {
        // Tìm tất cả các đối tượng có tag "keyT"
        GameObject[] targets = GameObject.FindGameObjectsWithTag("keyT");

        foreach (GameObject target in targets)
        {
            // Tính khoảng cách
            float distance = Vector3.Distance(transform.position, target.transform.position);

            // Kiểm tra nếu trong phạm vi 10m và chưa trigger
            if (distance <= detectionRange && !hasTriggered)
            {
                hasTriggered = true;
                // Phát âm thanh
                if (rageSound != null)
                {
                    audioSource.Play();
                }
                // Hủy đối tượng keyT sau 1 giây
                Destroy(target, 15f);
                // Hủy chính bản thân sau 2 giây
                Destroy(gameObject, 3f);
            }
        }
    }
}
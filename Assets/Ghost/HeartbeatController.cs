using UnityEngine;

public class HeartbeatController : MonoBehaviour
{
    [Header("Âm thanh tim đập")]
    public AudioClip heartbeatClip;       // Gắn clip âm thanh ở đây
    public float maxVolume = 1f;          // Âm lượng khi gần ghost
    public float minVolume = 0.05f;       // Âm lượng khi xa
    public float maxPitch = 1.8f;         // Nhịp tim nhanh nhất
    public float minPitch = 0.8f;         // Nhịp tim chậm nhất
    public float detectionRadius = 50f;   // Khoảng cách ghost ảnh hưởng

    private AudioSource heartbeatAudio;

    void Start()
    {
        heartbeatAudio = gameObject.GetComponent<AudioSource>();
        if (heartbeatAudio == null)
            heartbeatAudio = gameObject.AddComponent<AudioSource>();

        heartbeatAudio.clip = heartbeatClip;
        heartbeatAudio.loop = true;
        heartbeatAudio.playOnAwake = false;
        heartbeatAudio.volume = minVolume;
        heartbeatAudio.pitch = minPitch;
        heartbeatAudio.Play();
    }

    void Update()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject ghost in ghosts)
        {
            float distance = Vector3.Distance(transform.position, ghost.transform.position);
            if (distance < closestDistance)
                closestDistance = distance;
        }

        if (closestDistance <= detectionRadius)
        {
            float t = 1 - (closestDistance / detectionRadius);
            heartbeatAudio.volume = Mathf.Lerp(minVolume, maxVolume, t);
            heartbeatAudio.pitch = Mathf.Lerp(minPitch, maxPitch, t);
        }
        else
        {
            heartbeatAudio.volume = Mathf.Lerp(heartbeatAudio.volume, minVolume, Time.deltaTime * 2f);
            heartbeatAudio.pitch = Mathf.Lerp(heartbeatAudio.pitch, minPitch, Time.deltaTime * 2f);
        }
    }

    // Vẽ vòng Gizmos hiển thị detectionRadius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Màu vòng tròn
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

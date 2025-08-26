using UnityEngine;

public class IncreaseVolumeOnHover : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip soundClip; // Gán âm thanh trong Inspector
    public float maxVolume = 1f; // Âm lượng tối đa
    public float volumeIncreaseRate = 0.2f; // Tốc độ tăng âm lượng mỗi giây
    private float hoverTime = 0f; // Thời gian chuột hover
    private bool isHovering = false;

    void Start()
    {
        // Lấy hoặc thêm AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = soundClip;
        audioSource.volume = 0f; // Âm lượng ban đầu
        audioSource.playOnAwake = false;
        audioSource.loop = true; // Lặp âm thanh
    }

    void Update()
    {
        if (isHovering)
        {
            // Tăng thời gian hover
            hoverTime += Time.deltaTime;
            // Tăng âm lượng dựa trên thời gian hover
            audioSource.volume = Mathf.Clamp(hoverTime * volumeIncreaseRate, 0f, maxVolume);

            // Phát âm thanh nếu chưa phát
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    void OnMouseEnter()
    {
        isHovering = true;
    }

    void OnMouseExit()
    {
        isHovering = false;
        hoverTime = 0f; // Reset thời gian hover
        audioSource.volume = 0f; // Reset âm lượng
        audioSource.Stop(); // Dừng âm thanh
    }
}
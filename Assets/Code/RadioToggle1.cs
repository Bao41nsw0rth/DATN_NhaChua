using UnityEngine;

public class RadioToggle : MonoBehaviour
{
    public AudioSource radioAudio;
    public KeyCode toggleKey = KeyCode.E;
    public float interactDistance = 3f;  // khoảng cách có thể bật/tắt
    public float maxDistance = 10f;      // khoảng cách tối đa nghe âm thanh

    private Transform player;
    public GameObject interactUI;        // UI "Nhấn E để bật/tắt radio"

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (interactUI != null)
            interactUI.SetActive(false);

        if (radioAudio != null)
        {
            radioAudio.loop = true; // phát liên tục
            radioAudio.volume = 0f; // bắt đầu tắt
        }
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        // Hiển thị prompt nếu trong khoảng bật/tắt
        if (dist <= interactDistance)
        {
            if (interactUI != null)
                interactUI.SetActive(true);

            if (Input.GetKeyDown(toggleKey))
            {
                if (radioAudio.isPlaying)
                    radioAudio.Pause();
                else
                    radioAudio.Play();
            }
        }
        else
        {
            if (interactUI != null)
                interactUI.SetActive(false);
        }

        // Điều chỉnh âm lượng dựa trên khoảng cách
        if (radioAudio != null && radioAudio.isPlaying)
        {
            if (dist <= maxDistance)
            {
                // volume = 1 khi sát radio, giảm linearly đến 0 khi xa maxDistance
                radioAudio.volume = 1f - (dist / maxDistance);
                radioAudio.volume = Mathf.Clamp01(radioAudio.volume); // đảm bảo 0-1
            }
            else
            {
                radioAudio.volume = 0f;
            }
        }
    }
}

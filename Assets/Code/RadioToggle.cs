using UnityEngine;
using UnityEngine.UI; // Nếu bạn dùng Text thường
// using TMPro; // Nếu bạn dùng TextMeshPro

public class RadioToggle : MonoBehaviour
{
    public AudioSource radioAudio;
    public KeyCode toggleKey = KeyCode.E;
    public float interactDistance = 3f;
    private Transform player;

    public GameObject interactUI; // Gán UI Text vào đây

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (interactUI != null)
            interactUI.SetActive(false); // Ẩn UI khi bắt đầu
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        if (dist < interactDistance)
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
    }
}

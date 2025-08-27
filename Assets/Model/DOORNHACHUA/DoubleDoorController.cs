using UnityEngine;
using TMPro; // Nếu dùng TextMeshPro

public class DoubleDoorController : MonoBehaviour
{
    [Header("Door Pivots")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Door Settings")]
    public float openAngle = 90f;
    public float speed = 2f;
    public float interactDistance = 3f;

    [Header("UI")]
    public TextMeshProUGUI interactPrompt; // Text hiển thị prompt

    [Header("Colliders")]
    public Collider leftCollider;
    public Collider rightCollider;

    [Header("Audio")]
    public AudioSource doorAudio; // gắn AudioSource vào đây
    public AudioClip doorSound;   // âm thanh mở/đóng cửa (chung 1 file)

    private bool isOpen = false;
    private Quaternion leftClosedRot, leftOpenRot;
    private Quaternion rightClosedRot, rightOpenRot;

    void Start()
    {
        leftClosedRot = leftDoor.rotation;
        leftOpenRot = Quaternion.Euler(leftDoor.eulerAngles + new Vector3(0, -openAngle, 0));

        rightClosedRot = rightDoor.rotation;
        rightOpenRot = Quaternion.Euler(rightDoor.eulerAngles + new Vector3(0, openAngle, 0));

        if (interactPrompt != null)
            interactPrompt.gameObject.SetActive(false); // ẩn lúc đầu
    }

    void Update()
    {
        // Xoay cửa
        leftDoor.rotation = Quaternion.Slerp(leftDoor.rotation, isOpen ? leftOpenRot : leftClosedRot, Time.deltaTime * speed);
        rightDoor.rotation = Quaternion.Slerp(rightDoor.rotation, isOpen ? rightOpenRot : rightClosedRot, Time.deltaTime * speed);

        // Kiểm tra player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance <= interactDistance)
            {
                // Hiển thị prompt
                if (interactPrompt != null)
                {
                    interactPrompt.gameObject.SetActive(true);
                    interactPrompt.text = isOpen ? "Nhấn E để đóng cửa" : "Nhấn E để mở cửa";
                }

                // Nhấn E để mở/đóng
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isOpen = !isOpen;

                    // Bật/tắt colliders
                    if (leftCollider != null) leftCollider.enabled = !isOpen;
                    if (rightCollider != null) rightCollider.enabled = !isOpen;

                    // Phát âm thanh
                    if (doorAudio != null && doorSound != null)
                    {
                        doorAudio.PlayOneShot(doorSound);
                    }
                }
            }
            else
            {
                if (interactPrompt != null)
                    interactPrompt.gameObject.SetActive(false);
            }
        }
    }
}

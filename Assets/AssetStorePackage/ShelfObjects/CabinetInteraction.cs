using System.Collections;
using UnityEngine;

public class CabinetInteraction : MonoBehaviour, IInteractable
{
    public Transform door;
    public float openAngle = 90f;
    public float speed = 3f;

    public AudioClip interactionSFX;
    private AudioSource audioSource;

    private bool isOpen = false;
    private bool isMoving = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = door.localRotation;
        openRot = Quaternion.Euler(door.localEulerAngles + new Vector3(0, openAngle, 0));

        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (!isMoving)
        {
            isOpen = !isOpen;
            StopAllCoroutines();
            StartCoroutine(RotateDoor());

            if (audioSource != null)
            {
                audioSource.PlayOneShot(interactionSFX);
            }
        }
    }

        IEnumerator RotateDoor()
    {
        isMoving = true;
        Quaternion target = isOpen ? openRot : closedRot;

        while (Quaternion.Angle(door.localRotation, target) > 0.01f)
        {
            door.localRotation = Quaternion.Slerp(door.localRotation, target, Time.deltaTime * speed);
            yield return null;
        }

        door.localRotation = target;
        isMoving = false;
    }
}

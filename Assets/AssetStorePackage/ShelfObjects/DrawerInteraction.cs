using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerInteraction : MonoBehaviour, IInteractable
{
    public Transform drawer;
    public float openDistance = 0.2f;
    public float speed = 3f;

    public AudioClip interactionSFX;   // âm thanh mở
    private AudioSource audioSource;

    private bool isOpen = false;
    private bool isMoving = false;
    private Vector3 closedPos;
    private Vector3 openPos;

    void Start()
    {
        closedPos = drawer.localPosition;
        openPos = closedPos + new Vector3(0, 0, openDistance);

        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()   // gọi khi player nhấn E và chỉ vào
    {
        if (!isMoving)
        {
            isOpen = !isOpen;
            StopAllCoroutines();
            StartCoroutine(MoveDrawer());

            if (audioSource != null)
            {
                audioSource.PlayOneShot(interactionSFX);
            }
        }
    }

    IEnumerator MoveDrawer()
    {
        isMoving = true;
        Vector3 target = isOpen ? openPos : closedPos;

        while (Vector3.Distance(drawer.localPosition, target) > 0.01f)
        {
            drawer.localPosition = Vector3.Lerp(drawer.localPosition, target, Time.deltaTime * speed);
            yield return null;
        }

        drawer.localPosition = target;
        isMoving = false;
    }
}

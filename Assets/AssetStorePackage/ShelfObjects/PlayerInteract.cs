using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 3f;
    public Image interactCircle;
    private IInteractable currentTarget;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                // hiện vòng tròn
                interactCircle.enabled = true;
                currentTarget = interactable;

                // nhấn E để tương tác
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentTarget.Interact();
                }
            }
            else
            {
                HideCircle();
            }
        }
        else
        {
            HideCircle();
        }
    }

    void HideCircle()
    {
        interactCircle.enabled = false;
        currentTarget = null;
    }
}
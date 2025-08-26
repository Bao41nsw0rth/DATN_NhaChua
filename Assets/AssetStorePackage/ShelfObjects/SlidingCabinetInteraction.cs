using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingCabinetInteraction : MonoBehaviour, IInteractable
{
    public Transform drawer;
    public float openDistance = 0.5f; // khoảng cách trượt ngang
    public float speed = 2f;

    private bool isOpen = false;
    private bool isMoving = false;
    private Vector3 closedPos;
    private Vector3 openPos;

    void Start()
    {
        closedPos = drawer.localPosition;
        // 👇 thay đổi trục X thay vì Z
        openPos = closedPos + new Vector3(openDistance, 0, 0);
    }

    public void Interact()
    {
        if (!isMoving)
        {
            isOpen = !isOpen;
            StopAllCoroutines();
            StartCoroutine(MoveDrawer());
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

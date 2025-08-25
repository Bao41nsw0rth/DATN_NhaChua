using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform ObjectGrabPointTransform;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    public void OnGrab(Transform ObjectGrabPointTransform)
    {
        this.ObjectGrabPointTransform = ObjectGrabPointTransform;
        objectRigidbody.useGravity = false;
    }

    public void OnDrop()
    {
        this.ObjectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
    }

    private void FixedUpdate()
    {
        if (ObjectGrabPointTransform == null)
            return;

        float lerpSpeed = 10f;
        Vector3 newPosition = Vector3.Lerp(transform.position, ObjectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
        objectRigidbody.MovePosition(newPosition);
    }
}

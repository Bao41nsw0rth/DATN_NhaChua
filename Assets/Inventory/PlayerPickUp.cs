using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private Transform PlayerCameraTransform;
    [SerializeField] private Transform ObjectGrabPointTransform;
    [SerializeField] private LayerMask layerMask;

    private ObjectGrabbable objectGrabbable;
    //Inventory inventory = new Inventory();


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float maxDistance = 25f;
            if (objectGrabbable == null)
            {
                if (Physics.Raycast(PlayerCameraTransform.position, PlayerCameraTransform.forward, out RaycastHit raycastHit, maxDistance, layerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.OnGrab(ObjectGrabPointTransform);
                    }
                }
            }
            else
            {
                objectGrabbable.OnDrop();
                objectGrabbable = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (objectGrabbable == null)
            {
                Debug.Log("No object to pick up.");
                return;
            }

            Item item = objectGrabbable.GetComponent<ItemComponent>()?.itemData;

            if (item != null)
            {
                InventoryManager.Instance.AddItem(item);
                Destroy(objectGrabbable.gameObject);
                objectGrabbable = null;
            }
        }

    }
}

using UnityEngine;

public class InventoryPreviewer : MonoBehaviour
{
    public Camera previewCamera;
    public Transform previewRoot;
    private GameObject currentItem;

    private Vector3 prevMousePos;

    public void ShowItem(GameObject itemPrefab)
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
        }

        currentItem = Instantiate(itemPrefab, previewRoot);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        if (currentItem != null)
        {
            float rotSpeed = 50f;
            currentItem.transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime, Space.Self);
        }
    }
}

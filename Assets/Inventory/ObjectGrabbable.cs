using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    [SerializeField] private GameObject InventoryMenu;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI ItemDescription;
    [SerializeField] private InventoryPreviewer previewer;
    
    public void OnGrab()
    {
        Item item = gameObject.GetComponent<ItemComponent>()?.itemData;

        if (item == null) return;

        InventoryMenu.SetActive(true);

        ItemName.text = item.name;
        ItemDescription.text = item.itemDescription;
        previewer.ShowItem(item.itemPrefab, item.itemRotation);
        
        gameObject.SetActive(false);
    }

    public void OnDrop()
    {
        gameObject.SetActive(true);
        InventoryMenu.SetActive(false);
    }
}
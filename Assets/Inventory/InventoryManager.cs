using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemData
{
    public string itemID;
}

[System.Serializable]
public class InventoryData
{
    public List<ItemData> items = new List<ItemData>();
}

public class InventoryManager : MonoBehaviour
{
    [Header("UI References")] [SerializeField]
    private GameObject InventoryMenu;

    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI ItemDescription;
    [SerializeField] private InventoryPreviewer previewer;

    [Header("Items")] [SerializeField] private List<Item> allItems; // danh sách toàn bộ Item trong game
    [SerializeField] private List<Item> items; // item player đang có

    private int currentIndex = 0;
    // private string savePath;

    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // savePath = Path.Combine(Application.persistentDataPath, "inventory.json");
            // LoadInventory();
            AddItem(allItems[12]);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleInventory();
        }

        if (InventoryMenu.activeSelf && items.Count > 1)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0f)
                NextItem();
            else if (scroll < 0f)
                PreviousItem();
        }
    }

    public void ToggleInventory()
    {
        InventoryMenu.SetActive(!InventoryMenu.activeSelf);

        if (InventoryMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (items.Count > 0)
            {
                currentIndex = 0;
                ShowCurrentItem();
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void ShowCurrentItem()
    {
        if (items.Count == 0 || currentIndex < 0 || currentIndex >= items.Count)
            return;

        Item currentItem = items[currentIndex];
        ItemName.text = currentItem.itemName;
        ItemDescription.text = currentItem.itemDescription;
        previewer.ShowItem(currentItem.itemPrefab, currentItem.itemRotation);
    }

    private void NextItem()
    {
        if (items.Count == 0) return;
        currentIndex = (currentIndex + 1) % items.Count;
        ShowCurrentItem();
    }

    private void PreviousItem()
    {
        if (items.Count == 0) return;
        currentIndex = (currentIndex - 1 + items.Count) % items.Count;
        ShowCurrentItem();
    }

    public void AddItem(Item item)
    {
        if (item == null || items.Contains(item)) return;

        items.Add(item);
        // SaveInventory();
    }

    // public void SaveInventory()
    // {
    //     InventoryData data = new InventoryData();
    //     foreach (var item in items)
    //     {
    //         data.items.Add(new ItemData { itemID = item.id });
    //     }
    //
    //     string json = JsonUtility.ToJson(data, true);
    //     File.WriteAllText(savePath, json);
    // }

    // public void LoadInventory()
    // {
    //     if (!File.Exists(savePath)) return;
    //
    //     string json = File.ReadAllText(savePath);
    //     InventoryData data = JsonUtility.FromJson<InventoryData>(json);
    //
    //     items.Clear();
    //     foreach (var savedItem in data.items)
    //     {
    //         Item found = allItems.Find(i => i.id == savedItem.itemID);
    //         if (found != null)
    //             items.Add(found);
    //     }
    // }
}
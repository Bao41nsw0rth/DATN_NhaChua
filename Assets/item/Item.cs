using UnityEngine;

public enum ItemType
{
    Generic,       // Chỉ có thông tin
    Consumable,    // Đồ ăn, hồi máu
    KeyItem        // Chìa khóa, trigger event
}

public abstract class Item : ScriptableObject
{
    public string id;
    public string itemName;
    [TextArea] public string itemDescription;
    public GameObject itemPrefab;
    public ItemType itemType;
    public Quaternion itemRotation;
}
using UnityEngine;

public enum ItemType
{
    Other,
    RecoverSanity
}

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public GameObject itemPrefab;
    public ItemType itemType;
    [Range(0, 50)]
    public int Values;
}

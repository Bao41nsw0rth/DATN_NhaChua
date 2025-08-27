using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "ScriptableObject/Consumable Item")]
public class ConsumableItem : Item
{
    [Range(0, 50)]
    public int restoreHealth;
    [Range(0, 50)]
    public int restoreSanity;

    public void Consume()
    {
        //code
    }
}


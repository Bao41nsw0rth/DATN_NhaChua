using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyItem", menuName = "ScriptableObject/Key Item")]
public class KeyItem : Item
{
    public string unlockTargetID; // ví dụ ID của cửa hay object cần mở

    public void UseOn()
    {
        //code mở cửa, mở ....
    }
}


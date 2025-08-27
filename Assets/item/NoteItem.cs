using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoteItem", menuName = "ScriptableObject/Note Item")]
public class NoteItem : Item
{
    [TextArea] public string noteContent;
}
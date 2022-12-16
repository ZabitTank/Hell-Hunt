using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public new int name;
    public GlobalVariable.ItemType type;

    [Header("Image")]
    GameObject prefabs;
}

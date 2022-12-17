using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public new string name;
    public ItemType type;
    public int stackLimit;

    [Header("Image")]
    public GameObject prefabs;
    [TextArea(1,5)]
    public string description;
}


[Serializable]
public enum ItemType
{
    Gun,
    MeleeWeapon,
    Gold,
    HealItem,
    SpellCard,
}
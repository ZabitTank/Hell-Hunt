using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public int id;
    public new string name;
    public ItemType type;
    public int stackLimit;

    [Header("Gameobject")]
    public GameObject prefabs;
    [Header("Gameobject")]
    public Sprite sprite;
    [TextArea(1,5)]
    public string description;

    public String DisplayGeneralInfo()
    {
        return String.Concat(
            "Type: ", type.ToString(),"\n",
            "Name: ", name, "\n"
            ,description
            );
    }

    public virtual string DisplayAttribute()
    {
        return "";
    }

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

[Serializable]
public class ItemRef
{
    public int id;
    public string name;
    public int stackLimit;

    public ItemRef()
    {
        id = -1;
    }
    public ItemRef(Item item)
    {
        id = item.id;
        name = item.name;
        stackLimit = item.stackLimit;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class Stats
{
    // Equipment DynamicCharacterStat
    [SerializeField]
    private Attribute[] attributes;
    public Attribute[] Attributes
    {
        get { return attributes; }
        private set { attributes = value; }
    }
    public Dictionary<EquipmentAttribute, Attribute> GetAttribute = new();

    // Other stat
    public ModifiableInt HP;
    public ModifiableInt MP;

    public Item playerCurrentWeapon;
    public Item playerDefaultWeapon;

    public abstract void AttributeModified(Attribute attribute);
}

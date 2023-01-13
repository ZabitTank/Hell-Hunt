using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
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

    [SerializeField]
    private ModifiableInt[] magazine;
    public ModifiableInt[] Magazine
    {
        get { return magazine; }
        private set { magazine = value; }
    }

    public Dictionary<GunType, ModifiableInt> GetMagazine = new();

    // Other stat
    public ModifiableInt HP;
    public ModifiableInt MP;

    public Item playerCurrentWeapon;
    public Item playerDefaultWeapon;

    public int GetStatValue(EquipmentAttribute type)
    {
        return GetAttribute[type].value.ModifiedValue;
    }
    public void SetupDictionary()
    {
        foreach (var attribute in Attributes)
        {
            var tempValue = attribute.value.BaseValue;
            attribute.SetParent(this);
            attribute.value.BaseValue = tempValue;

            GetAttribute.Add(attribute.type, attribute);
        }

        GetMagazine.Add(GunType.HandGun, magazine[0]);
        GetMagazine.Add(GunType.ShotGun, magazine[1]);
        GetMagazine.Add(GunType.Rifle, magazine[2]);
    }

    public abstract void AttributeModified(Attribute attribute);
}

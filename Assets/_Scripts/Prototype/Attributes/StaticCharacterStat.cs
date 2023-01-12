using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StaticCharacterStat : Stats
{
    [NonSerialized]
    public BaseEnemyAI enemy;

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

    public Item playerDefaultWeapon;


    public void SetParent(BaseEnemyAI _enemy)
    {
        enemy = _enemy;

        foreach (var attribute in attributes)
        {
            var tempValue = attribute.value.BaseValue;
            attribute.SetParent(this);
            attribute.value.BaseValue = tempValue;

            GetAttribute.Add(attribute.type, attribute);
        }

        HP.RegisterBaseModEvent(() =>
        {
            GlobalVariable.Instance.playerReferences.UIHealthBar.SetValue(
                HP.BaseValue / (float)GetAttribute[EquipmentAttribute.MaxHP].value.ModifiedValue);
        });
    }

    public void RegisterHPEvent(ModifiedEvent die)
    {
        HP.RegisterBaseModEvent(die);
    }

    public int GetStatValue(EquipmentAttribute type)
    {
        return GetAttribute[type].value.ModifiedValue;
    }



    public override void AttributeModified(Attribute attribute)
    {

    }
}

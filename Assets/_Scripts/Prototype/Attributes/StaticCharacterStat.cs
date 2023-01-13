using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StaticCharacterStat : Stats
{
    [NonSerialized]
    public BaseEnemyAI enemy;

    public void SetParent(BaseEnemyAI _enemy)
    {
        enemy = _enemy;

        foreach (var attribute in Attributes)
        {
            var tempValue = attribute.value.BaseValue;
            attribute.SetParent(this);
            attribute.value.BaseValue = tempValue;

            GetAttribute.Add(attribute.type, attribute);
        }
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

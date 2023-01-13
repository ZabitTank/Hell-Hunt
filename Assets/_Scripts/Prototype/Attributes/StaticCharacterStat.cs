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
        SetupDictionary();
    }

    public void RegisterHPEvent(ModifiedEvent die)
    {
        HP.RegisterBaseModEvent(die);
    }
    public override void AttributeModified(Attribute attribute)
    {

    }

}

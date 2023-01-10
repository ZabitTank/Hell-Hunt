using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class Attribute
{
    [NonSerialized]
    public CharacterStat parent;
    public EquipmentAttribute type;
    public ModifiableInt value;
    public void SetParent(CharacterStat combineStat)
    {
        parent = combineStat;
        value = new ModifiableInt(AttributeModified);
    }

    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}

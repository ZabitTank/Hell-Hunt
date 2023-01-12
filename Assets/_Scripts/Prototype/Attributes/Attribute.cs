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
    public Stats parent;
    public EquipmentAttribute type;
    public ModifiableInt value;
    public void SetParent(Stats combineStat)
    {
        parent = combineStat;
        value = new ModifiableInt();
    }

    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}

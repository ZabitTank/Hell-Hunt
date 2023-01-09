using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Attribute
{
    [NonSerialized]
    public Player parent;
    public EquipmentAttribute type;
    public ModifiableInt value;

    public void SetParent(Player _character)
    {
        parent = _character;
        value = new ModifiableInt(AttributeModified);
    }

    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}

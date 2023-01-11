using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public abstract class Stats
{
    public abstract void AttributeModified(Attribute attribute);
}

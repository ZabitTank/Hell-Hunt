using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public delegate void ModifiedEvent();

[System.Serializable]
public class ModifiableInt
{
    [SerializeField]
    private int baseValue;
    public int BaseValue
    {
        get { return baseValue; }
        set 
        { 
            baseValue = value;
            UpdateModifiedValue();
        }
    }

    [SerializeField]
    private int modifiedValue;
    public int ModifiedValue
    {
        get { return modifiedValue; }
        private set { modifiedValue = value; }
    }

    public List<IModifier> modifiers;

    public event ModifiedEvent ValueModified;
    public ModifiableInt(ModifiedEvent method = null)
    {
         modifiers = new List<IModifier>();
         modifiedValue = baseValue;
        if(method != null)
        {
            ValueModified += method;
        }
    }

    public void RegisterModEvent(ModifiedEvent method){
        ValueModified += method;
    }

    public void UnregisterModEvent(ModifiedEvent method) {
        ValueModified -= method;
    }

    public void UpdateModifiedValue()
    {
        var valueToAdd = 0;
        foreach(var modifier in modifiers)
        {
            modifier.AddValue(ref valueToAdd);
        }
        ModifiedValue = baseValue + valueToAdd;

        if (ValueModified != null)
            ValueModified.Invoke();
    }

    public void AddModifier(IModifier modifier)
    {
        modifiers.Add(modifier);
        UpdateModifiedValue();
    }

    public void RemoveModifier(IModifier modifier)
    {
        modifiers.Remove(modifier);
        UpdateModifiedValue();
    }
}
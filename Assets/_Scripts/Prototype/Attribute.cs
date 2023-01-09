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

[Serializable]
public class CharacterStat
{
    [NonSerialized]
    public Player character;
    
    // Equipment Stat
    [SerializeField]
    private Attribute[] attributes;
    public Attribute[] Attributes
    {
        get { return attributes; }
        private set { attributes = value; }
    }
    public Dictionary<EquipmentAttribute, Attribute> GetAttribute =new();

    // Other stat
    public ModifiableInt HP;
    public ModifiableInt MP;

    public Attribute GetStat(EquipmentAttribute type)
    {
        foreach (var attribute in attributes)
        {
            if (attribute.type == type)
                return attribute;
        }
        return null;
    }

    public void UpdateUI()
    {

        var attributeTextUi = "";
        foreach (var attribute in attributes)
        {
            attributeTextUi += string.Concat(Enum.GetName(typeof(EquipmentAttribute), attribute.type),": ",attribute.value.ModifiedValue,'\n');
        }
        GlobalVariable.Instance.playerReferences.StatUI.text = attributeTextUi;
        //GlobalVariable.Instance.playerReferences.StatUI.text = "";
    }

    public void SetParent(Player _character)
    {
        character = _character;

        foreach (var attribute in attributes)
        {
            var tempValue = attribute.value.BaseValue;
            attribute.SetParent(this);
            attribute.value.BaseValue = tempValue;
            GetAttribute.Add(attribute.type, attribute);
        }

        foreach (var slot in character.Equipment.GetSlots)
        {
            slot.onBeforeUpdate += OnBeforeSlotUpdate;
            slot.onAfterUpdate += OnAfterSlotUpdate;
        }

        HP.RegisterBaseModEvent(() =>
        {
            GlobalVariable.Instance.playerReferences.UIHealthBar.SetValue(
                HP.BaseValue / (float)GetAttribute[EquipmentAttribute.MaxHP].value.ModifiedValue);
        });

        GetAttribute[EquipmentAttribute.MaxHP].value.RegisterModEvent(() =>
        {
            GlobalVariable.Instance.playerReferences.UIHealthBar.SetValue(
    HP.BaseValue / (float)GetAttribute[EquipmentAttribute.MaxHP].value.ModifiedValue);
        });
    }
    public void AttributeModified(Attribute attribute)
    {
       
    }
    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        var item = _slot.Item;

        if (item == null)
            return;

        if (item.type == ItemType.MeleeWeapon || item.type == ItemType.Gun)
        {

        }
        else if (item.type == ItemType.Armor || item.type == ItemType.Headgear)
        {
            var equipment = (Equipment)item;
            foreach (var buff in equipment.buffs)
            {
                foreach (var characterAttribute in attributes)
                {
                    if (buff.type == characterAttribute.type)
                    {
                        characterAttribute.value.RemoveModifier(buff);
                    }
                }
            }
        }
        else if (item.type == ItemType.SpellCard)
        {
        }
        UpdateUI();
    }
    public void OnAfterSlotUpdate(InventorySlot _slot)
    {

        var item = _slot.Item;

        if (item == null)
            return;

        if (item.type == ItemType.MeleeWeapon || item.type == ItemType.Gun)
        {

        }
        else if (item.type == ItemType.Armor || item.type == ItemType.Headgear)
        {
            var equipment = (Equipment)item;
            foreach (var buff in equipment.buffs)
            {
                foreach (var characterAttribute in attributes)
                {
                    if (buff.type == characterAttribute.type)
                    {
                        characterAttribute.value.AddModifier(buff);
                    }
                }
            }

        }
        else if (item.type == ItemType.SpellCard)
        {

        }
        UpdateUI();
    }
}

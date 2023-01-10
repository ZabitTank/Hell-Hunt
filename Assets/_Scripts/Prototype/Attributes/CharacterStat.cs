using System;
using System.Collections.Generic;
using UnityEngine;


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
    public Dictionary<EquipmentAttribute, Attribute> GetAttribute = new();

    // Other stat
    public ModifiableInt HP;
    public ModifiableInt MP;

    public Item playerCurrentWeapon;
    public Item playerDefaultWeapon;


    public void SetParent(Player _character)
    {
        character = _character;

        foreach (var attribute in attributes)
        {
            var tempValue = attribute.value.BaseValue;
            attribute.SetParent(this);
            attribute.value.BaseValue = tempValue;
            GetAttribute.Add(attribute.type, attribute);
            attribute.value.RegisterModEvent(UpdateUI);
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
    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        var item = _slot.Item;

        if (item == null)
            return;

        if (item.type == ItemType.Armor || item.type == ItemType.Headgear)
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
    }
    public void OnAfterSlotUpdate(InventorySlot _slot)
    {

        var item = _slot.Item;

        if (item == null)
        {
            if(_slot.AllowedItems[0] == ItemType.Gun || _slot.AllowedItems[0] ==  ItemType.MeleeWeapon)
            {
                playerCurrentWeapon = playerDefaultWeapon;
            }
            return;
        }

        if (item.type == ItemType.MeleeWeapon || item.type == ItemType.Gun)
        {
            playerCurrentWeapon = item;
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
    }

    public int GetStatValue(EquipmentAttribute type)
    {
        return GetAttribute[type].value.ModifiedValue;
    }

    public void UpdateUI()
    {

        var attributeTextUi = "";
        foreach (var attribute in attributes)
        {
            attributeTextUi += string.Concat(Enum.GetName(typeof(EquipmentAttribute), attribute.type), ": ", attribute.value.ModifiedValue, '\n');
        }
        GlobalVariable.Instance.playerReferences.StatUI.text = attributeTextUi;
        //GlobalVariable.Instance.playerReferences.StatUI.text = "";
    }

    public void AttributeModified(Attribute attribute)
    {

    }
}

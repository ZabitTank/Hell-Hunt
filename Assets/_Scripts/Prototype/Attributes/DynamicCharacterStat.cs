using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class DynamicCharacterStat : Stats
{
    [NonSerialized]
    public Player character;

    public void SetParent(Player _character)
    {
        character = _character;

        //TODO: refactor this patch
        if (character.Equipment.GetSlots[2].itemRef.id < 0)
        {
            playerCurrentWeapon = playerDefaultWeapon;
        } else
        {
            playerCurrentWeapon = character.Equipment.GetSlots[2].Item;
        }

        foreach (var attribute in Attributes)
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
                foreach (var characterAttribute in Attributes)
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

        //TODO: refactor this patch
        if (item == null)
        {
            if(_slot.AllowedItems[0] == ItemType.Gun || _slot.AllowedItems[0] ==  ItemType.MeleeWeapon)
            {
                playerCurrentWeapon = playerDefaultWeapon;
                UpdateWeaponUI(item);
                character.playerWeapon.ChangeWeapon(playerCurrentWeapon);
            }
            return;
        }

        //TODO: refactor this patch
        if (item.type == ItemType.MeleeWeapon || item.type == ItemType.Gun)
        {
            playerCurrentWeapon = item;
            UpdateWeaponUI(item);
            character.playerWeapon.ChangeWeapon(playerCurrentWeapon);
        }
        else if (item.type == ItemType.Armor || item.type == ItemType.Headgear)
        {
            var equipment = (Equipment)item;
            foreach (var buff in equipment.buffs)
            {
                foreach (var characterAttribute in Attributes)
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
        GlobalAudio.Instance.PlayeEquipSound();
    }

    public int GetStatValue(EquipmentAttribute type)
    {
        return GetAttribute[type].value.ModifiedValue;
    }

    public void UpdateUI()
    {

        var attributeTextUi = "";
        foreach (var attribute in Attributes)
        {
            attributeTextUi += string.Concat(Enum.GetName(typeof(EquipmentAttribute), attribute.type), ": ", attribute.value.ModifiedValue, '\n');
        }
        GlobalVariable.Instance.playerReferences.StatUI.text = attributeTextUi;
        //GlobalVariable.Instance.playerReferences.StatUI.text = "";
    }

    // cheating
    public void UpdateWeaponUI(Item item)
    {
        if (!item)
        {
            GlobalVariable.Instance.playerReferences.MeleeStatUI.text = "";
            GlobalVariable.Instance.playerReferences.RangedStatUI.text = "";
        }
         else if (item.type == ItemType.MeleeWeapon)
        {
            var castItem = (MeleeWeaponData)item;
            GlobalVariable.Instance.playerReferences.MeleeStatUI.text = castItem.DisplayAttribute();
            GlobalVariable.Instance.playerReferences.RangedStatUI.text = "";
        } else if(item.type == ItemType.Gun)
        {
            var castItem = (GunData)item;
            GlobalVariable.Instance.playerReferences.MeleeStatUI.text = castItem.meleeAttribute.DisplayAttribute();
            GlobalVariable.Instance.playerReferences.RangedStatUI.text = castItem.gunAttribute.DisplayAttribute();
        }

    }

    public override void AttributeModified(Attribute attribute)
    {

    }
}

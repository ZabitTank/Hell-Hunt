using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaticInventoryUI : InventoryUI
{
    public GameObject[] slots;

    public override void CreateDisplay()
    {
        itemsDisplay = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            this.SetItemSlotEvent(slots[i]);
            itemsDisplay.Add(slots[i], inventory.GetSlots[i]);
            inventory.GetSlots[i].slotUI = slots[i];
            inventory.GetSlots[i].onAfterUpdate += OnSlotUpdate;
        }
    }

    public override void OnSlotUpdate(InventorySlot slot)
    {
        var img = slot.slotUI.GetComponentsInChildren<Image>()[1];
        if (slot.itemRef.id >= 0)
        {
            img.sprite = slot.Item.GetSprite();
            img.color = new(img.color.r, img.color.g, img.color.b, 1f);
        }
        else
        {
            img.sprite = null;
            img.color = new(img.color.r, img.color.g, img.color.b, 0);
        }
    }

    public override void UpdateInventorySlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplay)
        {
            var img = slot.Key.GetComponentsInChildren<Image>()[1];
            if (slot.Value.itemRef.id >= 0)
            {
                img.sprite = slot.Value.Item.GetSprite();
                img.color = new(img.color.r, img.color.g, img.color.b,1f);
            }
            else
            {
                img.sprite = null;
                img.color = new(img.color.r, img.color.g, img.color.b, 0);
            }
        }
    }
}

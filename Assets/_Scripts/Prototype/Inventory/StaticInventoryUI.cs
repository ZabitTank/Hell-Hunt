using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaticInventoryUI : InventoryUI
{
    public Image headgearImage;
    public Image armorImage;
    public Image spellCardImage;
    public Image weaponImage;

    public TextMeshProUGUI playerStat;
    public TextMeshProUGUI meleeStat;
    public TextMeshProUGUI gunStat;

    public GameObject[] slots;

    private void Update()
    {
        UpdateInventorySlots();
    }

    public override void CreateDisplay()
    {
        itemsDisplay = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            this.SetItemSlotEvent(slots[i]);
            itemsDisplay.Add(slots[i], inventory.container.items[i]);
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

    public void updateUI(ItemType type, Item item)
    {
        Sprite sprite = item.prefabs.GetComponent<SpriteRenderer>().sprite;
        if (type == ItemType.MeleeWeapon || type == ItemType.Gun)
        {
            weaponImage.sprite = sprite;
        } else if (type == ItemType.SpellCard)
        {
            spellCardImage.sprite = sprite;
        } else if(type == ItemType.Headgear)
        {
            headgearImage.sprite = sprite;
        } else if (type == ItemType.Armor)
        {
            armorImage.sprite = sprite;
        }
    }
}

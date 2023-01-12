using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DynamicInventoryUI : InventoryUI
{
    // slot prfabs
    public GameObject slotPrefabs;

    //Item Slot UI location setting
    public float HORIZONTAL_SPACE_BETWEEN_ITEM;
    public float VERTICAL_SPACE_BETWEEN_ITEM;
    public int NUMBER_ITEMS_IN_ROW;
    public Vector3 START_POSITION;

    public override void CreateDisplay()
    {
        itemsDisplay = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            GameObject itemSlotUI = Instantiate(slotPrefabs, Vector3.zero, Quaternion.identity, transform);
            itemSlotUI.GetComponent<RectTransform>().localPosition = GetPosition(i);

            itemsDisplay.Add(itemSlotUI, inventory.GetSlots[i]);
            inventory.GetSlots[i].slotUI = itemSlotUI;
            inventory.GetSlots[i].onAfterUpdate += OnSlotUpdate;

            SetItemSlotEvent(itemSlotUI);
        }
    }

        public override void UpdateInventorySlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplay)
        {
            if (slot.Value.itemRef.id >= 0)
            {
                var item = slot.Value.Item;
                slot.Key.GetComponentsInChildren<Image>()[1].sprite = item.GetSprite();
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount.ToString();
            }
            else
            {
                slot.Key.GetComponentsInChildren<Image>()[1].sprite = null;
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public override void OnSlotUpdate(InventorySlot slot)
    {
        if (slot.itemRef.id < 0)
        {
            slot.slotUI.GetComponentsInChildren<Image>()[1].sprite = null;
            slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        else
        {
            var item = slot.Item;
            slot.slotUI.GetComponentsInChildren<Image>()[1].sprite = item.GetSprite();
            slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString();
        }
    }

    public Vector3 GetPosition(int index)
    {
        return START_POSITION + new Vector3(HORIZONTAL_SPACE_BETWEEN_ITEM * (index % NUMBER_ITEMS_IN_ROW), -VERTICAL_SPACE_BETWEEN_ITEM * (index / NUMBER_ITEMS_IN_ROW), 0f);
    }
}
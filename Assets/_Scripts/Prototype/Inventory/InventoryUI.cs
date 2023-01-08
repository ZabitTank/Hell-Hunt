using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class InventoryUI : MonoBehaviour
{
    // References inventory
    public Inventory inventory;

    protected Dictionary<GameObject, InventorySlot> itemsDisplay = new();
    protected MouseItem mouseItem;

    public ItemInfoUI infoUI;

    private bool isActive = false;
    public void SwapActiveUnActive()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }
    void Start()
    {
        mouseItem = GlobalVariable.mouseItem;
        foreach(var slot in inventory.container)
        {
            slot.parent = this;
        }
        CreateDisplay();
    }

    public abstract void UpdateInventorySlots();
    public abstract void CreateDisplay();
    protected void SetItemSlotEvent(GameObject itemSlotUI)
    {
        AddEvent(itemSlotUI, EventTriggerType.PointerClick, delegate { OnPointClick(itemSlotUI); });
        AddEvent(itemSlotUI, EventTriggerType.PointerEnter, delegate { OnPointEnter(itemSlotUI); });
        AddEvent(itemSlotUI, EventTriggerType.PointerExit, delegate { OnPointExit(itemSlotUI); });
        AddEvent(itemSlotUI, EventTriggerType.BeginDrag, delegate { OnBeginDrag(itemSlotUI); });
        AddEvent(itemSlotUI, EventTriggerType.Drag, delegate { OnDrag(itemSlotUI); });
        AddEvent(itemSlotUI, EventTriggerType.EndDrag, delegate { OnEndDrag(itemSlotUI); });
    }
    private void AddEvent(GameObject gameObject, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
    private void OnPointClick(GameObject slotUI)
    {
        inventory.currentSelectSlot = itemsDisplay[slotUI];
        if (inventory.currentSelectSlot.id == -1) return;
        Item item = inventory.database.getItem[inventory.currentSelectSlot.id];
        if (item.type == ItemType.Gun || item.type == ItemType.MeleeWeapon)
        {
            inventory.currentSeletedWeapon = inventory.currentSelectSlot;
        }
        DisplaySelectItem(item);
    }
    private void OnPointEnter(GameObject itemSlotUI)
    {
        mouseItem.hoverObj = itemSlotUI;
        if (itemsDisplay.ContainsKey(itemSlotUI))
        {
            GlobalVariable.mouseItem.hoverItem = itemsDisplay[itemSlotUI];
        }
    }

    private void OnPointExit(GameObject itemSlotUI)
    {
        mouseItem.hoverItem = null;
        mouseItem.hoverObj = null; 
    }

    private void OnBeginDrag(GameObject itemSlotUI)
    {
        var mouseObject = new GameObject();
        var reactTranform = mouseObject.AddComponent<RectTransform>();
        reactTranform.sizeDelta = new Vector2(100, 100);
        mouseObject.transform.SetParent(transform.parent);

        if (itemsDisplay[itemSlotUI].id >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.getItem[itemsDisplay[itemSlotUI].id].GetSprite();
            img.raycastTarget = false;
        }
        mouseItem.obj = mouseObject;
        mouseItem.item = itemsDisplay[itemSlotUI];
    }

    private void OnDrag(GameObject itemSlotUI)
    {
       if(mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    private void OnEndDrag(GameObject itemSlotUI)
    {
        if (mouseItem.hoverObj)
        {
            inventory.MoveItem(itemsDisplay[itemSlotUI], mouseItem.hoverItem.parent.itemsDisplay[mouseItem.hoverObj]);
            if (mouseItem.hoverItem.parent is StaticInventoryUI)
                mouseItem.hoverItem.parent.UpdateInventorySlots();
            else if (this is StaticInventoryUI)
                this.UpdateInventorySlots();
        } else
        {
            //inventory.RemoveItem(itemsDisplay[itemSlotUI].itemRef);
        }
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }
    public void DisplaySelectItem(Item item)
    {
        infoUI.DisplayItemInfo(item);
    }
}
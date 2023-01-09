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

    public Dictionary<GameObject, InventorySlot> itemsDisplay = new();

    public ItemInfoUI infoUI;

    private bool isActive = false;
    public void SwapActiveUnActive()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }
    void Start()
    {
        foreach (var slot in inventory.container)
        {
            slot.parent = this;
        }
        CreateDisplay();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterUI(); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitUI(); });

    }

    private void OnExitUI()
    {
        MouseData.UI = null;
    }

    private void OnEnterUI()
    {
        MouseData.UI = this;
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
        if (inventory.currentSelectSlot.itemRef.id == -1) return;
        Item item = inventory.currentSelectSlot.Item;
        if (item.type == ItemType.Gun || item.type == ItemType.MeleeWeapon)
        {
            inventory.currentSeletedWeapon = inventory.currentSelectSlot;
        }
        DisplaySelectItem(item);
    }
    private void OnPointEnter(GameObject itemSlotUI)
    {
        if (itemsDisplay.ContainsKey(itemSlotUI))
        {
            MouseData.slotHover = itemSlotUI;
        }
    }

    private void OnPointExit(GameObject itemSlotUI)
    {
        MouseData.slotHover = null; 
    }

    private void OnBeginDrag(GameObject itemSlotUI)
    {
        var mouseObject = new GameObject();
        var reactTranform = mouseObject.AddComponent<RectTransform>();
        reactTranform.sizeDelta = new Vector2(100, 100);
        mouseObject.transform.SetParent(transform.parent);

        if (itemsDisplay[itemSlotUI].itemRef.id >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = itemsDisplay[itemSlotUI].Item.GetSprite();
            img.raycastTarget = false;
        }
        MouseData.slotBeingDrag = mouseObject;
    }

    private void OnDrag(GameObject itemSlotUI)
    {
       if(MouseData.slotBeingDrag)
        {
            MouseData.slotBeingDrag.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    private void OnEndDrag(GameObject itemSlotUI)
    {
        // prevent drag empty slot
        if (itemsDisplay[itemSlotUI].itemRef.id == -1) return;
        
        if(!MouseData.UI)
        {
            inventory.RemoveItem(itemsDisplay[itemSlotUI].itemRef);
        }
        if(MouseData.slotHover)
        {
            var desSlot = MouseData.UI.itemsDisplay[MouseData.slotHover];
            var srcSlot = itemsDisplay[itemSlotUI];

            if (desSlot.AllowedPlaceInSlot(srcSlot.Item)
    && (desSlot.itemRef.id == -1 || srcSlot.AllowedPlaceInSlot(desSlot.Item)))
            {
                inventory.SwapItem(srcSlot, desSlot);
                // Static UI dont use Update()
                if (desSlot.parent is StaticInventoryUI)
                    desSlot.parent.UpdateInventorySlots();
                else if (this is StaticInventoryUI)
                    this.UpdateInventorySlots();
            }
        }
        Destroy(MouseData.slotBeingDrag);
    }
    public void DisplaySelectItem(Item item)
    {
        infoUI.DisplayItemInfo(item);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // References inventory
    public Inventory inventory;

    // UI
    Dictionary<GameObject, InventorySlot> itemsDisplay = new();
    public GameObject slotPrefabs;

    // Highlight Info
    public Image selectItemImage;
    public TextMeshProUGUI selectItemAttributeText;
    public TextMeshProUGUI selectItemGeneralInfo;

    public EquipmentUI equipmentUI;

    //Item Slot UI location setting
    public float HORIZONTAL_SPACE_BETWEEN_ITEM;
    public float VERTICAL_SPACE_BETWEEN_ITEM;
    public int NUMBER_ITEMS_IN_ROW;
    public Vector3 START_POSITION;

    // Mouse
    public MouseItem mouseItem;

    private bool isActive = false;
    public void SwapActiveUnActive()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }
    private void Awake()
    {
        mouseItem = new();
    }
    void Start()
    {
        CreateDisplay();
    }

    void Update()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplay)
        {
            if (slot.Value.id >= 0)
            {
                slot.Key.GetComponentsInChildren<Image>()[1].sprite = inventory.database.getItem[slot.Value.id].GetSprite();
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount.ToString();

            }
            else
            {
                slot.Key.GetComponentsInChildren<Image>()[1].sprite = null;
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "0";
            }
        }
    }


    private void CreateDisplay()
    {
        itemsDisplay = new Dictionary<GameObject, InventorySlot>();
        for(int i = 0; i < inventory.container.Length; i++)
        {
            GameObject itemSlotUI = Instantiate(slotPrefabs, Vector3.zero, Quaternion.identity, transform);
            itemSlotUI.GetComponent<RectTransform>().localPosition = GetPosition(i);

            itemsDisplay.Add(itemSlotUI, inventory.container[i]);

            SetItemSlotEvent(itemSlotUI);
        }
        // set equiment slot event
    }
    private void SetItemSlotEvent(GameObject itemSlotUI)
    {
        AddEvent(itemSlotUI, EventTriggerType.PointerClick, delegate { OnPointClick(itemSlotUI); });
        AddEvent(itemSlotUI, EventTriggerType.PointerEnter, delegate { OnPointEnter(itemSlotUI); });
        AddEvent(itemSlotUI, EventTriggerType.PointerExit, delegate { OnPointExit(itemSlotUI); });
        AddEvent(itemSlotUI, EventTriggerType.BeginDrag, delegate { OnBeginDrag(itemSlotUI); });
        AddEvent(itemSlotUI, EventTriggerType.Drag, delegate { OnDrag(itemSlotUI); });
        AddEvent(itemSlotUI, EventTriggerType.EndDrag, delegate { OnEndDrag(itemSlotUI); });
    }

    //private GameObject AddSlot(int index, InventorySlot item)
    //{
    //    var itemSlotUI = Instantiate(slotPrefabs, Vector3.zero, Quaternion.identity, transform);

    //    itemSlotUI.GetComponent<RectTransform>().localPosition = GetPosition(i);
    //    itemSlotUI.GetComponentInChildren<TextMeshProUGUI>().text = item.amount.ToString();
    //    itemSlotUI.GetComponentsInChildren<Image>()[1].sprite = inventory.database.getItem[item.id].prefabs.GetComponent<SpriteRenderer>().sprite;
    //    AddEvent(itemSlotUI, EventTriggerType.PointerClick, delegate { OnPointClick(item); });


    //    itemsDisplay.Add(item, itemSlotUI);

    //    return itemSlotUI;
    //}

    private void AddEvent(GameObject gameObject, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void SelectItem(Item item)
    {
        selectItemImage.sprite = item.GetSprite();
        selectItemGeneralInfo.text = item.DisplayGeneralInfo();
        selectItemAttributeText.text = item.DisplayAttribute();
    }

    public void OnPointClick(GameObject slotUI)
    {
        inventory.currentSelectSlot = itemsDisplay[slotUI];
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
            mouseItem.hoverItem = itemsDisplay[itemSlotUI];
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
        reactTranform.sizeDelta = new Vector2(50, 50);
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
            inventory.MoveItem(itemsDisplay[itemSlotUI], mouseItem.hoverItem);
        } else
        {
            inventory.RemoveItem(itemsDisplay[itemSlotUI].itemRef);
        }
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }
    private void DisplaySelectItem(Item item)
    {
        selectItemImage.sprite = item.GetSprite();
        selectItemAttributeText.text = item.DisplayAttribute();
        selectItemGeneralInfo.text = item.DisplayGeneralInfo();
    }

    public Vector3 GetPosition(int index)
    {
        return START_POSITION + new Vector3(HORIZONTAL_SPACE_BETWEEN_ITEM * (index % NUMBER_ITEMS_IN_ROW), -VERTICAL_SPACE_BETWEEN_ITEM * (index / NUMBER_ITEMS_IN_ROW), 0f);
    }

}


public class MouseItem
{
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}
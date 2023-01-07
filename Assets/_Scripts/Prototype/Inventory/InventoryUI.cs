using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject slotPrefabs;
    
    public float HORIZONTAL_SPACE_BETWEEN_ITEM;
    public float VERTICAL_SPACE_BETWEEN_ITEM;
    public int NUMBER_ITEMS_IN_ROW;
    public Vector3 START_POSITION;

    Dictionary<GameObject, InventorySlot> itemsDisplay = new() ;

    public Image selectItemImage;
    public TextMeshProUGUI selectItemAttributeText;
    public TextMeshProUGUI selectItemGeneralInfo;

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
                slot.Key.GetComponentsInChildren<Image>()[1].sprite = inventory.database.getItem[slot.Value.id].prefabs.GetComponent<SpriteRenderer>().sprite;
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

            AddEvent(itemSlotUI, EventTriggerType.PointerClick, delegate { OnPointClick(itemSlotUI); });

        }
    }

    //private GameObject AddSlot(int index, InventorySlot itemSlot)
    //{
    //    var itemSlotUI = Instantiate(slotPrefabs, Vector3.zero, Quaternion.identity, transform);

    //    itemSlotUI.GetComponent<RectTransform>().localPosition = GetPosition(i);
    //    itemSlotUI.GetComponentInChildren<TextMeshProUGUI>().text = itemSlot.amount.ToString();
    //    itemSlotUI.GetComponentsInChildren<Image>()[1].sprite = inventory.database.getItem[itemSlot.id].prefabs.GetComponent<SpriteRenderer>().sprite;
    //    AddEvent(itemSlotUI, EventTriggerType.PointerClick, delegate { OnPointClick(itemSlot); });


    //    itemsDisplay.Add(itemSlot, itemSlotUI);

    //    return itemSlotUI;
    //}

    public Vector3 GetPosition(int index)
    {
        return START_POSITION + new Vector3(HORIZONTAL_SPACE_BETWEEN_ITEM * (index % NUMBER_ITEMS_IN_ROW), -VERTICAL_SPACE_BETWEEN_ITEM * (index / NUMBER_ITEMS_IN_ROW), 0f);
    }

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
        selectItemImage.sprite = item.prefabs.GetComponent<SpriteRenderer>().sprite;
        selectItemGeneralInfo.text = item.DisplayGeneralInfo();
        selectItemAttributeText.text = item.DisplayAttribute();
    }

    public void OnPointClick(GameObject slotUI)
    {
        inventory.currentSelectSlot = itemsDisplay[slotUI];
        Item item = inventory.database.getItem[inventory.currentSelectSlot.id];
        if(item.type == ItemType.Gun || item.type == ItemType.MeleeWeapon)
        {
            inventory.currentSeletedWeapon = inventory.currentSelectSlot;
        }
        selectItemImage.sprite = item.prefabs.GetComponent<SpriteRenderer>().sprite;
        selectItemAttributeText.text = item.DisplayAttribute();
        selectItemGeneralInfo.text = item.DisplayGeneralInfo();
    }

}

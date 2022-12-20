using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject slotPrefabs;
    
    public float HORIZONTAL_SPACE_BETWEEN_ITEM;
    public float VERTICAL_SPACE_BETWEEN_ITEM;
    public int NUMBER_ITEMS_IN_ROW;

    public Vector3 START_POSITION;

    Dictionary<InventorySlot, GameObject> itemsDisplay;

    private void Awake()
    {
        itemsDisplay = new Dictionary<InventorySlot, GameObject>();
    }
    void Start()
    {
        CreateDisplay();
    }

    void Update()
    {
        UpdateDisplay();
    }


    private void CreateDisplay()
    {
        for (int i = 0; i < inventory.inventory.Count; i++)
        {
            InventorySlot slot = inventory.inventory[i];
            AddIteminInventoryUI(i, slot);

        }
    }
    private void UpdateDisplay()
    {
        for (int i = 0; i < inventory.inventory.Count; i++)
        {
            InventorySlot slot = inventory.inventory[i];
            if (itemsDisplay.ContainsKey(slot))
            {
                itemsDisplay[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString();
            }
            else
            {
                AddIteminInventoryUI(i, slot);
            }
        }
    }
    private void AddIteminInventoryUI(int index, InventorySlot slot)
    {
        var itemSlot = Instantiate(slotPrefabs, Vector3.zero, Quaternion.identity, transform);

        itemSlot.GetComponent<RectTransform>().localPosition = GetPosition(index);
        itemSlot.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString();
        itemSlot.GetComponentsInChildren<Image>()[1].sprite = slot.item.prefabs.GetComponent<SpriteRenderer>().sprite;

        itemsDisplay.Add(slot, itemSlot);
    }
    public Vector3 GetPosition(int index)
    {
        return START_POSITION + new Vector3(HORIZONTAL_SPACE_BETWEEN_ITEM * (index % NUMBER_ITEMS_IN_ROW), -VERTICAL_SPACE_BETWEEN_ITEM * (index / NUMBER_ITEMS_IN_ROW), 0f);
    }

}

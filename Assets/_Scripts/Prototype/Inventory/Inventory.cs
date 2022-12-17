using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Items/Inventory")]
public class Inventory : ScriptableObject
{
    public List<InventorySlot> inventory;

    Inventory()
    {
        inventory = new List<InventorySlot>();
    }

    public void AddItem(Item item, int amount)
    {
        foreach(InventorySlot slot in inventory)
        {
            if(slot.item == item)
            {
                slot.AddAmount(amount);
                return;
            }
        }
        inventory.Add(new(item, amount));
    }

}

[Serializable]
public class InventorySlot
{
    public Item item;
    public int amount;
    public InventorySlot(Item item, int amount)
    {
        this.item = item;
        this.amount = (item.stackLimit > amount) ? amount : item.stackLimit;
    }
    public void AddAmount(int amount)
    {
        int totalItem = this.amount + amount;
        this.amount = (totalItem > item.stackLimit) ? item.stackLimit : totalItem;
    }
}
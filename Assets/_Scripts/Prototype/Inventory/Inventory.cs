using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Items/Inventory")]
public class Inventory : ScriptableObject
{
    public string savePath;
    public ItemDatabase database;
    public InventoryObject container;

    public InventorySlot currentSelectSlot;
    public InventorySlot currentSeletedWeapon;

    private void Awake()
    {
        currentSeletedWeapon = null;
        currentSelectSlot = null;
    }
    public void AddItem(ItemRef itemRef, int amount)
    {
        for(int i = 0; i < container.items.Length; i++)
        {
            InventorySlot itemSlot = container.items[i];
            if (itemSlot.itemRef.id == itemRef.id)
            {
                amount = itemSlot.AddAmount(amount);
                if (amount <= 0) return;
            }
        }
        AddIntoEmptySlot(itemRef, amount);
    }

    public InventorySlot AddIntoEmptySlot(ItemRef itemRef, int amount)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].itemRef.id == -1)
            {
                container.items[i].UpdateSlot(itemRef, amount);
                return container.items[i];
            }
        }
        // full
        return null;
    }

    public void SwapItem(InventorySlot slot1, InventorySlot slot2)
    {
        if (!slot1.AllowedPlaceInSlot(slot2.Item) || !slot2.AllowedPlaceInSlot(slot1.Item))
        {
            return;
        }
        InventorySlot temp = new(slot2.itemRef, slot2.amount);
        slot2.UpdateSlot(slot1.itemRef, slot1.amount);
        slot1.UpdateSlot(temp.itemRef, temp.amount);
    }

    public void RemoveItem(ItemRef item)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].itemRef == item)
            {
                container.items[i].UpdateSlot(new(), 0);
            }
        }
    }


    [ContextMenu("Save")]
    public void Save()
    {
        string saveData = JsonUtility.ToJson(container, true);
        BinaryFormatter bf = new();
        FileStream file = File.Create(String.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();

        //IFormatter formatter = new BinaryFormatter();
        //Stream stream = new FileStream(String.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        //formatter.Serialize(stream, container);
        //stream.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(String.Concat(Application.persistentDataPath, savePath))){
            BinaryFormatter bf = new();
            FileStream file = File.Open(String.Concat(Application.persistentDataPath, savePath),FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this.container);
            file.Close();

            //IFormatter formatter = new BinaryFormatter();
            //Stream stream = new FileStream(String.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            //container = (List<InventorySlot>)formatter.Deserialize(stream);
        }
    }

    [ContextMenu("Clear")]
    public void clear()
    {
        container.Clear();
    }
    //public void OnAfterDeserialize()
    //{
    //    for(int i = 0; i < container.Length; i++)
    //    {
    //        container[i].itemRef = new ItemRef(database.getItem[container[i].id]);
    //    }
    //}

    //public void OnBeforeSerialize()
    //{
    //    return;
    //}
}

[Serializable]
public class InventoryObject
{
    public InventorySlot[] items;
    public void Clear()
    {
        foreach (var itemSlot in items)
        {
            itemSlot.UpdateSlot(new(), 0);
        }
    }
}

[Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    public ItemRef itemRef;
    public int amount;

    [NonSerialized]
    public InventoryUI parent;

    public InventorySlot()
    {
        itemRef = new();
        amount = 0;
    }
    public InventorySlot(ItemRef itemRef, int amount)
    {
        this.itemRef = itemRef;
        this.amount = (itemRef.stackLimit > amount) ? amount : itemRef.stackLimit;
    }

    public Item Item
    {
        get
        {
            if (itemRef.id < 0)
                return null;
            return parent.inventory.database.getItem[itemRef.id];
        }

    }

    public int AddAmount(int amount)
    {
        int totalItem = this.amount + amount;
        this.amount = (totalItem > itemRef.stackLimit) ? itemRef.stackLimit : totalItem;
        return totalItem - this.amount;
    }

    public void UpdateSlot(ItemRef itemRef,int amount)
    {
        this.itemRef = itemRef;
        this.amount = amount;
    }

    public bool AllowedPlaceInSlot(Item item)
    {
        if (AllowedItems.Length == 0 || item == null) return true;
        foreach(var type in AllowedItems)
        {
            if (item.type == type) return true;
        }
        return false;
    }
}
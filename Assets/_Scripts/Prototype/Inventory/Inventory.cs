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
    public InventoryType type;
    [SerializeField]
    private InventoryObject container;

    public InventorySlot[] GetSlots
    {
        get
        {
            return container.slots;
        }
    }

    public InventorySlot currentSelectSlot;
    public InventorySlot currentSeletedWeapon;

    private void Awake()
    {
        currentSeletedWeapon = null;
        currentSelectSlot = null;
    }
    public void AddItem(ItemRef itemRef, int amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            InventorySlot itemSlot = container.slots[i];
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
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].itemRef.id == -1)
            {
                GetSlots[i].UpdateSlot(itemRef, amount);
                return GetSlots[i];
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
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].itemRef == item)
            {
                GetSlots[i].UpdateSlot(new(), 0);
            }
        }
    }


    [ContextMenu("Save")]
    public void Save()
    {
        // json
        string saveData = JsonUtility.ToJson(container, true);
        BinaryFormatter bf = new();
        FileStream file = File.Create(String.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();

        //// binary
        //IFormatter formatter = new BinaryFormatter();
        //Stream stream = new FileStream(String.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        //formatter.Serialize(stream, container);
        //stream.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        // json
        if (File.Exists(String.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new();
            FileStream file = File.Open(String.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this.container);
            file.Close();

            //// binary
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
}

[Serializable]
public class InventoryObject
{
    public InventorySlot[] slots;
    public void Clear()
    {
        foreach (var itemSlot in slots)
        {
            itemSlot.UpdateSlot(new(), 0);
        }
    }
}

public enum InventoryType
{
    Inventory,
    Equipment,
    Chest,
}

[Serializable]
public class InventorySlot
{
    [NonSerialized]
    public InventoryUI parent;
    [NonSerialized]
    public GameObject slotUI;
    [NonSerialized]
    public SlotUpdated onAfterUpdate;
    [NonSerialized]
    public SlotUpdated onBeforeUpdate;

    public ItemType[] AllowedItems = new ItemType[0];
    public ItemRef itemRef;
    public int amount;

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

    public int AddAmount(int _amount)
    {
        int totalItem = this.amount + _amount;

        int amount = (totalItem > itemRef.stackLimit) ? itemRef.stackLimit : totalItem;

        UpdateSlot(itemRef, amount);

        return totalItem - amount;
    }

    public void UpdateSlot(ItemRef itemRef, int amount)
    {
        if (onBeforeUpdate != null)
            onBeforeUpdate.Invoke(this);

        this.itemRef = itemRef;
        this.amount = amount;

        if(onAfterUpdate != null)
            onAfterUpdate.Invoke(this);

    }

    public bool AllowedPlaceInSlot(Item item)
    {
        if (AllowedItems.Length == 0 || item == null) return true;
        foreach (var type in AllowedItems)
        {
            if (item.type == type) return true;
        }
        return false;
    }
}

public delegate void SlotUpdated(InventorySlot _slot);
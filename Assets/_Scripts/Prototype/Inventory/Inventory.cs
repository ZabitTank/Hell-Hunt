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
    public InventorySlot[] container;

    public InventorySlot currentSelectSlot;
    public InventorySlot currentSeletedWeapon;

    private void Awake()
    {
        currentSeletedWeapon = null;
        currentSelectSlot = null;
    }
    public void AddItem(ItemRef itemRef, int amount)
    {
        for(int i = 0; i < container.Length; i++)
        {
            InventorySlot itemSlot = container[i];
            if (itemSlot.id == itemRef.id)
            {
                amount = itemSlot.AddAmount(amount);
                if (amount <= 0) return;
            }
        }
        AddIntoEmptySlot(itemRef, amount);
    }

    public InventorySlot AddIntoEmptySlot(ItemRef itemRef, int amount)
    {
        for (int i = 0; i < container.Length; i++)
        {
            if (container[i].id == -1)
            {
                container[i].UpdateSlot(itemRef.id, itemRef, amount);
                return container[i];
            }
        }
        // full
        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new(item2.id, item2.itemRef, item2.amount);
        item2.UpdateSlot(item1.id, item1.itemRef, item1.amount);
        item1.UpdateSlot(temp.id, temp.itemRef, temp.amount);
    }

    public void RemoveItem(ItemRef item)
    {
        for (int i = 0; i < container.Length; i++)
        {
            if (container[i].itemRef == item)
            {
                container[i].UpdateSlot(-1, null, 0);
            }
        }
    }


    [ContextMenu("Save")]
    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
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
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();

            //IFormatter formatter = new BinaryFormatter();
            //Stream stream = new FileStream(String.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            //container = (List<InventorySlot>)formatter.Deserialize(stream);
        }
    }

    [ContextMenu("Clear")]
    public void clear()
    {
        foreach(var itemSlot in container)
        {
            itemSlot.UpdateSlot(-1, new(), 0);
        }
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
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    public int id;
    public ItemRef itemRef;
    public int amount;
    public InventoryUI parent;

    public InventorySlot()
    {
        id = -1;
        itemRef = null;
        amount = 0;
    }
    public InventorySlot(int id,ItemRef item, int amount)
    {
        this.id = id;
        this.itemRef = item;
        this.amount = (item.stackLimit > amount) ? amount : item.stackLimit;
    }
    public int AddAmount(int amount)
    {
        int totalItem = this.amount + amount;
        this.amount = (totalItem > itemRef.stackLimit) ? itemRef.stackLimit : totalItem;
        return totalItem - this.amount;
    }

    public void UpdateSlot(int id, ItemRef itemRef,int amount)
    {
        this.itemRef = itemRef;
        this.id = id;
        this.amount = amount;
    }

    public bool AllowedPlaceInSlot(Item item)
    {
        if (AllowedItems.Length == 0) return true;
        foreach(var type in AllowedItems)
        {
            if (item.type == type) return true;
        }
        return false;
    }
}
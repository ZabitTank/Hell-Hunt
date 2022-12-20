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
    public List<InventorySlot> container;

    Inventory()
    {
        container = new List<InventorySlot>();
    }

    public void AddItem(ItemRef itemRef, int amount)
    {
        foreach(InventorySlot itemSlot in container)
        {
            if(itemSlot.itemRef.id == itemRef.id)
            {
                itemSlot.AddAmount(amount);
                return;
            }
        }
        container.Add(new(itemRef.id,itemRef, amount));
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

    public void OnAfterDeserialize()
    {
        for(int i = 0; i < container.Count; i++)
        {
            container[i].itemRef = new ItemRef(database.getItem[container[i].id]);
        }
    }

    public void OnBeforeSerialize()
    {
        return;
    }
}

[Serializable]
public class InventorySlot
{
    public int id;
    public ItemRef itemRef;
    public int amount;
    public InventorySlot(int id,ItemRef item, int amount)
    {
        this.id = id;
        this.itemRef = item;
        this.amount = (item.stackLimit > amount) ? amount : item.stackLimit;
    }
    public void AddAmount(int amount)
    {
        int totalItem = this.amount + amount;
        this.amount = (totalItem > itemRef.stackLimit) ? itemRef.stackLimit : totalItem;
    }
}
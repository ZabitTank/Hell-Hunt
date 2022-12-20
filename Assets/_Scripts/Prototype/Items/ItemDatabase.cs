using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Database", menuName = "Items/Database")]
public class ItemDatabase : ScriptableObject,ISerializationCallbackReceiver
{
    public Item[] items;
    public Dictionary<int, Item> getItem;

    public void OnAfterDeserialize()
    {
        getItem = new();
        for (int i = 0; i < items.Length; i++)  
        {
            items[i].id = i;
            getItem.Add(i,items[i]);
        }
    }

    public void OnBeforeSerialize()
    {

    }

}

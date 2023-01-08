using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipments")]
public class Equipment : Item
{
    public ItemBuff[] buffs;
    private void Awake()
    {
        buffs = new ItemBuff[3];
    }
    public override string DisplayAttribute()
    {
        string buffDescription = "";
        foreach(var buff in buffs)
        {
            buffDescription += (String.Concat(buff.DisplayAttribute(), "\n"));
        }
        return buffDescription;
    }

}
[Serializable]
public class ItemBuff
{
    public Attribute attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max)
    {
        max = _max;
        min = _min;
    }
    public void GenerateRandom()
    {
        value = UnityEngine.Random.Range(min, max);
    }

    public String DisplayAttribute()
    {
        return String.Concat(Enum.GetName(typeof(Attribute), attribute),": ", value.ToString());
    }
}

[Serializable]
public enum Attribute
{
    Armor,
    Movement,
    Dexterity
}
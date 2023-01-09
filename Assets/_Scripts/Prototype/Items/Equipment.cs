using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipments")]
public class Equipment : Item
{
    public ItemBuff[] buffs;
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
public class ItemBuff : IModifier
{
    public EquipmentAttribute type;
    public int value;
   
    public String DisplayAttribute()
    {
        return String.Concat(Enum.GetName(typeof(EquipmentAttribute), type),": ", value.ToString());
    }

    public void AddValue(ref int value)
    {
        value += this.value;
    }
}

[Serializable]
public enum EquipmentAttribute
{
    Armor,
    Movement,
    Dexterity,
    MaxHP,
}
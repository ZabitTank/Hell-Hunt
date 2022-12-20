using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="HealItem",menuName = "Items/Heal")]
public class HealItem : Item
{
    public int healAmount;
    private void Awake()
    {
        type = ItemType.HealItem;
        stackLimit = 99;
    }
    public override string DisplayAttribute()
    {
        return string.Concat(
            "Amount:    ", healAmount, "\n"
            );
    }
}

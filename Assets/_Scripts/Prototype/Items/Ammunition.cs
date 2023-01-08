using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammunition", menuName = "Items/Ammunitions")]
public class Ammunition : Item
{
    public GunType gunType;

    private void Awake()
    {
        type = ItemType.Ammunition;
    }

    public override string DisplayAttribute()
    {
        return string.Concat(
            "Type:    ", gunType, "\n",
            "Max Size:", stackLimit);
    }

}
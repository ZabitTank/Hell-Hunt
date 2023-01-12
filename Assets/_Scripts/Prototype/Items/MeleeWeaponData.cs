
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Items/Weapons/Melees")]
public class MeleeWeaponData : Item
{
    public AnimatorOverrideController animatorOverride;

    [Header("Melee EquipmentAttribute")]
    public MeleeWeaponAttribute attribute;

    private void Awake()
    {
        type = ItemType.MeleeWeapon;
        stackLimit = 1;
    }

    public override string DisplayAttribute()
    {
        return attribute.DisplayAttribute();
    }
}

[Serializable]
public struct MeleeWeaponAttribute
{
    public float damage;
    public float range;
    public float attackRate;

    public string DisplayAttribute()
    {
        return string.Concat(
            "Damage:    ", damage, "\n",
            "Speed:     ", attackRate, "\n",
            "Ragned:    ",range);
    }
}
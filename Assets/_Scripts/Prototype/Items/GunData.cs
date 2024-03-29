﻿using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun",menuName ="Items/Weapons/Guns")]
public class GunData : Item
{
    public Vector2 localMuzzlePosition;
    public AnimatorOverrideController muzzlEffectAnimatorOverride;
    public AnimatorOverrideController weaponAnimatorOverride;
    public GameObject bulletPrefab;

    [Header("Melee EquipmentAttribute")]
    public MeleeWeaponAttribute meleeAttribute;

    public GunType gunType;

    [Header("Ranged EquipmentAttribute")]
    public RangedAttribute gunAttribute;

    private void Awake()
    {
        type = ItemType.Gun;
        stackLimit = 1;
    }

    public override string DisplayAttribute()
    {
        return string.Concat(
            "Damage:    ", gunAttribute.bulletDamage, "\n",
            "Reload:    ", gunAttribute.reloadSpeed, "\n",
            "Fire rate: ", gunAttribute.fireRate, "\n",
            "Accurate:  ", gunAttribute.reloadSpeed);
    }

}

[Serializable]
public struct RangedAttribute
{
    public int bulletDamage;
    public float reloadSpeed;
    public float fireRate;
    public float fireForce;
    public int ammoCap;
    public float accurateStat;

    public string DisplayAttribute()
    {
        return string.Concat(
            "Damage:    ", this.bulletDamage, "\n",
            "PerformReload:    ", this.reloadSpeed, "\n",
            "Fire rate: ", this.fireRate, "\n",
            "Accurate:  ", this.reloadSpeed);
    }
}


[Serializable]
public enum GunType
{
    ShotGun,
    Rifle,
    HandGun,
}
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun",menuName ="Items/Weapons/Guns")]
public class GunData : Item
{
    public Vector2 localMuzzlePosition;
    public AnimatorOverrideController muzzlEffectAnimatorOverride;
    public AnimatorOverrideController weaponAnimatorOverride;
    public GameObject bulletPrefab;

    [SerializeField]
    [Header("Melee's Attribute")]
    public MeleeWeaponAttribute meleeAttribute;

    [SerializeField]
    [Header("Gun's Attribute")]
    public GunAttribute gunAttribute;

    private void Awake()
    {
        type = ItemType.Gun;
        stackLimit = 1;
    }

}

[Serializable]
public struct GunAttribute
{
    public int bulletDamage;
    public float reloadSpeed;
    public float fireRate;
    public float fireForce;
    public int ammoCap;
    public float accurateStat;
}


[Serializable]
public enum GunType
{
    ShotGun,
    Rifle,
    HandGun,
}
using UnityEngine;

[CreateAssetMenu(fileName = "Gun",menuName ="Items/Weapons/Guns")]
public class GunData : Item
{
    public Vector2 localMuzzlePosition;
    public AnimatorOverrideController muzzlEffectAnimatorOverride;
    public AnimatorOverrideController weaponAnimatorOverride;
    public GameObject bulletPrefab;

    [Header("Melee's Attribute")]
    public int meleeDamage;
    public float weaponSpeed;
    public float attackRate;

    [Header("Gun's Attribute")]
    public int bulletDamage;
    public float reloadTime;
    public float fireRate;
    public float fireForce;
    public int ammoCap;
    public float accurate;

}
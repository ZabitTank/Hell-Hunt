using UnityEngine;

[CreateAssetMenu(fileName = "Gun",menuName ="Items/Weapons/Guns")]
public class GunData : Item
{
    public Transform muzzlePosition;
    public AnimatorOverrideController muzzlEffectAnimatorOverride;
    public AnimatorOverrideController weaponAnimatorOverride;
    public GameObject bulletPrefab;

    [Header("Melee's Attribute")]
    public float meleeDamage;
    public float weaponSpeed;
    public float attackRate;

    [Header("Gun's Attribute")]
    public float reloadTime;
    public float fireRate;
    public float fireForce;
    public int ammoCap;
    public int currentAmmo;


}
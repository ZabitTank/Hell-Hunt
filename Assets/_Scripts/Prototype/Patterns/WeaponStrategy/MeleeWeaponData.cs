
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Items/Weapons/Melees")]
public class MeleeWeaponData : Item
{
    public AnimatorOverrideController animatorOverride;

    [Header("Melee's Attribute")]
    public float damage;
    public float weaponSpeed;
    public float attackRate;
    
}
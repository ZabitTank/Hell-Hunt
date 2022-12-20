
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Items/Weapons/Melees")]
public class MeleeWeaponData : Item
{
    public AnimatorOverrideController animatorOverride;

    [SerializeField]
    [Header("Melee's Attribute")]
    public MeleeWeaponAttribute attribute;

    private void Awake()
    {
        type = ItemType.MeleeWeapon;
        stackLimit = 1;
    }


}

[Serializable]
public struct MeleeWeaponAttribute
{
    public float damage;
    public float weaponSpeed;
    public float attackRate;
}
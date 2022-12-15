using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun",menuName ="Weapon/Gun")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public string type;

    [Header("Attribute")]
    public float damage;
    public float distance;
    public float fireRate;
    public float reloadTime;

    [Header("Ammo")]
    public int ammoCap;
    public int currentAmmo;
}
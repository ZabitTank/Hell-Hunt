﻿using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseWeapon : MonoBehaviour
{
    private IWeaponAttackBehaviour weaponBehavior;

    [SerializeField] Animator bodyAnimator;
    [SerializeField] Animator muzzleAnimator;

    [SerializeField] GunData defaultWeapon;
    private void Awake()
    {
        GunBehaviour gunBehaviour = gameObject.AddComponent<GunBehaviour>();
        gunBehaviour.InitState(defaultWeapon, bodyAnimator, muzzleAnimator);

        weaponBehavior = gunBehaviour;
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && weaponBehavior.CanDoPrimaryAttack() && !EventSystem.current.IsPointerOverGameObject())
        {
            weaponBehavior.PrimaryAttack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && weaponBehavior.CanDoSecondaryAttack())
        {
            weaponBehavior.SecondaryAttack();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            weaponBehavior.PreparePrimaryAttack();
            return;
        }
    }

}

using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerWeapon : MonoBehaviour
{
    private IWeaponAttackBehaviour weaponBehavior;

    [SerializeField] Animator bodyAnimator;
    [SerializeField] Animator muzzleAnimator;
    [SerializeField] Item defaultWeapon;

    public Inventory inventory;
    private void Awake()
    {
        ChangeWeapon(defaultWeapon);
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.E)) {
            if (inventory.currentSeletedWeapon != null)
            {
                Item weapon = inventory.database.getItem[inventory.currentSeletedWeapon.itemRef.id];
                ChangeWeapon(weapon);
            } else
            {
                ChangeWeapon(defaultWeapon);
            }

        }
        
        if (Input.GetKey(KeyCode.Mouse0) && weaponBehavior.CanDoPrimaryAttack())
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

    private void ChangeWeapon(Item weapon)
    {
        switch(weapon.type)
        {
            case ItemType.Gun:
                GunBehaviour gunBehaviour = gameObject.AddComponent<GunBehaviour>();
                gunBehaviour.InitState((GunData)weapon, bodyAnimator, muzzleAnimator);
                weaponBehavior = gunBehaviour;
                break;
            case ItemType.MeleeWeapon:
                MeleeWeaponBehaviour meleeWeaponBehaviour = gameObject.AddComponent<MeleeWeaponBehaviour>();
                meleeWeaponBehaviour.InitState((MeleeWeaponData)weapon, bodyAnimator);
                weaponBehavior = meleeWeaponBehaviour;
                break;
        }
    }
}

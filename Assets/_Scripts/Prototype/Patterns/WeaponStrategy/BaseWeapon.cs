using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseWeapon : MonoBehaviour
{
    private IWeaponAttackBehaviour weaponBehavior;

    [SerializeField] Animator bodyAnimator;
    [SerializeField] Animator muzzleAnimator;
    [SerializeField] Item currentWeapon;

    public Inventory inventory;
    private void Awake()
    {
        GunBehaviour gunBehaviour = gameObject.AddComponent<GunBehaviour>();
        gunBehaviour.InitState((GunData)currentWeapon, bodyAnimator, muzzleAnimator);

        weaponBehavior = gunBehaviour;
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.E)) {
            if (inventory.currentSeletedWeapon != null)
            {
                Item itemData = inventory.database.getItem[inventory.currentSeletedWeapon.itemRef.id];
                GunBehaviour gunBehaviour = gameObject.AddComponent<GunBehaviour>();
                gunBehaviour.InitState((GunData)itemData, bodyAnimator, muzzleAnimator);

                weaponBehavior = gunBehaviour;
            }

        }

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

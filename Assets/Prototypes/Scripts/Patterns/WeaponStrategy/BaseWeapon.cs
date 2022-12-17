using UnityEditor;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    private IWeaponAttackBehaviour weaponBehavior;

    [SerializeField] Animator bodyAnimator;
    [SerializeField] Animator muzzleAnimator;
    [SerializeField] GunData defaultGun;
    private void Awake()
    {
        GunBehaviour gunBehaviour = gameObject.AddComponent<GunBehaviour>();
        gunBehaviour.InitState(defaultGun, bodyAnimator, muzzleAnimator);
        weaponBehavior = gunBehaviour;
    }

    private void SetWeapon()
    {

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && weaponBehavior.CanDoPrimaryAttack())
        {
            weaponBehavior.PrimaryAttack();
        }

        if(Input.GetKeyDown(KeyCode.Mouse1) && weaponBehavior.CanDoSecondaryAttack())
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

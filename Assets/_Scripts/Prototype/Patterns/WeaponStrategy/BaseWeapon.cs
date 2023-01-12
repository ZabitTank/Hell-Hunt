using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseWeapon : MonoBehaviour
{
    public CharacterController characterController { get; set; }

    private IWeaponAttackBehaviour weaponBehavior;

    [SerializeField] Animator muzzleAnimator;
    [SerializeField] Transform meleePosition;
    public float testAttackRange;
    public LayerMask enemyLayer;

    public void DoPrimaryAttack()
    {
        if(weaponBehavior.CanDoPrimaryAttack())
            weaponBehavior.PrimaryAttack();
    }

    public void DoSeconddaryAttack()
    {
        if (weaponBehavior.CanDoSecondaryAttack())
            weaponBehavior.SecondaryAttack();
    }
    public void PreparePrimaryAttack()
    {
        weaponBehavior.PreparePrimaryAttack();
    }


    public void ChangeWeapon(Item weapon)
    {
        if (weaponBehavior != null)
            Destroy(weaponBehavior.Self());
        switch (weapon.type)
        {
            case ItemType.Gun:
                GunBehaviour gunBehaviour = gameObject.AddComponent<GunBehaviour>();
                gunBehaviour.Initialize((GunData)weapon, characterController, muzzleAnimator, meleePosition, enemyLayer);
                weaponBehavior = gunBehaviour;
                break;
            case ItemType.MeleeWeapon:
                MeleeWeaponBehaviour meleeWeaponBehaviour = gameObject.AddComponent<MeleeWeaponBehaviour>();
                meleeWeaponBehaviour.Initialize((MeleeWeaponData)weapon, characterController, meleePosition, enemyLayer);
                weaponBehavior = meleeWeaponBehaviour;
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!meleePosition) return;
        Gizmos.DrawWireSphere(meleePosition.position, testAttackRange);
    }
}

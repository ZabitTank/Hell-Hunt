using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerWeapon : MonoBehaviour
{
    public Player parent { get; set; }

    private IWeaponAttackBehaviour weaponBehavior;

    [SerializeField] Animator muzzleAnimator;
    [SerializeField] Transform meleePosition;
    public float testAttackRange;
    public LayerMask enemyLayer;

    private void Update()
    {
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

    public void ChangeWeapon(Item weapon)
    {
        if (weaponBehavior != null)
            Destroy(weaponBehavior.Self());
        switch (weapon.type)
        {
            case ItemType.Gun:
                GunBehaviour gunBehaviour = gameObject.AddComponent<GunBehaviour>();
                gunBehaviour.Initialize((GunData)weapon, parent.characterController, muzzleAnimator, meleePosition, enemyLayer);
                weaponBehavior = gunBehaviour;
                break;
            case ItemType.MeleeWeapon:
                MeleeWeaponBehaviour meleeWeaponBehaviour = gameObject.AddComponent<MeleeWeaponBehaviour>();
                meleeWeaponBehaviour.Initialize((MeleeWeaponData)weapon, parent.characterController, meleePosition, enemyLayer);
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

using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseWeapon : MonoBehaviour
{
    public CharacterController characterController { get; set; }
    private IWeaponAttackBehaviour weaponBehavior;

    [HideInInspector]
    public DynamicCharacterStat playerAttribute;
    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public Animator muzzleAnimator;

    public Transform meleePosition;
    public Transform muzzlePosition;

    public float testAttackRange;
    public LayerMask enemyLayer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        muzzleAnimator = GetComponentInChildren<Animator>();
    }
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
                gunBehaviour.Initialize(this,(GunData)weapon);
                weaponBehavior = gunBehaviour;
                break;
            case ItemType.MeleeWeapon:
                MeleeWeaponBehaviour meleeWeaponBehaviour = gameObject.AddComponent<MeleeWeaponBehaviour>();
                meleeWeaponBehaviour.Initialize(this,(MeleeWeaponData)weapon);
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

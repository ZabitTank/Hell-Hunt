using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseWeapon : MonoBehaviour
{
    public CharacterController characterController;

    private GunBehaviour gunBehaviour;
    private MeleeWeaponBehaviour meleeWeaponBehaviour;

    private IWeaponAttackBehaviour currentBehaviour;

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
        gunBehaviour = GetComponent<GunBehaviour>();
        meleeWeaponBehaviour = GetComponent<MeleeWeaponBehaviour>();
    }
    public void DoPrimaryAttack()
    {
        if(currentBehaviour.CanDoPrimaryAttack())
            currentBehaviour.PrimaryAttack();
    }

    public void DoSeconddaryAttack()
    {
        if (currentBehaviour.CanDoSecondaryAttack())
            currentBehaviour.SecondaryAttack();
    }
    public void PreparePrimaryAttack()
    {
        currentBehaviour.PreparePrimaryAttack();
    }


    public void ChangeWeapon(Item weapon)
    {

        switch (weapon.type)
        {
            case ItemType.Gun:
     
                gunBehaviour.Initialize(this,(GunData)weapon);
                currentBehaviour = gunBehaviour;
                break;
            case ItemType.MeleeWeapon:
                meleeWeaponBehaviour.Initialize(this,(MeleeWeaponData)weapon);
                currentBehaviour = meleeWeaponBehaviour;
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!meleePosition) return;
        Gizmos.DrawWireSphere(meleePosition.position, testAttackRange);
    }
}

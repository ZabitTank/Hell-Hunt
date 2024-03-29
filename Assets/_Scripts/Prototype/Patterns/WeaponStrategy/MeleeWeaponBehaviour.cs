using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;

public class MeleeWeaponBehaviour : MonoBehaviour,IWeaponAttackBehaviour
{
    BaseWeapon Base;

    MeleeWeaponData meleeWeaponData;
    CharacterController characterController;
    MeleeWeaponAttribute meleeAttribute;
    float timeToMelee;

    Transform meleePosition;
    LayerMask layerMask;

    AudioClip meleeSound;

    public void Initialize(BaseWeapon _baseWeapon,MeleeWeaponData _meleeWeaponData)
    {
        Base = _baseWeapon;
        layerMask = Base.enemyLayer;
        meleePosition = Base.meleePosition;
        meleeWeaponData = _meleeWeaponData;
        characterController = Base.characterController;
        meleeAttribute = meleeWeaponData.attribute;

        //TODO: ???????
        if (characterController.bodyAnimator == null)
        {
            return;
        }
        characterController.bodyAnimator.runtimeAnimatorController = meleeWeaponData.animatorOverride;

        characterController.bodyAnimator.SetFloat("MeleeSpeed", meleeAttribute.attackRate);

        timeToMelee = Time.time;

        meleeSound = GlobalAudio.Instance.weaponAudioClips.punch;
    }

    public Component Self()
    {
        return this;
    }


    public bool CanDoPrimaryAttack()
    {
        return (Time.time >= timeToMelee && !EventSystem.current.IsPointerOverGameObject());
    }

    public bool CanDoSecondaryAttack()
    {
        return CanDoPrimaryAttack();
    }

    public void PrimaryAttack()
    {
        StartCoroutine(PerformMeleeAttack());
    }

    public void SecondaryAttack()
    {
        PrimaryAttack();
    }

    public void PreparePrimaryAttack()
    {

    }
    IEnumerator PerformMeleeAttack()
    {
        timeToMelee = Time.time + 1 / meleeAttribute.attackRate;
        characterController.PerformMeleeAttackAniamtion();
        yield return new WaitForSeconds(3 / (4 * meleeAttribute.attackRate));
        // detect in object in range
        Base.audioSource.PlayOneShot(meleeSound);
        Collider2D[] hitobject = Physics2D.OverlapCircleAll(meleePosition.position, meleeAttribute.range, layerMask);
        //
        foreach (Collider2D enemy in hitobject)
        {
            enemy.GetComponent<Rigidbody2D>().AddForce(transform.right * 500);

            var baseAI = enemy.GetComponent<BaseEnemyAI>();
            if (baseAI)
                baseAI.TakeDamage(-(int)meleeAttribute.damage);
        }
    }

}

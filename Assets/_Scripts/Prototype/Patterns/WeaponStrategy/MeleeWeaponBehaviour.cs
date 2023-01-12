using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;

public class MeleeWeaponBehaviour : MonoBehaviour,IWeaponAttackBehaviour
{
    MeleeWeaponData meleeWeaponData;

    CharacterController characterController;
    MeleeWeaponAttribute meleeAttribute;
    float timeToMelee;

    Transform meleePosition;

    LayerMask layerMask;

    public void Initialize(MeleeWeaponData _meleeWeaponData, CharacterController _characterController,Transform _meleePosition,LayerMask _layerMask)
    {
        layerMask = _layerMask;
        meleePosition = _meleePosition;
        meleeWeaponData = _meleeWeaponData;
        characterController = _characterController;

        meleeAttribute = meleeWeaponData.attribute;

        characterController.bodyAnimator.runtimeAnimatorController = meleeWeaponData.animatorOverride;

        characterController.bodyAnimator.SetFloat("MeleeSpeed", meleeAttribute.attackRate);

        timeToMelee = Time.time;
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
        Collider2D[] hitobject = Physics2D.OverlapCircleAll(meleePosition.position, meleeAttribute.range, layerMask);
        //
        foreach (Collider2D enemy in hitobject)
        {
            enemy.GetComponent<Rigidbody2D>().AddForce(transform.right * 500);
        }
    }

}

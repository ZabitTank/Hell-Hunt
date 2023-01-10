using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;

public class MeleeWeaponBehaviour : MonoBehaviour,IWeaponAttackBehaviour
{


    MeleeWeaponData meleeWeaponData;
    Animator bodyAnimator;

    MeleeWeaponAttribute meleeWeaponAttribute;
    float timeToMelee;

    Transform meleePosition;
    LayerMask layerMask;
    public void InitState(MeleeWeaponData meleeWeaponData, Animator bodyAnimator,Transform _meleePosition,LayerMask _layerMask)
    {
        layerMask = _layerMask;
        meleePosition = _meleePosition;
        this.meleeWeaponData = meleeWeaponData;
        this.bodyAnimator = bodyAnimator;

        meleeWeaponAttribute = meleeWeaponData.attribute;

        this.bodyAnimator.runtimeAnimatorController = meleeWeaponData.animatorOverride;
        this.bodyAnimator.SetFloat("MeleeSpeed", meleeWeaponAttribute.attackRate);

        timeToMelee = Time.time;

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
        timeToMelee = Time.time + 1/meleeWeaponAttribute.attackRate;
        bodyAnimator.SetTrigger("Melee");
        // detect in object in range
        Collider2D[] hitobject = Physics2D.OverlapCircleAll(meleePosition.position, meleeWeaponAttribute.range, layerMask);
        //
        foreach (Collider2D enemy in hitobject)
        {
            enemy.GetComponent<Rigidbody2D>().AddForce(transform.right * 500);
        }
    }

    public void SecondaryAttack()
    {
        PrimaryAttack();
    }

    public void PreparePrimaryAttack()
    {

    }
    public Component Self()
    {
        return this;
    }
}

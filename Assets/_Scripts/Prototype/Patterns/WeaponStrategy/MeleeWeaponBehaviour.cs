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

    // Player's Stats

    public void InitState(MeleeWeaponData meleeWeaponData, Animator bodyAnimator)
    {
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
    }

    public void SecondaryAttack()
    {
        PrimaryAttack();
    }

    public void PreparePrimaryAttack()
    {

    }
}

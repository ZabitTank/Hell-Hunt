using UnityEngine;

public interface IWeaponAttackBehaviour
{
    void PrimaryAttack();
    void SecondaryAttack();
    bool CanDoPrimaryAttack();
    bool CanDoSecondaryAttack();
    void PreparePrimaryAttack();
    Component Self();
}

public interface IWeaponBehavior
{
    void PrimaryAttack();
    void SecondaryAttack();
    bool CanDoPrimaryAttack();
    bool CanDoSecondaryAttack();
    void PreparePrimaryAttack();
}

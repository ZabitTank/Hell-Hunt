using System.Collections;
using UnityEngine;

public class GunBehaviour : MonoBehaviour,IWeaponBehavior
{
    GunData _gunData;
    Animator _bodyAnimator;
    Animator _muzzleAnimator;
    // State
    Transform muzzlePosition;
    int ammoStack;
    int currentAmmo;
    bool isReloading;
    float timeToFire;
    float timeToMelee;

    public void InitState(GunData gunData, Animator bodyAnimator, Animator muzzleAnimator)
    {
        _gunData = gunData;
        _bodyAnimator = bodyAnimator;
        _muzzleAnimator = muzzleAnimator;

        _bodyAnimator.runtimeAnimatorController = gunData.weaponAnimatorOverride;
        _muzzleAnimator.runtimeAnimatorController = gunData.muzzlEffectAnimatorOverride;
        _bodyAnimator.SetFloat("ReloadSpeed", 1 / _gunData.reloadTime);
        _bodyAnimator.SetFloat("FireRate", 1 / _gunData.fireRate);
        _bodyAnimator.SetFloat("MeleeSpeed", 1 / _gunData.attackRate);
        _muzzleAnimator.SetFloat("EffectSpeed", 1 / _gunData.fireRate);

        ammoStack = 1;
        currentAmmo = 0;
        isReloading = false;
        timeToFire = Time.time;
        timeToMelee = Time.time;

    }

    public bool CanDoPrimaryAttack()
    {
        return (Time.time > timeToFire && !isReloading && currentAmmo > 0);
    }

    public bool CanDoSecondaryAttack()
    {
        return (Time.time > timeToMelee);
    }

    public void PrimaryAttack()
    {
        timeToFire = Time.time + 1 / _gunData.fireRate;
        _bodyAnimator.SetTrigger("Shoot");

        if (_gunData.bulletPrefab == null)
        {
            return;
        }
        // Todo: Apply Object pooler or using raycast
        GameObject shot = Instantiate(_gunData.bulletPrefab, _gunData.muzzlePosition.position, _gunData.muzzlePosition.rotation);
        _muzzleAnimator.SetTrigger("Shoot");
    }

    public void SecondaryAttack()
    {
        timeToMelee = Time.time + 1 / _gunData.attackRate;

    }

    public void PreparePrimaryAttack()
    {
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        isReloading = true;
        _bodyAnimator.Play("Reload");
        yield return new WaitForSeconds(1 / _bodyAnimator.GetFloat("ReloadSpeed"));
        currentAmmo = _gunData.ammoCap;
        isReloading = false;

    }

    public void InitWeaponData(Item item)
    {
        throw new System.NotImplementedException();
    }
}

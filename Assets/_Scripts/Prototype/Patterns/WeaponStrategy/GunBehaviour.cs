using System.Collections;
using UnityEngine;

public class GunBehaviour : MonoBehaviour,IWeaponAttackBehaviour
{
    public float spread;

    GunData _gunData;
    Animator _bodyAnimator;
    Animator _muzzleAnimator;

    // State
    Transform muzzlePosition;
    int currentAmmo;
    bool isReloading;
    float timeToFire;
    float timeToMelee;
    float currentSpread;

    // player's state
    int totalAmmo;
    float playerAcurateState;

    public void InitState(GunData gunData, Animator bodyAnimator, Animator muzzleAnimator)
    {
        _gunData = gunData;
        _bodyAnimator = bodyAnimator;
        _muzzleAnimator = muzzleAnimator;

        muzzlePosition = _muzzleAnimator.transform;
        muzzlePosition.localPosition = _gunData.localMuzzlePosition;

        _bodyAnimator.runtimeAnimatorController = gunData.weaponAnimatorOverride;
        _muzzleAnimator.runtimeAnimatorController = gunData.muzzlEffectAnimatorOverride;
        _bodyAnimator.SetFloat("ReloadSpeed", 1 / _gunData.reloadTime);
        _bodyAnimator.SetFloat("FireRate", _gunData.fireRate);
        _bodyAnimator.SetFloat("MeleeSpeed",  _gunData.attackRate);
        _muzzleAnimator.SetFloat("EffectSpeed", _gunData.fireRate);

        currentAmmo = gunData.ammoCap;
        totalAmmo = currentAmmo * 100;
        isReloading = false;
        timeToFire = Time.time;
        timeToMelee = Time.time;

    }

    public bool CanDoPrimaryAttack()
    {
        return (Time.time >= timeToFire && !isReloading);
    }

    public bool CanDoSecondaryAttack()
    {
        return (Time.time > timeToMelee);
    }

    public void PrimaryAttack()
    {
        if (currentAmmo <= 0)
        {
            PreparePrimaryAttack();
            return;
        }
        timeToFire = Time.time + 1 / _gunData.fireRate;
        _bodyAnimator.SetTrigger("Shoot");

        SpawnBullet();
    }

    private void SpawnBullet()
    {
        if (_gunData.bulletPrefab == null)
        {
            return;
        }

        GameObject bullet = Instantiate(_gunData.bulletPrefab, _muzzleAnimator.transform.position, _muzzleAnimator.transform.rotation);
        bullet.GetComponent<Bullet>().InitState(_gunData.fireForce, _gunData.bulletDamage,caculateSpread());
        _muzzleAnimator.SetTrigger("Shoot");
        currentAmmo--;
    }

    public void SecondaryAttack()
    {
        timeToMelee = Time.time + 1 / _gunData.attackRate;

    }

    public void PreparePrimaryAttack()
    {
        if(totalAmmo > 0)
            StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        isReloading = true;
        _bodyAnimator.Play("Reload");
        yield return new WaitForSeconds(1 / _bodyAnimator.GetFloat("ReloadSpeed"));
        currentAmmo = (totalAmmo < _gunData.ammoCap) ? totalAmmo : _gunData.ammoCap;
        totalAmmo = Mathf.Clamp(totalAmmo - _gunData.ammoCap, 0, int.MaxValue);
        isReloading = false;
    }

    float caculateSpread()
    {
        float s = Random.Range(-spread, spread);

        return s - s/100*playerAcurateState;
    }
}

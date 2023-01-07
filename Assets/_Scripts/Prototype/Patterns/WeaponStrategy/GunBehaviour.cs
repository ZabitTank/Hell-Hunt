using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;

public class GunBehaviour : MonoBehaviour,IWeaponAttackBehaviour
{
    // test
    public float spread;

    GunData gunData;
    Animator bodyAnimator;
    Animator muzzleAnimator;

    GunAttribute gunAttribute;
    MeleeWeaponAttribute meleeAttribute;
    int currentAmmo;
    bool isReloading;
    float timeToFire;
    float timeToMelee;
    float currentSpread;

    // Player's Stats
    int totalAmmo;
    float playerAcurateState;

    public void InitState(GunData gunData, Animator bodyAnimator, Animator muzzleAnimator)
    {
        this.gunData = gunData;
        this.bodyAnimator = bodyAnimator;
        this.muzzleAnimator = muzzleAnimator;

        gunAttribute = gunData.gunAttribute;
        meleeAttribute = gunData.meleeAttribute;

        this.muzzleAnimator.transform.localPosition = new(gunData.localMuzzlePosition.x,gunData.localMuzzlePosition.y,0);
        //this.muzzleAnimator.transform.localPosition = GlobalV ariable.HANDGUN_MUZZLE_POSITION;


        this.bodyAnimator.runtimeAnimatorController = gunData.weaponAnimatorOverride;
        this.muzzleAnimator.runtimeAnimatorController = gunData.muzzlEffectAnimatorOverride;
        this.bodyAnimator.SetFloat("ReloadSpeed", gunAttribute.reloadSpeed);
        this.bodyAnimator.SetFloat("FireRate", gunAttribute.fireRate);
        this.bodyAnimator.SetFloat("MeleeSpeed", meleeAttribute.weaponSpeed);
        this.muzzleAnimator.SetFloat("EffectSpeed", gunAttribute.fireRate);

        currentAmmo = gunAttribute.ammoCap;
        totalAmmo = currentAmmo * 100;
        isReloading = false;
        timeToFire = Time.time;
        timeToMelee = Time.time;

    }

    public bool CanDoPrimaryAttack()
    {
        return (Time.time >= timeToFire && !isReloading && !EventSystem.current.IsPointerOverGameObject());
    }

    public bool CanDoSecondaryAttack()
    {
        return (Time.time > timeToMelee && !EventSystem.current.IsPointerOverGameObject());
    }

    public void PrimaryAttack()
    {
        if (currentAmmo <= 0)
        {
            PreparePrimaryAttack();
            return;
        }
        timeToFire = Time.time + 1 / gunAttribute.fireRate;
        bodyAnimator.SetTrigger("Shoot");

        SpawnBullet();
    }

    private void SpawnBullet()
    {
        if (gunData.bulletPrefab == null)
        {
            return;
        }

        GameObject bullet = Instantiate(gunData.bulletPrefab, muzzleAnimator.transform.position, muzzleAnimator.transform.rotation);
        bullet.GetComponent<Bullet>().InitState(gunAttribute.fireForce, gunAttribute.bulletDamage, caculateSpread());
        muzzleAnimator.SetTrigger("Shoot");
        currentAmmo--;
    }

    public void SecondaryAttack()
    {
        timeToMelee = Time.time + 1 / meleeAttribute.attackRate;
        bodyAnimator.SetTrigger("Melee");
    }

    public void PreparePrimaryAttack()
    {
        if(totalAmmo > 0)
            StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        isReloading = true;
        bodyAnimator.Play("Reload");
        yield return new WaitForSeconds(1 / bodyAnimator.GetFloat("ReloadSpeed"));
        currentAmmo = (totalAmmo < gunAttribute.ammoCap) ? totalAmmo : gunAttribute.ammoCap;
        totalAmmo = Mathf.Clamp(totalAmmo - gunAttribute.ammoCap, 0, int.MaxValue);
        isReloading = false;
    }

    float caculateSpread()
    {
        float s = Random.Range(-spread, spread);

        return s - s/100*playerAcurateState;
    }
}

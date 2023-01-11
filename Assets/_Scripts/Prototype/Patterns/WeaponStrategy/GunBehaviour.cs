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

    RangedAttribute gunAttribute;
    MeleeWeaponAttribute meleeAttribute;
    int currentAmmo;
    bool isReloading;
    float timeToFire;
    float timeToMelee;
    float currentSpread;

    // Other
    LayerMask LayerMask;
    Transform meleePosition;
    int totalAmmo;
    float playerAcurateState;

    public void InitState(GunData gunData, Animator bodyAnimator, Animator muzzleAnimator,Transform _meleePosition,LayerMask _layerMask)
    {
        LayerMask = _layerMask;
        meleePosition = _meleePosition;
        this.gunData = gunData;
        this.bodyAnimator = bodyAnimator;
        this.muzzleAnimator = muzzleAnimator;

        gunAttribute = gunData.gunAttribute;
        meleeAttribute = gunData.meleeAttribute;

        this.muzzleAnimator.transform.localPosition = GlobalVariable.MUZZLE_POSITION[gunData.gunType];

        this.bodyAnimator.runtimeAnimatorController = gunData.weaponAnimatorOverride;
        this.muzzleAnimator.runtimeAnimatorController = gunData.muzzlEffectAnimatorOverride;
        this.bodyAnimator.SetFloat("ReloadSpeed", gunAttribute.reloadSpeed);
        this.bodyAnimator.SetFloat("FireRate", gunAttribute.fireRate);
        this.bodyAnimator.SetFloat("MeleeSpeed", meleeAttribute.range);
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
        StartCoroutine(PerformMeleeAttack());
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

    IEnumerator PerformMeleeAttack()
    {
        timeToMelee = Time.time + 1 / meleeAttribute.attackRate;
        bodyAnimator.SetTrigger("Melee");
        yield return new WaitForSeconds(3 / (4 * meleeAttribute.attackRate));
        // detect in object in range
        Collider2D[] hitobject = Physics2D.OverlapCircleAll(meleePosition.position, meleeAttribute.range, LayerMask);
        //
        foreach (Collider2D enemy in hitobject)
        {
            enemy.GetComponent<Rigidbody2D>().AddForce(transform.right * 500);
        }
    }

    float caculateSpread()
    {
        float s = Random.Range(-spread, spread);

        return s - s/100*playerAcurateState;
    }

    public Component Self()
    {
        return this;
    }
}

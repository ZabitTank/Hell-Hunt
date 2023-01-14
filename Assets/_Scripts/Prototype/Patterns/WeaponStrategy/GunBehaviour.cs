using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;

public class GunBehaviour : MonoBehaviour,IWeaponAttackBehaviour
{
    BaseWeapon Base;

    GunData gunData;
    CharacterController characterController;
    Animator muzzleAnimator;
    Transform muzzleTranform;

    RangedAttribute gunAttribute;
    MeleeWeaponAttribute meleeAttribute;
    ModifiableInt currentAmmo;
    bool isReloading;
    float timeToFire;
    float timeToMelee;
    float timeToPlayShootSound = 1f;
    AudioClip shotAudioClip;
    AudioClip meleeAudioClip;
    // Other
    LayerMask LayerMask;
    Transform meleePosition;

    ModifiableInt totalAmmo;
    ModifiableInt playerAccurateStat;
    public void Initialize(BaseWeapon _parent,GunData _gunData)
    {
        Base = _parent;

        LayerMask = Base.enemyLayer;

        meleePosition = Base.meleePosition;
        gunData = _gunData;
        characterController = Base.characterController;
        muzzleAnimator = Base.muzzleAnimator;
        muzzleTranform = Base.muzzlePosition;

        gunAttribute = gunData.gunAttribute;
        meleeAttribute = gunData.meleeAttribute;

        //TODO: ???????
        if (characterController.bodyAnimator == null)
        {
            return;
        }

        characterController.bodyAnimator.runtimeAnimatorController = gunData.weaponAnimatorOverride;
        muzzleAnimator.runtimeAnimatorController = gunData.muzzlEffectAnimatorOverride;

        characterController.bodyAnimator.SetFloat("ReloadSpeed", gunAttribute.reloadSpeed);
        characterController.bodyAnimator.SetFloat("FireRate", gunAttribute.fireRate);
        characterController.bodyAnimator.SetFloat("MeleeSpeed", meleeAttribute.range);

        muzzleAnimator.SetFloat("EffectSpeed", gunAttribute.fireRate);
        muzzleAnimator.transform.localPosition = GlobalVariable.MUZZLE_FLASH_POSITION[gunData.gunType];
        muzzleTranform.transform.localPosition = GlobalVariable.MUZZLE_POSITION[gunData.gunType];

        shotAudioClip = GlobalAudio.Instance.weaponAudioClips.GetAudioByGunType(gunData.gunType);
        meleeAudioClip = GlobalAudio.Instance.weaponAudioClips.punch;
        isReloading = false;
        timeToFire = Time.time;
        timeToMelee = Time.time;
        playerAccurateStat = Base.playerStats.GetAttribute[EquipmentAttribute.Dexterity].value;

        totalAmmo = Base.playerStats.GetMagazine[gunData.gunType];
        currentAmmo = new(null);
        totalAmmo.UpdateBaseValue(totalAmmo.BaseValue);
        if (Base.gameObject.CompareTag("Player"))
        {
            currentAmmo.RegisterBaseModEvent(() =>
            {
                GlobalVariable.Instance.playerReferences.AmmoUI.text = string.Concat(currentAmmo.BaseValue.ToString(),
                    " / ", totalAmmo.BaseValue.ToString());
            });
        }
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
        if (currentAmmo.BaseValue <= 0)
        {
            PreparePrimaryAttack();
            return;
        }
        timeToFire = Time.time + 1 / gunAttribute.fireRate;

        if(Time.time > timeToPlayShootSound)
        {
            Base.audioSource.PlayOneShot(shotAudioClip);
            timeToPlayShootSound = Time.time + 1f;
        }

        characterController.PerformShootAnimation();
        SpawnBullet();
    }

    private void SpawnBullet()
    {
        if (gunData.bulletPrefab == null)
        {
            return;
        }

        GameObject bullet = Instantiate(gunData.bulletPrefab, muzzleTranform.position, muzzleTranform.rotation);

        bullet.GetComponent<Bullet>().InitState(gunAttribute.fireForce, gunAttribute.bulletDamage, caculateSpread());
        currentAmmo.UpdateBaseValue(-1);
    }

    public void SecondaryAttack()
    {
        StartCoroutine(PerformMeleeAttack());
    }

    public void PreparePrimaryAttack()
    {
        if (totalAmmo.BaseValue > 0)
            StartCoroutine(Reload());
        else
        {
            if (Time.time > timeToPlayShootSound)
            {
                Base.audioSource.PlayOneShot(GlobalAudio.Instance.weaponAudioClips.outAmmo);
                timeToPlayShootSound = Time.time + 1f;
            }
        }

    }

    IEnumerator Reload()
    {
        isReloading = true;
        characterController.PerformReload();
        yield return new WaitForSeconds(1 / gunAttribute.reloadSpeed);
        var amountAmmo = totalAmmo.BaseValue + currentAmmo.BaseValue;
        totalAmmo.SetBaseValue(Mathf.Clamp(amountAmmo - gunAttribute.ammoCap, 0, int.MaxValue));
        currentAmmo.SetBaseValue((amountAmmo < gunAttribute.ammoCap) ? amountAmmo : gunAttribute.ammoCap);
        isReloading = false;
    }

    IEnumerator PerformMeleeAttack()
    {
        timeToMelee = Time.time + 1 / meleeAttribute.attackRate;

        characterController.PerformMeleeAttackAniamtion();

        yield return new WaitForSeconds(3 / (4 * meleeAttribute.attackRate));
        Base.audioSource.PlayOneShot(meleeAudioClip);
        // detect in object in range
        Collider2D[] hitobject = Physics2D.OverlapCircleAll(meleePosition.position, meleeAttribute.range, LayerMask);
        //
        foreach (Collider2D enemy in hitobject)
        {
            enemy.GetComponent<Rigidbody2D>().AddForce(transform.right * 500);

            var baseAI = enemy.GetComponent<BaseEnemyAI>();
            if (baseAI)
                baseAI.TakeDamage(-(int)meleeAttribute.damage);

        }
    }

    float caculateSpread()
    {
        float s = Random.Range(-gunAttribute.accurateStat, gunAttribute.accurateStat);

        return s - (s / 10) * playerAccurateStat.BaseValue;
    }

    public Component Self()
    {
        return this;
    }
}

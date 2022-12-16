using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    IWeaponBehavior weapon;
    [SerializeField]
    private Animator muzzleFlash; // change if weapon type change

    // references
    private Player player;
    void Start()
    {
        weapon = new GunBehaviour();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isReloading);
        if(Input.GetKeyDown(KeyCode.Mouse0) && Time.time > timeToFire && !isReloading)
        {
            timeToFire = Time.time + 1 / fireRate;
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }
    }

    void Fire()
    {
        player.body.SetTrigger("Shoot");
        SpawnBulleet();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        player.body.Play("Reload");
        yield return new WaitForSeconds(1/  player.body.GetFloat("ReloadSpeed"));
        currentAmmo = ammoCap;
        isReloading = false;

    }

    void SpawnBulleet()
    {
        if (bullet == null)
        {
            return;
        }
        // Todo: Apply Object pooler or using raycast
        GameObject shot = Instantiate(bullet, muzzlePosition.position, muzzlePosition.rotation);
        shot.GetComponent<Bullet>().damage = weaponDamage;
        muzzleFlash.SetTrigger("Shoot");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damege;
    public float fireRate;
    public int currentAmmo;
    public int ammoCap;
    public int reloadTime;

    public Transform gun;

    [SerializeField] private GameObject bullet;

    private Player player;
    float timeToFire = 0;
    private bool isReloading; 
    public Animator body;

    [SerializeField]
    private Animator muzzleFlash;
    void Start()
    {
        isReloading = false;
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading) return;
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && Time.time > timeToFire)
        {
            timeToFire = Time.time + 1 / fireRate;
            Fire();
        }
    }

    void Fire()
    {
        player.Shoot();
        SpawnBulleet();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        player.body.Play("Reload");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = ammoCap;

        isReloading = false;


    }


    void SpawnBulleet()
    {
        if (bullet == null)
        {
            return;
        }

        GameObject shot = Instantiate(bullet, gun.position, gun.rotation);
        shot.GetComponent<Bullet>().damage = damege;

        muzzleFlash.SetTrigger("Shoot");
    }
}

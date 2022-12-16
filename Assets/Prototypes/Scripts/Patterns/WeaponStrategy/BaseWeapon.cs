using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public IWeaponBehavior weaponBehavior;
    private void Start()
    {
        weaponBehavior = gameObject.AddComponent<GunBehaviour>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && weaponBehavior.CanDoPrimaryAttack())
        {
            weaponBehavior.PrimaryAttack();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            weaponBehavior.PreparePrimaryAttack();
            return;
        }
    }

}

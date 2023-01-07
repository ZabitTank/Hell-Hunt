using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariable :Singleton<MonoBehaviour>
{
    public static readonly Vector3 HANDGUN_MUZZLE_POSITION = new(1.783f, -0.56f);
    public static readonly Vector3 SHOTGUN_MUZZLE_POSITION = new(1.825f, -0.494f);
    public static readonly Vector3 RIFLE_MUZZLE_POSITION = new(1.387f, -0.546f);

    public static readonly Dictionary<GunType, Vector2> MUZZLE_POSITION = new()
    {
        { GunType.Rifle, RIFLE_MUZZLE_POSITION},
        { GunType.ShotGun, SHOTGUN_MUZZLE_POSITION},
        { GunType.HandGun, HANDGUN_MUZZLE_POSITION},
    };

    public static Dictionary<GunType, Vector2> GUN_ANIMATOROVERIDER;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariable : Singleton<MonoBehaviour>
{
    public static readonly Vector3 HANDGUN_MUZZLE_POSITION = new(1.783f, -0.56f);
    public static readonly Vector3 SHOTGUN_MUZZLE_POSITION = new(1.825f, -0.494f);
    public static readonly Vector3 RIFLE_MUZZLE_POSITION = new(1.387f, -0.546f);

    public static Dictionary<GunType, Vector2> muzzlePosition = new();
    protected override void Awake()
    {
        base.Awake();
        muzzlePosition.Add(GunType.ShotGun, SHOTGUN_MUZZLE_POSITION);
        muzzlePosition.Add(GunType.HandGun, HANDGUN_MUZZLE_POSITION);
        muzzlePosition.Add(GunType.Rifle, RIFLE_MUZZLE_POSITION);
    }
}

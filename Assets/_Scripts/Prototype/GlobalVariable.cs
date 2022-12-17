using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariable : Singleton<MonoBehaviour>
{
    public static readonly Vector2 HANDGUN_MUZZLE_POSITION = new(1.783f, -0.56f);
    public static readonly Vector2 SHOTGUN_MUZZLE_POSITION = new(1.825f, -0.494f);
    public static readonly Vector2 RIFLE_MUZZLE_POSITION = new(1.387f, -0.546f);

    Dictionary<GunType, Vector2> m_Gun_RifflePos = new();
    protected override void Awake()
    {
        base.Awake();
        m_Gun_RifflePos.Add(GunType.ShotGun, SHOTGUN_MUZZLE_POSITION);
        m_Gun_RifflePos.Add(GunType.HandGun, HANDGUN_MUZZLE_POSITION);
        m_Gun_RifflePos.Add(GunType.Rifle, RIFLE_MUZZLE_POSITION);
    }
}

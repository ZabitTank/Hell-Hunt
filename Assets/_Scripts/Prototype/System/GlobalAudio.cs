using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : Singleton<GlobalAudio>
{
    AudioSource audioSource;
    // Start is called before the first frame update
    public WeaponAudioClip audioClips;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}

[Serializable]
public struct WeaponAudioClip
{
    public AudioClip handgun;
    public AudioClip rifle;
    public AudioClip shotgun;
    public AudioClip outAmmo;
    public AudioClip knife;
    public AudioClip punch;

    public AudioClip GetAudioByGunType(GunType type)
    {
        switch (type)
        {
            case GunType.HandGun:
                return handgun;
            case GunType.ShotGun:
                return shotgun;
            case GunType.Rifle:
                return rifle;
        }
        return null;
    }
}

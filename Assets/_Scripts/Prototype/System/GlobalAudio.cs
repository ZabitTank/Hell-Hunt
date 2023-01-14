using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : Singleton<GlobalAudio>
{
    AudioSource audioSource;
    public AudioSource background;
    // Start is called before the first frame update
    public WeaponAudioClip weaponAudioClips;
    public IventorySoundEffect iventorySoundEffect;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SettingUpdate();
    }

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlaySwapSlot()
    {
        audioSource.PlayOneShot(iventorySoundEffect.swapItem);
    }

    public void PlayeEquipSound()
    {
        audioSource.PlayOneShot(iventorySoundEffect.EquipItem);
    }

    public void PlayeDropItemSound()
    {
        audioSource.PlayOneShot(iventorySoundEffect.dropItem);
    }

    public void SettingUpdate()
    {
        if(background != null)
            background.volume = GameManager.Instance.backgroundVolume;
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
[Serializable]
public struct IventorySoundEffect
{
    public AudioClip swapItem;
    public AudioClip EquipItem;
    public AudioClip dropItem;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : PersistentSingleton<OptionMenu>
{

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void ChangeBackgroundTheme(float volume)
    {
        GameManager.Instance.backgroundVolume = volume;
    }

    public void ChangeCharacterAudioSource(float volume)
    {
        GameManager.Instance.characterSound = volume;
    }
}

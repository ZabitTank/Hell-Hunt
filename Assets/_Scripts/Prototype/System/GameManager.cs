using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    public bool isPause = false;
    public float backgroundVolume = 0.3f;
    public float characterSound = 0.3f;

    public void SaveGame()
    {
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level-1");
    }

    public void LoadGame()
    {
        //player.Inventory.Load();
        //player.Equipment.Load();
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  
    }

    public void SwapPauseResumeGame()
    {
        if (OptionMenu.Instance)
        {
            OptionMenu.Instance.gameObject.SetActive(isPause);
            isPause = !isPause;
        }

    }

    public void SetThemeVolume(float volume)
    {
        backgroundVolume = volume;
        if (GlobalAudio.Instance)
            GlobalAudio.Instance.background.volume = volume;
    }

    public void SetCharacterSound(float volume)
    {
        characterSound = volume;
        var weapons = FindObjectsOfType<BaseWeapon>();
        if (weapons.Length > 0)
        {
            foreach(var weapon in weapons)
            {
                weapon.audioSource.volume = volume;
            }
        }
    }

}
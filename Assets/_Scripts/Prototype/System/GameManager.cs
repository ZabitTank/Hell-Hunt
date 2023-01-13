using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{

    private void Start()
    {

    }

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

}
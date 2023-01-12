using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // References
    private Player player;

    // Main Menu
    public void PlayGame()
    {
        SceneManager.LoadScene("Level-1");
    }

    public void LoadGame()
    {
        player.Inventory.Load();
        player.Equipment.Load();
        player.LoadGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

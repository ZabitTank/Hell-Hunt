using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSetting : Singleton<SceneSetting>
{
    public ModifiableInt enemyCount;
    public bool isPlayerDead;

    public GameObject UIWin;
    public GameObject UILose;

    private void Start()
    {
        var enemies = FindObjectsOfType<BaseEnemyAI>();

        Debug.Log(enemies.Length);
        enemyCount.UpdateBaseValue(enemies.Length);
        isPlayerDead = false;

        enemyCount.RegisterBaseModEvent(() =>
        {
            if (enemyCount.BaseValue <= 0)
            {
                UIWin.SetActive(true);
            }
        });
    }
}
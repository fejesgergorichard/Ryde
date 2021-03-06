using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    void Start()
    {
        InitializeAppSettings();
        Resources.Load("");
    }

    private static void InitializeAppSettings()
    {
        QualitySettings.vSyncCount = 0;
        QualitySettings.antiAliasing = 0;

        Application.targetFrameRate = 60;
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        string targetScene = "Abstract1";
        SceneLoader.LoadMap(targetScene);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitializeAppSettings();

        string targetScene = "Abstract1";
        SceneManager.LoadScene(targetScene);
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        GameManager.ActiveMap = targetScene;
        GameManager.Instance?.Restart();
    }

    private static void InitializeAppSettings()
    {
        QualitySettings.vSyncCount = 0;
        QualitySettings.antiAliasing = 0;

        Application.targetFrameRate = 60;
    }
}

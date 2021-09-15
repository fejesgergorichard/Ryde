using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool _pausedFromUI = false;
    private GameObject PauseMenuUI;
    public GameObject SceneSelectorUI;

    public static bool GameIsPaused = false;

    private void Start()
    {
        // The UI must be the 0th child
        PauseMenuUI = transform.GetChild(0).gameObject;
        PauseMenuUI.SetActive(false);
        SceneSelectorUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused) Resume();
            else Pause();
        }
    }
    private void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // we can set this to create slowmo effects
        GameIsPaused = false;
    }
    private void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // we can set this to create slowmo effects
        GameIsPaused = true;
    }

    public void MenuButtonAction()
    {
        SceneManager.UnloadSceneAsync(GameManager.ActiveMap);
        SceneSelectorUI.SetActive(true);
        Resume();
    }

    public void HomeButtonAction()
    {
        //SceneManager.GetAllScenes().Select(s => SceneManager.UnloadScene(s));
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseFromUI()
    {
        _pausedFromUI = !_pausedFromUI;
        
        if (_pausedFromUI)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool PausedFromUI = false;
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

    public void SceneSelectorButtonAction()
    {
        SceneSelectorUI.SetActive(true);
        Resume();
    }

    public void HomeButtonAction()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadMainMenu();
    }

    public void PauseFromUI()
    {
        PausedFromUI = !PausedFromUI;
        
        if (PausedFromUI)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool _pausedFromUI = false;

    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;

    private void Start()
    {
        PauseMenuUI.SetActive(false);
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

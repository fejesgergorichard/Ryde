using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject Overlay;
    public GameObject SceneSelectorUI;
    public CanvasGroup canvasGroup; 
    public static SceneLoader Instance { get; private set; }

    public void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one GameManager in the scene!");
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadMainMenu();
    }

    public static void LoadMap(string sceneToLoad)
    {
        Instance?.LoadGameScene(sceneToLoad);
    }
    public static void LoadMain()
    {
        Instance?.LoadMainMenu();
    }
    public static void ReloadMap()
    {
        //Instance?.LoadGameScene(GameManager.ActiveMap);
        Instance?.ReloadMapInstance();
    }

    public void LoadGameScene(string sceneToLoad)
    {
        StartCoroutine(StartLoad(sceneToLoad));
    }

    public void LoadMainMenu()
    {
        StartCoroutine(StartLoadMainMenu());
    }

    private IEnumerator StartLoad(string sceneToLoad)
    {
        LoadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 0.1f));

        AsyncOperation loadMap = SceneManager.LoadSceneAsync(sceneToLoad);
        AsyncOperation loadCommon = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);

        while (!loadMap.isDone)
        {
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 0.1f));

        LoadingScreen.SetActive(false);
        Overlay.SetActive(true);
        PauseMenu.GameIsPaused = false;
        PauseMenu.PausedFromUI = false;
        SceneSelectorUI.SetActive(false);

        GameManager.ActiveMap = sceneToLoad;
    }

    private void ReloadMapInstance()
    {
        SceneManager.LoadScene(GameManager.ActiveMap);
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }

    private IEnumerator StartLoadMainMenu()
    {
        Overlay.SetActive(false);
        LoadingScreen.SetActive(true);

        yield return StartCoroutine(FadeLoadingScreen(1, 0.5f));

        AsyncOperation operation = SceneManager.LoadSceneAsync("MainMenu");
        while (!operation.isDone)
        {
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 0.2f));

        LoadingScreen.SetActive(false);
        Overlay.SetActive(false);
        PauseMenu.GameIsPaused = false;
        PauseMenu.PausedFromUI = false;
        SceneSelectorUI.SetActive(false);
    }

    private IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
    }
}
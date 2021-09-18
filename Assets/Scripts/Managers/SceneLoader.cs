using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject Overlay;
    public GameObject SceneSelectorUI;
    public GameObject PauseUI;
    public GameObject TrackCompleteUI;
    public int MinimumLoadTimeInMs = 500;
    private DateTime loadStartTime;

    public Image Image;

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
        loadStartTime = DateTime.Now;
        Image.color = new Color(1, 1, 1, 0f);
        LoadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 0.1f));

        Overlay.SetActive(true);
        PauseMenu.GameIsPaused = false;
        PauseMenu.PausedFromUI = false;
        SceneSelectorUI.SetActive(false);
        TrackCompleteUI.SetActive(false);
        GameManager.ActiveMap = sceneToLoad;
        Time.timeScale = 1f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        AsyncOperation operation2 = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        operation.allowSceneActivation = false;
        operation2.allowSceneActivation = false;

        while (!operation.isDone)
        {
            FillMask(operation.progress);

            if (DateTime.Compare(loadStartTime.AddMilliseconds(MinimumLoadTimeInMs), DateTime.Now) < 0)
            {
                operation.allowSceneActivation = true;
                operation2.allowSceneActivation = true;
            }

            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 0.1f));

        LoadingScreen.SetActive(false);
    }

    private void FillMask(float progress)
    {
        Image.color = new Color(1, 1, 1, progress + 0.1f);
    }

    private void ReloadMapInstance()
    {
        SceneManager.LoadScene(GameManager.ActiveMap);
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        TrackCompleteUI.SetActive(false);
    }

    private IEnumerator StartLoadMainMenu()
    {
        loadStartTime = DateTime.Now;
        Time.timeScale = 1f;
        Image.color = new Color(1, 1, 1, 0f);
        LoadingScreen.SetActive(true);

        yield return StartCoroutine(FadeLoadingScreen(1, 0.5f));
        
        Overlay.SetActive(false);
        PauseMenu.GameIsPaused = false;
        PauseMenu.PausedFromUI = false;
        SceneSelectorUI.SetActive(false);
        TrackCompleteUI.SetActive(false);
        PauseUI.SetActive(false);

        AsyncOperation operation = SceneManager.LoadSceneAsync("MainMenu");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            FillMask(operation.progress); 

            if (DateTime.Compare(loadStartTime.AddMilliseconds(MinimumLoadTimeInMs), DateTime.Now) < 0)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 0.2f));

        LoadingScreen.SetActive(false);
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
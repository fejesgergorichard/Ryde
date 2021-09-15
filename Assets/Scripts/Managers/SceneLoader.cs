using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject LoadingScreen;
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
        yield return StartCoroutine(FadeLoadingScreen(1, 0.5f));

        SceneManager.LoadScene(sceneToLoad);
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);

        yield return StartCoroutine(FadeLoadingScreen(0, 0.5f));
        LoadingScreen.SetActive(false);

        GameManager.ActiveMap = sceneToLoad;
    }

    private IEnumerator StartLoadMainMenu()
    {
        LoadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 0.5f));

        AsyncOperation operation = SceneManager.LoadSceneAsync("MainMenu");
        while (!operation.isDone)
        {
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 0.5f));
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
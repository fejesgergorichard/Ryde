using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    private GameObject _sceneSelectorUi;

    private void Start()
    {
        _sceneSelectorUi = transform.parent.gameObject;
    }

    public void LoadScene(string target)
    {
        SceneManager.GetAllScenes().Select(s => SceneManager.UnloadScene(s));

        SceneManager.LoadScene(target);
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        GameManager.ActiveMap = target;
        _sceneSelectorUi.SetActive(false);
    }
}

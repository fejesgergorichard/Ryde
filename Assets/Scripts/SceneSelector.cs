using System.Collections;
using System.Collections.Generic;
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
        SceneManager.LoadScene(target);
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        GameManager.ActiveMap = target;
        GameManager.Instance?.Restart();
        _sceneSelectorUi.SetActive(false);
    }
}

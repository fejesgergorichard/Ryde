using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string target)
    {
        SceneManager.LoadScene(target);
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        GameManager.ActiveMap = target;
    }
}

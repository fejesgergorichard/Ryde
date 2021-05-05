using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody _playerRigidBody;
    private GameObject _spawnObject;
    private bool _mapLoaded = true;

    public static string ActiveMap;


    void Start()
    {
        LoadPlayerInitInfo();
        ResetPlayer();
    }


    void Update()
    {
        
    }

    public void Restart()
    {
        ResetPlayer();

        LoadMap(ActiveMap);
        LoadMap(ActiveMap);
    }

    private void LoadMap(string mapName)
    {
        if (!_mapLoaded)
            SceneManager.LoadSceneAsync(mapName, LoadSceneMode.Additive).completed += SceneLoadAsyncCompleted;
        else
            SceneManager.UnloadSceneAsync(mapName);

        _mapLoaded = !_mapLoaded;
    }

    private void SceneLoadAsyncCompleted(AsyncOperation obj)
    {
        Debug.Log("SCENE LOADED BOI");
    }

    private void ResetPlayer()
    {
        _player.transform.parent = null;
        SceneManager.MoveGameObjectToScene(_player, gameObject.scene);
        _spawnObject = GameObject.Find("SpawnObject");

        _player.transform.position = _spawnObject.transform.position;
        _player.transform.rotation = _spawnObject.transform.rotation;
        _playerRigidBody.velocity = Vector3.zero;
        _playerRigidBody.angularVelocity = Vector3.zero;
    }

    private void LoadPlayerInitInfo()
    {
        _player = GameObject.Find("Player");
        _playerRigidBody = _player.GetComponent<Rigidbody>();
    }
}

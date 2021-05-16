using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody _playerRigidBody;
    private GameObject _spawnObject;
    private bool _mapLoaded = true;

    public static GameManager Instance;
    public GameObject TrackCompleteUI;
    public static string ActiveMap;

    [Header("Coin fly settings")]
    public Canvas Canvas;
    public RectTransform CoinFlyTarget;
    public TMP_Text CoinCounterText;
    public GameObject CoinSprite;

    private int _score;
    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            CoinCounterText.text = _score.ToString();
        }
    }


    private void Awake()
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
#if DEBUG
        if (ActiveMap == null || ActiveMap == "")
            ActiveMap = "Abstract1";
#endif

        TrackCompleteUI.SetActive(false);

        GameEvents.Instance.onCrystalTriggerEnter += CrystalCollected;

        LoadPlayerInitInfo();
        ResetPlayer();
    }

    public void Restart()
    {
        Score = 0;

        ResetPlayer();

        ReloadMap(ActiveMap);
        TrackCompleteUI.SetActive(false);
        Time.timeScale = 1f;
    }

    // TEST
    public async void AddCoin(Vector3 coinWorldPos, int num)
    {
        Score += num;

        Vector2 coinScreenPos = Camera.main.WorldToScreenPoint(coinWorldPos);

        for (int i = 0; i < num; i++)
        {
            var coin = Instantiate(CoinSprite, coinScreenPos, Quaternion.identity) as GameObject;
            coin.transform.SetParent(CoinFlyTarget.parent);
            LeanTween.moveLocal(coin, CoinFlyTarget.localPosition, 0.4f).setEaseInQuad().setIgnoreTimeScale(true);
            Destroy(coin, 0.4f);
            if (num > 1)
                await Task.Run(() => Thread.Sleep(30));
        }
    }


    private void LoadMap(string mapName)
    {
        if (!_mapLoaded)
        {
            SceneManager.LoadScene(mapName, LoadSceneMode.Additive);
            var mainCamera = GameObject.Find("Main Camera");
            var cameraPlaceholder = GameObject.Find("CameraPlaceholder");

            mainCamera.transform.position = cameraPlaceholder.transform.position;
            mainCamera.transform.rotation = cameraPlaceholder.transform.rotation;
        }
        else
            SceneManager.UnloadSceneAsync(mapName);

        _mapLoaded = !_mapLoaded;
    }

    private void CrystalCollected()
    {
        Debug.Log("Crystal collected!");
        Time.timeScale = 0f;
        AudioManager.Instance.PlaySound("Crystal");
        TrackCompleteUI.SetActive(true);
    }

    private void ReloadMap(string mapName)
    {
        LoadMap(mapName);
        LoadMap(mapName);
    }

    private void SceneLoadAsyncCompleted(AsyncOperation obj)
    {
        Debug.Log("SCENE LOADED BOI");
    }

    private void ResetPlayer()
    {
        _player.transform.parent = null;

        SceneManager.MoveGameObjectToScene(_player, gameObject.scene);

        // Reload camera target
        var mainCamera = GameObject.Find("Main Camera");
        var cameraScript = mainCamera.GetComponent<CameraControl>();
        cameraScript.Target = _player;

        // Reset position and forces
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

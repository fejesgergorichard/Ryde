using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Saving;

public class GameManager : MonoBehaviour
{
    private SaveData _saveData;

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
        DontDestroyOnLoad(Instance);
    }

    void Start()
    {
        #if DEBUG
            if (ActiveMap == null || ActiveMap == "")
                ActiveMap = "Abstract1";
        #endif
       _saveData = SaveSystem.SaveData;

        TrackCompleteUI.SetActive(false);

        GameEvents.Instance.onCrystalTriggerEnter += OnCrystalCollected;
        GameEvents.Instance.onFallEvent += OnFallEvent;

        Time.timeScale = 1f;
        Score = 0;
    }

    public void Restart()
    {
        TrackCompleteUI.SetActive(false);

        SceneLoader.ReloadMap();

        UpdatePlayerObjects();
        ResetPlayerPosition();
        SetCameraTargetToPlayer();

        Time.timeScale = 1f;
        Score = 0;
    }

    public async void AddCoin(Vector3 coinWorldPos, int num)
    {
        Vector2 coinScreenPos = Camera.main.WorldToScreenPoint(coinWorldPos);

        LeanTween.scale(CoinFlyTarget, Vector3.one, 0.1f).setDelay(0.2f);

        float pitch = 0.9f;
        for (int i = 0; i < num; i++)
        {
            Score++;

            pitch += 0.1f;
            AudioManager.PlaySound("Coin", pitch);

            var coin = Instantiate(CoinSprite, coinScreenPos, Quaternion.identity);
            coin.transform.SetParent(CoinFlyTarget.parent);
            LeanTween.moveLocal(coin, CoinFlyTarget.localPosition, 0.4f).setEaseInQuad().setIgnoreTimeScale(true);
            Destroy(coin, 0.4f);


            if (num > 1)
                await Task.Run(() => Thread.Sleep(65));
        }

        LeanTween.scale(CoinFlyTarget, new Vector3(0.6f, 0.6f, 0.6f), 0.2f).setDelay(0.3f);
    }

    private void OnCrystalCollected()
    {
        Debug.Log("Crystal collected!");
        Time.timeScale = 0f;
        AudioManager.PlaySound("Crystal");
        TrackCompleteUI.SetActive(true);

        var scoreCounterObject = TrackCompleteUI.transform.Find("ScoreCounter");
        scoreCounterObject.GetComponent<TMP_Text>().text = Score.ToString();
    }
    
    private void OnFallEvent()
    {
        Debug.Log("Player fell!");
        AudioManager.PlaySound("Fall");
        Restart();
    }

    private void ResetPlayerPosition()
    {
        if (_player != null)
        {
            _player.transform.parent = null;
            _player.GetComponent<PlayerControls>().ResetPosition();
        }
    }

    private void SetCameraTargetToPlayer()
    {
        var mainCamera = GameObject.Find("Main Camera");
        mainCamera.transform.parent = _player.transform.parent;
        CameraControl cc = mainCamera.GetComponent<CameraControl>();
        cc.Target = _player;
    }

    private void UpdatePlayerObjects()
    {
        _player = GameObject.Find("Player");
        _playerRigidBody = _player.GetComponent<Rigidbody>();
    }
}

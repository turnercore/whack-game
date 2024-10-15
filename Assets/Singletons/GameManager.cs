using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    // The static instance of GameManager
    public static GameManager Instance { get; private set; }
    public bool IsGamePaused { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsGameWon { get; private set; }
    public float timeElapsed = 0f;
    private int Kills = 0;
    private int Coins = 0;
    public int Score => CalculateScore();

    [SerializeField]
    private bool IsTesting = false;

    [SerializeField]
    private GameObject _player;
    public ScreenManager screenManager;
    private string _playerName;
    public string PlayerName
    {
        get
        {
            if (string.IsNullOrEmpty(_playerName))
            {
                _playerName = "TCM";
            }
            return _playerName;
        }
        set
        {
            // Set it to the string, but strip all spaces, only take the first 3 letters, and make them uppercase
            _playerName = value.Replace(" ", "")[..3].ToUpper();
        }
    }

    // Sciptable Object HighScore
    public HighScore highScore;
    public GameObject Player
    {
        get
        {
            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
            }
            return _player;
        }
        set { _player = value; }
    }

    public int GetHighScore()
    {
        return highScore.score;
    }

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Detach from any parent object
        transform.SetParent(null);
        // If no other instance exists, make this the singleton instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager objects
        }
    }

    void Start()
    {
        // Subscribe to EventBus OnEnenmyDied event
        EventBus.Instance.OnEnemyDied += OnEnemyDied;
        EventBus.Instance.OnCoinCollected += OnCoinCollected;
        EventBus.Instance.OnPlayerDied += GameOver;

        // Buttons event subscription
        EventBus.Instance.OnButtonWacked += OnButtonWacked;

        InitializeGame();
    }

    void InitializeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        IsGamePaused = false;
        IsGameOver = false;
        IsGameWon = false;
        Kills = 0;
        Coins = 0;
        timeElapsed = 0f;
        // Change to the menu screen
        if (!IsTesting)
            screenManager.TransitionToScreen(ScreenType.MainMenu, ScreenTransitionType.ZoomInOut);
    }

    int CalculateScore()
    {
        /*
        * The score is calculated based on the following formula:
        * Level * 1000
        * Kills x 100
        * Coins x 50
        * Time Alive * 10
        */
        return Kills * 100 + Coins * 50 + (int)timeElapsed * 10 + GetPlayerComponent().Level * 1000;
    }

    // Clean up
    private void OnDestroy()
    {
        EventBus.Instance.OnEnemyDied -= OnEnemyDied;
        EventBus.Instance.OnCoinCollected -= OnCoinCollected;
        EventBus.Instance.OnPlayerDied -= GameOver;
    }

    private void Update()
    {
        if (!IsGamePaused && !IsGameOver && !IsGameWon)
        {
            timeElapsed += Time.deltaTime;
        }
    }

    public int GetKills()
    {
        return Kills;
    }

    public int GetCoins()
    {
        return Coins;
    }

    void OnCoinCollected(int value)
    {
        Coins += value;
    }

    void OnEnemyDied(Enemy enemy)
    {
        Kills++;
    }

    public void RegisterPlayer(GameObject player)
    {
        // Register the player with the GameManager
        Player = player;
        OnPlayerSet?.Invoke(player);
    }

    public GameObject GetPlayerObject()
    {
        return Player;
    }

    public PlayerController GetPlayerComponent()
    {
        return Player.GetComponent<PlayerController>();
    }

    // OnPlayerSet Event
    public delegate void PlayerSetDelegate(GameObject player);
    public event PlayerSetDelegate OnPlayerSet;

    // Game over event
    public delegate void GameOverDelegate();
    public event GameOverDelegate OnGameOver;

    public void GameOver()
    {
        IsGameOver = true;
        UpdateHighScore(Score, PlayerName);
        OnGameOver?.Invoke();
    }

    private void UpdateHighScore(int score, string name)
    {
        if (score > highScore.score)
        {
            highScore.score = score;
            highScore.PlayerInitials = name;
            highScore.SaveData();
        }
    }

    private void OnButtonWacked(ButtonTypes buttonType)
    {
        switch (buttonType)
        {
            case ButtonTypes.Pause:
                PauseGame();
                break;
            case ButtonTypes.Resume:
                ResumeGame();
                break;
            case ButtonTypes.Restart:
                RestartGame();
                break;
            case ButtonTypes.Quit:
                Debug.Log("Quit Game");
                QuitGame();
                break;
            case ButtonTypes.MainMenu:
                screenManager.TransitionToScreen(
                    ScreenType.MainMenu,
                    ScreenTransitionType.ZoomInOut
                );
                break;
            case ButtonTypes.Play:
                screenManager.TransitionToScreen(ScreenType.Level, ScreenTransitionType.ZoomInOut);
                break;
            case ButtonTypes.Options:
                break;
            case ButtonTypes.Credits:
                screenManager.TransitionToScreen(
                    ScreenType.Credits,
                    ScreenTransitionType.ZoomInOut
                );
                break;
        }
    }

    public void PauseGame()
    {
        IsGamePaused = true;
        EventBus.Instance.TriggerGamePaused();
    }

    public void ResumeGame()
    {
        IsGamePaused = false;
        EventBus.Instance.TriggerGameResumed();
    }

    public void RestartGame()
    {
        IsGameOver = false;
        IsGameWon = false;
        InitializeGame();
        EventBus.Instance.TriggerGameResumed();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GameWon()
    {
        IsGameWon = true;
        UpdateHighScore(Score, PlayerName);
        EventBus.Instance.TriggerGameWon();
    }
}

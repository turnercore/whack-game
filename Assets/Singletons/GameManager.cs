using UnityEngine;

public class GameManager : MonoBehaviour
{
    const int SCORE_VALUE_TIME = 1;
    const int SCORE_VALUE_DAMAGE = 10;
    const int SCORE_VALUE_COIN = 100;
    const int SCORE_VALUE_LEVEL = 1000;

    // The static instance of GameManager
    [SerializeField]
    private Texture2D cursorSprite;

    [SerializeField]
    private CameraController cameraContoller;
    public static GameManager Instance { get; private set; }
    public bool IsGamePaused { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsGameWon { get; private set; }
    public float timeElapsed = 0f;
    private int Kills = 0;
    private int Coins = 0;
    private int _score = 0;
    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            EventBus.Instance.TriggerScoreChanged(_score);
        }
    }

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
        EventBus.Instance.OnPlayerDied += OnPlayerDied;

        // Buttons event subscription
        EventBus.Instance.OnButtonWacked += OnButtonWacked;

        // Scoring Event, did damage
        EventBus.Instance.OnEnemyHit += UpdateScoreDidDamge;
        EventBus.Instance.OnLevelUp += UpdateScoreLevelUp;

        InitializeGame();
    }

    void InitializeGame()
    {
        // Set cursor sprite
        Cursor.SetCursor(cursorSprite, Vector2.zero, CursorMode.Auto);
        // Confine cursor to the game window
        Cursor.lockState = CursorLockMode.Confined;
        IsGamePaused = false;
        IsGameOver = false;
        IsGameWon = false;
        Kills = 0;
        Coins = 0;
        Score = 0;
        timeElapsed = 0f;
        // Change to the menu screen
        if (!IsTesting)
        {
            screenManager.TransitionToScreen(ScreenType.MainMenu, ScreenTransitionType.ZoomInOut);
        }
    }

    // Clean up
    private void OnDestroy()
    {
        EventBus.Instance.OnEnemyDied -= OnEnemyDied;
        EventBus.Instance.OnCoinCollected -= OnCoinCollected;
        EventBus.Instance.OnPlayerDied -= OnPlayerDied;
    }

    private void Update()
    {
        if (!IsGamePaused && !IsGameOver && !IsGameWon)
        {
            timeElapsed += Time.deltaTime;
        }
    }

    private void UpdateScoreEnemyDied(Enemy enemy)
    {
        Score += enemy.score;
    }

    private void UpdateScoreDidDamge(float damage)
    {
        // Round down to the nearest integer
        Score += (int)damage * SCORE_VALUE_DAMAGE;
    }

    private void UpdateScoreCoinCollected(int value)
    {
        Score += value * SCORE_VALUE_COIN;
    }

    private void UpdateScoreTimeSurvived() => Score += (int)timeElapsed * SCORE_VALUE_TIME;

    private void UpdateScoreLevelUp() => Score += SCORE_VALUE_LEVEL;

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
        UpdateScoreCoinCollected(value);
    }

    void OnEnemyDied(Enemy enemy)
    {
        Kills++;
        UpdateScoreEnemyDied(enemy);
    }

    public void RegisterPlayer(GameObject player)
    {
        // Register the player with the GameManager
        Player = player;
        EventBus.Instance.TriggerPlayerSet(player);
    }

    public GameObject GetPlayerObject()
    {
        return Player;
    }

    public PlayerController GetPlayerComponent()
    {
        return Player.GetComponent<PlayerController>();
    }

    public void OnPlayerDied()
    {
        IsGameOver = true;
        UpdateScoreTimeSurvived();
        screenManager.TransitionToScreen(ScreenType.GameOver, ScreenTransitionType.ZoomInOut);
        EventBus.Instance.TriggerGameOver();
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
                timeElapsed = 0f;
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

        // Reset the player
        Player.GetComponent<PlayerController>().ResetPlayer();
        EventBus.Instance.TriggerScoreChanged(Score);

        EventBus.Instance.TriggerGameRestarted();
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

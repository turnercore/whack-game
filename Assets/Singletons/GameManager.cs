using UnityEngine;

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
    private GameObject _player;
    public ScreenManager screenManager;

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
        InitializeGame();

        // Subscribe to EventBus OnEnenmyDied event
        EventBus.Instance.OnEnemyDied += OnEnemyDied;
        EventBus.Instance.OnCoinCollected += OnCoinCollected;
        EventBus.Instance.OnPlayerDied += GameOver;
    }

    void InitializeGame()
    {
        // Initialize the game state
        IsGamePaused = false;
        IsGameOver = false;
        IsGameWon = false;
        Kills = 0;
        Coins = 0;
        timeElapsed = 0f;
    }

    int CalculateScore()
    {
        return Kills * 100 + Coins * 50 + (int)timeElapsed * 10 + GetPlayerComponent().Level * 1000;
    }

    // Clean up
    private void OnDestroy()
    {
        // Unsubscribe from the event
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
        UpdateHighScore();
        OnGameOver?.Invoke();
    }

    private void UpdateHighScore()
    {
        if (Score > highScore.score)
        {
            highScore.score = Score;
        }
        highScore.SaveData();
    }

    private void UpdateHighScorePlayer(string name)
    {
        highScore.PlayerInitials = name;
        highScore.SaveData();
    }
}

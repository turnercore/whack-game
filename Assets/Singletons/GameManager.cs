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
    private int Score = 0;
    private GameObject _player;
    public GameObject Player {
        get 
        {
            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
            }
            return _player;
        }
        set
        {
            _player = value;
            OnSetPlayer(value);
        }
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
    }

    void InitializeGame()
    {
        // Initialize the game state
        IsGamePaused = false;
        IsGameOver = false;
        IsGameWon = false;
        Kills = 0;
        Coins = 0;
        Score = 0;
        timeElapsed = 0f;
    }

    // Clean up
    private void OnDestroy()
    {
        // Unsubscribe from the event
        EventBus.Instance.OnEnemyDied -= OnEnemyDied;
        EventBus.Instance.OnCoinCollected -= OnCoinCollected;
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

    // Your globally accessible methods and variables
    public int playerScore = 0;

    public void AddScore(int points)
    {
        playerScore += points;
        Debug.Log("Score Added: " + points);
    }
    public void RegisterPlayer(GameObject player)
    {
        // Register the player with the GameManager
        Debug.Log("Player registered with GameManager.");
        this.Player = player;
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
    public event PlayerSetDelegate PlayerSet;

    private void OnSetPlayer(GameObject player)
    {
        PlayerSet?.Invoke(player);
    }
}

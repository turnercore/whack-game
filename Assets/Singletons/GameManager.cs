using UnityEngine;

public class GameManager : MonoBehaviour
{
    // The static instance of GameManager
    public static GameManager Instance { get; private set; }
    public static bool IsGamePaused { get; private set; }
    public static bool IsGameOver { get; private set; }
    public static bool IsGameWon { get; private set; }
    private int Kills = 0;
    private int Coins = 0;
    private int Score = 0;
    private GameObject player;

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
        // Initialize the game state
        IsGamePaused = false;
        IsGameOver = false;
        IsGameWon = false;
        Kills = 0;

        // Subscribe to EventBus OnEnenmyDied event
        EventBus.Instance.OnEnemyDied += OnEnemyDied;
        EventBus.Instance.OnCoinCollected += OnCoinCollected;
    }

    // Clean up
    private void OnDestroy()
    {
        // Unsubscribe from the event
        EventBus.Instance.OnEnemyDied -= OnEnemyDied;
        EventBus.Instance.OnCoinCollected -= OnCoinCollected;
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
        this.player = player;
    }
    public GameObject GetPlayerObject()
    {
        return player;
    }
    public PlayerController GetPlayerComponent()
    {
        return player.GetComponent<PlayerController>();
    }
}

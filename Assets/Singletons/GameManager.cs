using UnityEngine;

public class GameManager : MonoBehaviour
{
    // The static instance of GameManager
    public static GameManager Instance { get; private set; }
    public static bool IsGamePaused { get; private set; }
    public static bool IsGameOver { get; private set; }
    public static bool IsGameWon { get; private set; }
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

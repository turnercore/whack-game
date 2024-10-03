using UnityEngine;
using System; // For Action and EventHandler

public class EventBus : MonoBehaviour
{
    // The static instance of EventBus
    public static EventBus Instance { get; private set; }

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
            Destroy(gameObject); // Destroy duplicate EventBus objects
        }
    }

    // Example of event declaration using Action

    /*
    Global events can be declared here, and other scripts can subscribe to them.
    When the event is triggered, all subscribed methods will be called.

    Events:
    - OnPlayerDied: Triggered when the player dies
    - OnPlayerHit: Triggered when the player is hit
    - OnEnemyDied: Triggered when an enemy dies
    - OnGameWon: Triggered when the game is won
    - OnGameOver: Triggered when the game is over
    - OnGamePaused: Triggered when the game is paused
    - OnGameResumed: Triggered when the game is resumed

    */

    // OnPlayerDied event trigger
    public event Action OnPlayerDied;
    public void PlayerDied()
    {
        OnPlayerDied?.Invoke(); // Safely invoke event
    }
    public void SubscribeToPlayerDeath(Action listener)
    {
        OnPlayerDied += listener;
    }
    public void UnsubscribeFromPlayerDeath(Action listener)
    {
        OnPlayerDied -= listener;
    }

    // OnPlayerHit event trigger
    public event Action<float, Vector2> OnPlayerHit;
    public void PlayerHit(float damage, Vector2 hitDirection)
    {
        OnPlayerHit?.Invoke(damage, hitDirection); // Safely invoke event
    }
    public void SubscribeToPlayerHit(Action<float, Vector2> listener)
    {
        OnPlayerHit += listener;
    }
    public void UnsubscribeFromPlayerHit(Action<float, Vector2> listener)
    {
        OnPlayerHit -= listener;
    }

    // OnEnemyDied event trigger
    public event Action<Enemy> OnEnemyDied;
    public void EnemyDied(Enemy enemy)
    {
        OnEnemyDied?.Invoke(enemy); // Safely invoke event
    }
    public void SubscribeToEnemyDeath(Action<Enemy> listener)
    {
        OnEnemyDied += listener;
    }

    public void UnsubscribeFromEnemyDeath(Action<Enemy> listener)
    {
        OnEnemyDied -= listener;
    }

}

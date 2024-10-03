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
    - OnXPChanged: Triggered when the player's XP changes
    - OnLevelUp: Triggered when the player levels up
    - OnCoinCollected: Triggered when a coin is collected

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
        OnEnemyDied?.Invoke(enemy);
        KillsTextUpdate(); // Trigger KillsTextUpdate event
    }
    public void SubscribeToEnemyDeath(Action<Enemy> listener)
    {
        OnEnemyDied += listener;
    }

    public void UnsubscribeFromEnemyDeath(Action<Enemy> listener)
    {
        OnEnemyDied -= listener;
    }

    // OnXPChanged event trigger
    public event Action OnXPChanged;
    public void XPChanged()
    {
        OnXPChanged?.Invoke(); // Safely invoke event
    }
    public void SubscribeToXPChanged(Action listener)
    {
        OnXPChanged += listener;
    }
    public void UnsubscribeFromXPChanged(Action listener)
    {
        OnXPChanged -= listener;
    }

    // OnLevelUp event trigger
    public event Action OnLevelUp;
    public void LevelUp()
    {
        OnLevelUp?.Invoke(); // Safely invoke event
        OnXPChanged?.Invoke(); // Trigger XPChanged event
    }
    public void SubscribeToLevelUp(Action listener)
    {
        OnLevelUp += listener;
    }
    public void UnsubscribeFromLevelUp(Action listener)
    {
        OnLevelUp -= listener;
    }

    // OnCoinCollected event trigger
    public event Action<int> OnCoinCollected;
    public void CoinCollected(int value)
    {
        OnCoinCollected?.Invoke(value); // Safely invoke event
    }
    public void SubscribeToCoinCollected(Action<int> listener)
    {
        OnCoinCollected += listener;
    }
    public void UnsubscribeFromCoinCollected(Action<int> listener)
    {
        OnCoinCollected -= listener;
    }

    // OnKillsTextUpdate event trigger
    public event Action OnKillsTextUpdate;
    public void KillsTextUpdate()
    {
        OnKillsTextUpdate?.Invoke(); // Safely invoke event
    }
    public void SubscribeToKillsTextUpdate(Action listener)
    {
        OnKillsTextUpdate += listener;
    }
    public void UnsubscribeFromKillsTextUpdate(Action listener)
    {
        OnKillsTextUpdate -= listener;
    }
}

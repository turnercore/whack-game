using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance { get; private set; }

    private void Awake()
    {
        transform.SetParent(null);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Player events
    public event Action OnPlayerDied;

    public void TriggerPlayerDied() => OnPlayerDied?.Invoke();

    public event Action<float, Vector2> OnPlayerHit;

    public void TriggerPlayerHit(float damage, Vector2 hitDirection) =>
        OnPlayerHit?.Invoke(damage, hitDirection);

    // Enemy events
    public event Action<Enemy> OnEnemyDied;

    public void TriggerEnemyDied(Enemy enemy)
    {
        OnEnemyDied?.Invoke(enemy);
        TriggerKillsTextUpdate();
    }

    // Game state events
    public event Action OnGameWon;

    public void TriggerGameWon() => OnGameWon?.Invoke();

    public event Action OnGameOver;

    public void TriggerGameOver() => OnGameOver?.Invoke();

    public event Action OnGamePaused;

    public void TriggerGamePaused() => OnGamePaused?.Invoke();

    public event Action OnGameResumed;

    public void TriggerGameResumed() => OnGameResumed?.Invoke();

    // XP and level events
    public event Action OnXPChanged;

    public void TriggerXPChanged() => OnXPChanged?.Invoke();

    public event Action OnLevelUp;

    public void TriggerLevelUp()
    {
        OnLevelUp?.Invoke();
        TriggerXPChanged();
    }

    // Coin events
    public event Action<int> OnCoinCollected;

    public void TriggerCoinCollected(int value) => OnCoinCollected?.Invoke(value);

    // UI events
    public event Action OnKillsTextUpdate;

    public void TriggerKillsTextUpdate() => OnKillsTextUpdate?.Invoke();

    // Wackable button events
    public event Action OnWackableButtonsEnabled;

    public void TriggerWackableButtonsEnabled() => OnWackableButtonsEnabled?.Invoke();

    public event Action OnWackableButtonsDisabled;

    public void TriggerWackableButtonsDisabled() => OnWackableButtonsDisabled?.Invoke();

    public event Action<ButtonTypes> OnButtonWacked;

    public void TriggerButtonWacked(ButtonTypes buttonType) => OnButtonWacked?.Invoke(buttonType);
}

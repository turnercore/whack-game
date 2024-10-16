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

    public event Action OnPlayerDash;

    public void TriggerPlayerDash() => OnPlayerDash?.Invoke();

    public event Action OnPlayerDashEnd;

    public void TriggerPlayerDashEnd() => OnPlayerDashEnd?.Invoke();

    public event Action OnPlayerHealed;

    public void TriggerPlayerHealed() => OnPlayerHealed?.Invoke();

    // Enemy events
    public event Action<Enemy> OnEnemyDied;

    public void TriggerEnemyDied(Enemy enemy)
    {
        OnEnemyDied?.Invoke(enemy);
        TriggerKillsTextUpdate();
    }

    public event Action<float> OnEnemyHit;

    public void TriggerEnemyHit(float damage) => OnEnemyHit?.Invoke(damage);

    // Game state events
    public event Action OnGameWon;

    public void TriggerGameWon() => OnGameWon?.Invoke();

    public event Action OnGameOver;

    public void TriggerGameOver() => OnGameOver?.Invoke();

    public event Action OnGamePaused;

    public void TriggerGamePaused() => OnGamePaused?.Invoke();

    public event Action OnGameResumed;

    public void TriggerGameResumed() => OnGameResumed?.Invoke();

    public event Action<GameObject> OnPlayerSet;

    public void TriggerPlayerSet(GameObject player) => OnPlayerSet?.Invoke(player);

    public event Action OnGameStarted;

    public void TriggerGameStarted() => OnGameStarted?.Invoke();

    public event Action OnGameRestarted;

    public void TriggerGameRestarted() => OnGameRestarted?.Invoke();

    public event Action OnGameQuit;

    public void TriggerGameQuit() => OnGameQuit?.Invoke();

    // XP and level events
    public event Action OnXPChanged;

    public void TriggerXPChanged() => OnXPChanged?.Invoke();

    public event Action OnLevelUp;

    public void TriggerLevelUp()
    {
        OnLevelUp?.Invoke();
        TriggerXPChanged();
    }

    public event Action<int> OnScoreChanged;

    public void TriggerScoreChanged(int value) => OnScoreChanged?.Invoke(value);

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

    // Camera events
    public event Action OnCameraZoomIn;

    public void TriggerCameraZoomIn() => OnCameraZoomIn?.Invoke();

    public event Action OnCameraZoomOut;

    public void TriggerCameraZoomOut() => OnCameraZoomOut?.Invoke();

    // Audio events
    public event Action<AudioClip, int> OnAudioSFXPlayed;

    public void TriggerAudioSFXPlayed(AudioClip sfx, int triggeringObjectId) =>
        OnAudioSFXPlayed?.Invoke(sfx, triggeringObjectId);
}

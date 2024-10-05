using System.Collections;
using UnityEngine;

public class EnemyEmote : MonoBehaviour
{
    [Header("Linked Components")]
    [SerializeField] private float ouchEmoteLength = 0.5f;
    [SerializeField] private float stunnedEmoteLength = 1.0f;
    [SerializeField] private float worriedEmoteLength = 1.5f;
    [SerializeField] private float angryEmoteLength = 0.7f;
    [SerializeField] private float yellingEmoteLength = 1.0f;
    [SerializeField] private Enemy enemy;
    [SerializeField] private Animator emoteAnimator;
    [SerializeField] private OffScreenChecker offScreenChecker;
    [SerializeField] private Health health;
    [SerializeField] private bool isBoss = false;

    private float emoteTimer = 0f;
    private bool hasYelled = false;
    private bool isCurrentlyWacked => enemy.IsWacked;
    private EmoteState currentState;
    private bool isTransitioning = false;

    private enum EmoteState
    {
        Blank,
        Ouch,
        Stunned,
        Worried,
        Angry,
        Yelling
    }

    void Start()
    {
        health.OnDeath += OnDeath;
        offScreenChecker.OnFirstEnterScreen += OnFirstEnterScreen;
        offScreenChecker.OnScreenStatusChanged += OnScreenStatusChanged;
        TransitionToState(EmoteState.Blank);
    }

    void OnDestroy()
    {
        health.OnDeath -= OnDeath;
        offScreenChecker.OnFirstEnterScreen -= OnFirstEnterScreen;
        offScreenChecker.OnScreenStatusChanged -= OnScreenStatusChanged;
    }

    void Update()
    {
        HandleStateTransitions();
        UpdateEmoteTimer();
    }

    private void OnDeath()
    {
        DisableEmotes();
    }

    private void DisableEmotes()
    {
        emoteAnimator.gameObject.SetActive(false);
    }

    private void HandleStateTransitions()
    {
        if (isTransitioning || enemy.IsDead)
        {
            return;
        }

        switch (currentState)
        {
            case EmoteState.Blank:
                if (isCurrentlyWacked)
                {
                    TransitionToState(EmoteState.Ouch, ouchEmoteLength);
                }
                break;

            case EmoteState.Ouch:
                if (emoteTimer <= 0)
                {
                    TransitionToState(EmoteState.Stunned, stunnedEmoteLength);
                }
                break;

            case EmoteState.Stunned:
                if (emoteTimer <= 0)
                {
                    if (!isCurrentlyWacked)
                    {
                        if (health.CurrentHealth <= Mathf.CeilToInt(health.MaxHealth * 0.25f))
                        {
                            TransitionToState(EmoteState.Worried, worriedEmoteLength);
                        }
                        else if (Random.value < 0.2f)
                        {
                            TransitionToState(EmoteState.Angry, angryEmoteLength);
                        }
                        else
                        {
                            TransitionToState(EmoteState.Blank);
                        }
                    }
                }
                break;

            case EmoteState.Worried:
                if (isCurrentlyWacked)
                {
                    TransitionToState(EmoteState.Ouch, ouchEmoteLength);
                }
                else if (emoteTimer <= 0)
                {
                    TransitionToState(EmoteState.Blank);
                }
                break;

            case EmoteState.Angry:
                if (isCurrentlyWacked)
                {
                    TransitionToState(EmoteState.Ouch, ouchEmoteLength);
                }
                else if (emoteTimer <= 0)
                {
                    TransitionToState(EmoteState.Blank);
                }
                break;

            case EmoteState.Yelling:
                if (emoteTimer <= 0)
                {
                    TransitionToState(EmoteState.Blank);
                }
                break;
        }
    }

    private void OnFirstEnterScreen()
    {
        if (isBoss && !hasYelled)
        {
            hasYelled = true;
            TransitionToState(EmoteState.Yelling, yellingEmoteLength);
        }
    }

    private void OnScreenStatusChanged(bool isOnScreen)
    {
        if (!isOnScreen && currentState != EmoteState.Blank)
        {
            TransitionToState(EmoteState.Blank);
        }
    }

    private void TransitionToState(EmoteState newState, float duration = 0f)
    {
        if (currentState == newState || isTransitioning) return;

        isTransitioning = true;
        currentState = newState;
        emoteTimer = duration;
        ChangeEmote(newState);
        isTransitioning = false;
    }

    private void UpdateEmoteTimer()
    {
        if (emoteTimer > 0)
        {
            emoteTimer -= Time.deltaTime;
        }
    }

    private void ChangeEmote(EmoteState emoteState)
    {
        if (enemy.IsDead) return;
        if (emoteAnimator != null) emoteAnimator.SetInteger("EmoteType", (int)emoteState);
    }
}
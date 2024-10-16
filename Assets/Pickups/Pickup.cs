using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // The speed at which the pickup will float towards the player
    [SerializeField]
    protected float floatSpeed = 5f;

    [SerializeField]
    protected AudioClip pickupSound;

    // Called when the pickup is collected by a player
    public void TriggerPickup(GameObject player)
    {
        // This is the default behavior for floating to the player
        // You can override this method in derived classes if needed
        StartCoroutine(FloatToPlayer(player));
    }

    // Coroutine to move the pickup to the player over time
    protected IEnumerator FloatToPlayer(GameObject player)
    {
        // Move towards the player's position until close enough
        while (Vector2.Distance(transform.position, player.transform.position) > 0.1f)
        {
            // Move towards the player
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.transform.position,
                floatSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Once reached the player, do something (to be defined in derived classes)
        OnReachPlayer(player);
    }

    // This function can be overridden by derived classes to define what happens when the pickup reaches the player
    protected virtual void OnReachPlayer(GameObject player)
    {
        // Play the pickup sound if it exists
        PlayPickupSound();
        // Destroy the pickup object by default
        Destroy(gameObject);
    }

    // Play the pickup sound if it exists
    protected void PlayPickupSound()
    {
        if (pickupSound != null)
        {
            EventBus.Instance.TriggerAudioSFXPlayed(pickupSound, gameObject.GetInstanceID());
        }
    }

    // Start
    protected virtual void Start()
    {
        // Subscribe to EventBus audio sfx event played
        EventBus.Instance.OnGameOver += OnGameOver;
    }

    // OnDestroy
    protected virtual void OnDestroy()
    {
        // Unsubscribe from EventBus audio sfx event played
        EventBus.Instance.OnGameOver -= OnGameOver;
    }

    // OnGameOver
    protected void OnGameOver()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // The speed at which the pickup will float towards the player
    public float floatSpeed = 5f;

    // Called when the pickup is collected by a player
    public virtual void PickupItem(GameObject player)
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
        // Destroy the pickup object by default
        Destroy(gameObject);
    }
}

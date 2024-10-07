using UnityEngine;

public class BackAwayFromPlayer : Juice
{
    public float retreatSpeed = 2f; // Speed at which the object retreats
    public float retreatDistance = 5f; // Maximum distance to retreat away from the player

    private Transform playerTransform;
    private Vector3 originalPosition;

    void Start()
    {
        // Store the original position of the object to know when it has retreated sufficiently
        originalPosition = transform.position;
    }

    void Update()
    {
        if (isActive && playerTransform != null)
        {
            // Calculate direction away from the player
            Vector3 directionAway = (transform.position - playerTransform.position).normalized;
            Vector3 targetPosition =
                transform.position + directionAway * retreatSpeed * Time.deltaTime;

            // Only retreat if within retreat distance from the original position
            if (Vector3.Distance(originalPosition, targetPosition) <= retreatDistance)
            {
                transform.position = targetPosition;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the trigger
        if (other.CompareTag("Player"))
        {
            isActive = true;
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player has exited the trigger
        if (other.CompareTag("Player"))
        {
            isActive = false;
            playerTransform = null;
        }
    }
}

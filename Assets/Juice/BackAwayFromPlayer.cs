using UnityEngine;

public class BackAwayFromPlayer : Juice
{
    public float retreatForce = 200f; // Force to apply when retreating
    public float retreatDistance = 5f; // Maximum distance to retreat away from the player

    private Transform playerTransform => GameManager.Instance.Player.transform;
    private Rigidbody2D rb;
    private bool isRetreating;

    void Start()
    {
        if (juiceTarget == null)
        {
            Debug.LogError("Juice Target not set!");
        }
        rb = juiceTarget.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D found on the GameObject!");
        }
    }

    void Update()
    {
        if (isRetreating && playerTransform != null && rb != null)
        {
            // Calculate the direction away from the player
            Vector3 directionAway = (transform.position - playerTransform.position).normalized;

            // Apply force in the opposite direction of the player
            if (Vector3.Distance(playerTransform.position, transform.position) <= retreatDistance)
            {
                rb.AddForce(directionAway * retreatForce * Time.deltaTime, ForceMode2D.Force);
            }
            else
            {
                // Stop retreating once the object is beyond the retreat distance
                isRetreating = false;
                rb.velocity = Vector2.zero; // Stop any movement
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the trigger
        if (other.CompareTag("Player"))
        {
            isRetreating = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player has exited the trigger
        if (other.CompareTag("Player"))
        {
            isRetreating = false;
            rb.velocity = Vector2.zero; // Stop any movement
        }
    }
}

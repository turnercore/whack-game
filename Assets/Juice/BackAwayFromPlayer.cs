using UnityEngine;

public class BackAwayFromPlayer : Juice
{
    public float retreatForce = 200f; // Force to apply when retreating
    public float retreatDistance = 5f; // Maximum distance to retreat away from the player

    private Vector3 playerLocation =>
        GameManager.Instance.Player.GetComponent<PlayerController>().GetPosition();
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
        // Retreat unless we're beyond the retreat distance
        if (isRetreating)
        {
            // Calculate the direction away from the player
            Vector3 directionAway = (transform.position - playerLocation).normalized;

            // Apply force in the opposite direction of the player
            if (Vector3.Distance(playerLocation, transform.position) <= retreatDistance)
            {
                rb.AddForce(retreatForce * Time.deltaTime * directionAway, ForceMode2D.Force);
            }
            else
            {
                rb.velocity = Vector2.zero; // Stop any movement
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the trigger
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            isRetreating = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player has exited the trigger
        if (other.CompareTag("Player"))
        {
            isRetreating = false;
        }
    }
}

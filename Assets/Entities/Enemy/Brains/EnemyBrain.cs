using UnityEngine;

public abstract class EnemyBrain : MonoBehaviour
{
    protected Transform playerTransform;
    protected Enemy enemy;
    private bool isInitialized = false;
    private GameObject player;

    protected virtual void Start()
    {
        // Get reference to the player
        player = GameManager.Instance.GetPlayerObject();
        if (player == null)
        {
            // Try to find the player by tag
            player = GameObject.FindWithTag("Player");
        }

        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found.");
            enabled = false; // Disable this script if the player is not found
            return;
        }

        // Get reference to the Enemy component
        enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogWarning("Enemy component not found on this GameObject. Disabling AI.");
            enabled = false; // Disable this script if the Enemy component is not found
            return;
        }

        // Subscribe to the OnDeath event on the Health component, if it exists
        if (TryGetComponent<Health>(out var health))
        {
            health.OnDeath += Die;
        }

        // Initialize the brain
        isInitialized = true;
    }

    protected virtual void Die()
    {
        // Disable the brain when the enemy dies
        enabled = false;
    }

    protected virtual void FixedUpdate()
    {
        if (isInitialized && !enemy.IsDead)
        {
            // To be implemented in derived classes
            Act();
        }
    }

    // Abstract method to be implemented by derived classes
    protected abstract void Act();

    // Utility method for movement (for common use among derived classes)
    protected void MoveTowards(Vector2 direction)
    {
        if (enemy != null && enemy.rb != null)
        {
            // Move towards the specified direction at the speed defined in Enemy
            enemy.rb.velocity = direction.normalized * enemy.speed;
        }
        else
        {
            Debug.LogWarning("Enemy or Rigidbody2D reference is missing. Cannot move.");
        }
    }
}

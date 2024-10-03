using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private const float FORCE_HIT_REDUCTION = 0.5f;
    private const float DAMAGE_HIT_REDUCTION = 0.5f;
    private Enemy parentEnemy;

    private void Awake()
    {
        // Get the Enemy component on the parent
        parentEnemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ensure the parent enemy is "wacked" and ready to trigger
        if (!parentEnemy.IsWacked || !other.CompareTag("Enemy")) return;
        // Log the collision

        // Check if the collider is an enemy and not the parent
        if (other.gameObject != parentEnemy.gameObject)
        {
            var otherEnemy = other.GetComponent<Enemy>();
            // Check if the other enemy is not already wacked
            if (otherEnemy != null && !otherEnemy.IsWacked)
            {
                // Use the position difference to calculate a hit direction
                Vector2 contactNormal = (other.transform.position - transform.position).normalized;
                // Calculate the force of the hit, should be half of the parent's force hit
                //float forceHit = parentEnemy.ForceHit * FORCE_HIT_REDUCTION;
                // Calculate the damage of the hit, should be half of the parent's force damage
                float forceDamage = parentEnemy.DamageHit * DAMAGE_HIT_REDUCTION;
                //If damage is less than 0.5, ignore the hit
                if (forceDamage < 0.5f) return;
                // Calculate added force of the hit, should be half of the parent's added force
                float addedForce = parentEnemy.AddedForceHit * FORCE_HIT_REDUCTION;
                // Hit the other enemy
                otherEnemy.Hit(contactNormal, forceDamage);
                //Give it a tiny bit of angular pulse
                otherEnemy.rb.AddTorque(0.1f, ForceMode2D.Impulse);
            }
        }
    }
}

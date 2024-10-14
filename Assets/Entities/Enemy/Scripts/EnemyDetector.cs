using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class EnemyDetector : MonoBehaviour
{
    public float comboMultiplier = 1.0f; // Combo multiplier starts at 1
    public float multiplierIncrease = 1.2f; // How much the multiplier increases on each hit
    public ComboMultiplierMode multiplierMode = ComboMultiplierMode.Additive; // How the multiplier is calculated

    private Enemy parentEnemy;

    [SerializeField]
    private PolygonCollider2D polygonCollider;

    [SerializeField]
    private ComboIndicator comboIndicator;

    private void OnEnable()
    {
        // Ensure the polygon collider is enabled
        polygonCollider.enabled = true;
    }

    private void OnDisable()
    {
        // Ensure the polygon collider is disabled
        polygonCollider.enabled = false;
        comboMultiplier = 1.0f; // Reset combo multiplier when disabled
    }

    private void Awake()
    {
        // Get the Enemy component on the parent
        parentEnemy = GetComponentInParent<Enemy>();
    }

    // New combo system. When the enemy hits another enemy, we'll increase the combo multiplier and apply increased damage and force to the next enemy.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ensure the parent enemy is "wacked" and ready to trigger
        if (!parentEnemy.IsWacked || !other.CompareTag("Enemy"))
            return;

        // Check if the collider is an enemy and not the parent
        if (other.gameObject != parentEnemy.gameObject)
        {
            var otherEnemy = other.GetComponent<Enemy>();
            // Check if the other enemy is not already wacked
            if (otherEnemy != null && !otherEnemy.IsWacked)
            {
                // Use the position difference to calculate a hit direction
                Vector2 contactNormal = (other.transform.position - transform.position).normalized;

                // Calculate the force of the hit using the combo multiplier
                float forceDamage = parentEnemy.DamageHit * comboMultiplier;
                float addedForce = parentEnemy.AddedForceHit * comboMultiplier;

                // Hit the other enemy
                otherEnemy.Hit(contactNormal, forceDamage);
                otherEnemy.rb.AddForce(contactNormal * addedForce, ForceMode2D.Impulse);
                otherEnemy.rb.AddTorque(0.1f, ForceMode2D.Impulse);

                // Increase the combo multiplier for the next hit
                switch (multiplierMode) {
                    case MultiplierMode.Additive:
                        comboMultiplier += multiplierIncrease;  // Additive mode
                        break;  
                        case MultiplierMode.Multiplicative:
                        comboMultiplier *= multiplierIncrease;  // Multiplicative mode
                        break;
                        default:
                        comboMultiplier += multiplierIncrease;  // Additive mode
                        break;

                    }
                
                
                                comboMultiplier += multiplierIncrease;  // Additive mode
                else

            }
        }
    }
}

public enum ComboMultiplierMode
{
    Additive,
    Multiplicative,
}

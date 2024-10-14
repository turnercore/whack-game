using TMPro;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class EnemyDetector : MonoBehaviour
{
    public float comboMultiplier = 1.0f; // Combo multiplier starts at 1
    public float multiplierIncrease = 1.2f; // How much the multiplier increases on each hit
    public float damage = 0.0f;
    public float force = 0.0f;
    public float addedWackedTime = 0.0f;
    public ComboMultiplierMode multiplierMode = ComboMultiplierMode.None; // How the multiplier is calculated

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
                // Multiply the damage by the combo mulitplier for the new damage
                damage = parentEnemy.DamageHit * comboMultiplier;

                // Multiply the force by the combo mulitplier for the new force
                force = parentEnemy.AddedForceHit;

                // Increase the multiplier
                switch (multiplierMode)
                {
                    case ComboMultiplierMode.Additive:
                        comboMultiplier += multiplierIncrease; // Additive mode
                        break;
                    case ComboMultiplierMode.Multiplicative:
                        comboMultiplier *= multiplierIncrease; // Multiplicative mode
                        break;
                    default:
                        comboMultiplier += multiplierIncrease; // Additive mode
                        break;
                }

                // Hit the other enemy, passing on the combo values
                Vector2 contactNormal = (other.transform.position - transform.position).normalized;
                otherEnemy.Hit(
                    contactNormal,
                    damage,
                    multiplierMode,
                    multiplierIncrease,
                    comboMultiplier,
                    force,
                    addedWackedTime
                );

                // Reset my own combo multiplier values
                comboMultiplier = 1.0f;
                damage = 0.0f;
                force = 0.0f;
                addedWackedTime = 0.0f;
                multiplierMode = ComboMultiplierMode.None;
            }
        }
    }
}

public enum ComboMultiplierMode
{
    Additive,
    Multiplicative,
    None,
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class EnemyDetector : MonoBehaviour
{
    public float comboMultiplier = 1.0f; // Combo multiplier starts at 1
    public float multiplierIncrease = 1.2f; // How much the multiplier increases on each hit
    public float baseDamage = 0.0f;
    public float damage = 0.0f;
    public float force = 0.0f;
    public float addedWackedTime = 0.0f;
    public ComboMultiplierMode multiplierMode = ComboMultiplierMode.None; // How the multiplier is calculated

    [SerializeField]
    private float pulseRadius = 1.0f;

    [SerializeField]
    private float pulseForce = 1.0f;

    private Enemy parentEnemy;
    private List<Enemy> enemiesHit = new List<Enemy>();

    [SerializeField]
    private PolygonCollider2D polygonCollider;

    [SerializeField]
    private ComboIndicator comboIndicator;

    [SerializeField]
    private float timeout = 1.0f;

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
                // Multiply the damage by the combo mulitplier for the new damage
                damage = baseDamage * comboMultiplier;

                // If the enemy has been hit by the enemy before, or if I'm dead, it still hits but doesn't combo
                if (enemiesHit.Contains(otherEnemy) || parentEnemy.IsDead)
                {
                    comboMultiplier = 1.0f;
                    multiplierIncrease = 0.0f;
                    addedWackedTime = 0.0f;
                }

                // Hit the other enemy, passing on the combo values
                Vector2 contactNormal = (other.transform.position - transform.position).normalized;
                otherEnemy.Hit(
                    contactNormal,
                    baseDamage,
                    damage,
                    multiplierMode,
                    multiplierIncrease,
                    comboMultiplier,
                    force,
                    addedWackedTime
                );

                // Apply a force pulse if combo is 2 or greater
                if (comboMultiplier >= 2.0f)
                {
                    ApplyForcePulse();
                }

                // Add the enemy to the list of enemies hit
                enemiesHit.Add(otherEnemy);
                StartCoroutine(TakeEnemyOffList(otherEnemy));

                // Reset my own combo multiplier values
                comboMultiplier = 1.0f;
                baseDamage = 0.0f;
                damage = 0.0f;
                force = 0.0f;
                addedWackedTime = 0.0f;
                multiplierMode = ComboMultiplierMode.None;
            }
        }
    }

    private IEnumerator TakeEnemyOffList(Enemy enemy)
    {
        // wait for timeout time
        yield return new WaitForSeconds(timeout);
        // remove enemy from list
        enemiesHit.Remove(enemy);
    }

    //TODO: This should be scaled with multiplier, and possibly let the weapon/player powerups have an effect on the pulse
    // POWERUP OPPORTUNITY HERE - Add a powerup that increases the pulse radius and force
    private void ApplyForcePulse()
    {
        // Find all colliders within the pulse radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, pulseRadius);
        foreach (var hitCollider in hitColliders)
        {
            // Check if the collider is an enemy and has a Rigidbody2D
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            Rigidbody2D rb = hitCollider.GetComponent<Rigidbody2D>();
            if (enemy != null && rb != null && enemy != parentEnemy && !enemiesHit.Contains(enemy))
            {
                // Apply force away from the impact point
                Vector2 direction = (
                    hitCollider.transform.position - transform.position
                ).normalized;
                rb.AddForce(direction * pulseForce, ForceMode2D.Impulse);
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

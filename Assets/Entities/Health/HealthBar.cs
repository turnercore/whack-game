using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform foreground;
    public Transform background;
    private float _maxHealth = 1f;
    private float _currentHealth = 1f;
    private Health health; // Reference to the Health script on the parent object

    // Start
    void Start()
    {
        if (foreground == null)
        {
            Debug.LogError("Foreground transform is missing on the HealthBar.");
        }

        if (background == null)
        {
            Debug.LogError("Background transform is missing on the HealthBar.");
        }

        // Initialize to ensure the health bar is correctly scaled at the start
        // Get the parent component's Health script
        health = GetComponentInParent<Health>();
        if (health != null)
        {
            // Subscribe to the OnHit event
            health.OnTakeDamage += UpdateHealthBar;
            // Subscribe to the OnDeath event
            health.OnDeath += UpdateHealthBarDeath;
        }
        else
        {
            Debug.LogWarning("Health component not found on the parent GameObject. Health bar will not update.");
        }
    }

    public float MaxHealth
    {
        get => _maxHealth;
        set
        {
            _maxHealth = value;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }
    }
    public float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            // Clamp the value between 0 and MaxHealth
            _currentHealth = Mathf.Clamp(value, 0, MaxHealth);

            // Calculate health percent
            float healthPercent = _currentHealth / MaxHealth;

            // Update the scale of the foreground
            // foreground.localScale = new Vector3(healthPercent, 1, 1);
            Debug.Log($"Health Percent: {healthPercent}, Scale: {foreground.localScale}");

        }
    }

    public void UpdateHealthBar(float damage)
    {
        if (health != null)
        {
            // Update the health bar with the current health
            CurrentHealth = health.CurrentHealth;
            MaxHealth = health.MaxHealth;

            if (health.IsDead)
            {
                // Disable the health bar if the parent object is dead
                gameObject.SetActive(false);
            }
        }
    }

    public void UpdateHealthBarDeath()
    {
        // Set the health bar to empty when the parent object dies
        CurrentHealth = 0;
    }
}

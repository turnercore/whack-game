using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarSlider : MonoBehaviour
{
    // Slider to represent health
    [SerializeField]
    private UnityEngine.UI.Slider healthSlider;

    // Health component to get health values
    private Health health;

    private void Awake()
    {
        // Ensure the healthSlider is assigned
        if (healthSlider == null)
        {
            healthSlider = GetComponent<UnityEngine.UI.Slider>();
        }

        // Safety check for missing components
        if (healthSlider == null)
        {
            Debug.LogError("Missing required components on " + gameObject.name);
        }
    }

    private void Start()
    {
        // Get the Health component
        health = GameManager.Instance.Player.GetComponent<PlayerController>().health;
    }

    private void Update()
    {
        // Update the health slider value with a slight LERP
        healthSlider.value = Mathf.Lerp(
            healthSlider.value,
            health.CurrentHealth / health.MaxHealth,
            Time.deltaTime * 5
        );
    }
}

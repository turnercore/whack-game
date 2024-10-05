using System.Collections;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform foreground;
    public Transform background;

    [SerializeField]
    private Health health; // Reference to the Health script on the parent object
    private float y;
    private float z;
    private float x;

    [SerializeField]
    private AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [SerializeField]
    private float animationDuration = 0.5f; // How long the animation should last

    void Start()
    {
        // Set the foreground to the max width, height, and z + 1
        // foreground.localScale = new Vector3(background.localScale.x, background.localScale.y, background.localScale.z + 1);

        // Get the initial y and z of the health bar foreground background\
        x = foreground.localScale.x;
        y = foreground.localScale.y;
        z = foreground.localScale.z;

        if (health != null)
        {
            // Subscribe to the OnHit event
            health.OnTakeDamage += UpdateHealthBar;
            // Subscribe to the OnDeath event
            health.OnDeath += UpdateHealthBarDeath;
            // Sbuscribe to the OnHeal event
            health.OnHeal += UpdateHealthBar;
        }
    }

    // Cleanup
    private void OnDestroy()
    {
        // Unsubscribe from the OnHit event
        health.OnTakeDamage -= UpdateHealthBar;
        // Unsubscribe from the OnDeath event
        health.OnDeath -= UpdateHealthBarDeath;
        // Unsubscribe from the OnHeal event
        health.OnHeal -= UpdateHealthBar;
    }

    public void UpdateHealthBar(float amount)
    {
        // Calculate the target width based on the health percentage
        float targetWidth = health.CurrentHealth / health.MaxHealth * y;

        // Stop any running coroutines before starting a new one
        StopAllCoroutines();
        StartCoroutine(AnimateHealthBar(targetWidth));
    }

    private IEnumerator AnimateHealthBar(float targetWidth)
    {
        float startWidth = foreground.localScale.y; // Get the current width
        float elapsed = 0f;

        // Animate over the duration
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            // Evaluate the curve to get the progress
            float t = elapsed / animationDuration;
            float curveValue = animationCurve.Evaluate(t);

            // Lerp between start and target based on curve value
            float newWidth = Mathf.Lerp(startWidth, targetWidth, curveValue);
            foreground.localScale = new Vector3(x, newWidth, z);

            yield return null;
        }

        // Ensure the final scale is set precisely
        foreground.localScale = new Vector3(x, targetWidth, z);
    }

    public void UpdateHealthBarDeath()
    {
        // Set the width of the foreground to 0
        foreground.localScale = Vector3.zero;
    }
}

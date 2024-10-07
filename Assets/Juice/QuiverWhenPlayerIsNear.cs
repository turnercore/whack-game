using UnityEngine;

public class QuiverWhenPlayerIsNearJuice : Juice
{
    public float quiverIntensity = 0.05f; // Intensity of the quiver movement
    public float quiverSpeed = 1000f; // Speed of the quiver movement

    private Vector3 originalPosition;

    void Start()
    {
        // Store the original position of the object to return to it when not quivering
        originalPosition = juiceTarget.transform.localPosition;
    }

    void Update()
    {
        if (isActive)
        {
            // Apply a small random quiver movement to the object's position
            float offsetX = Mathf.Sin(Time.time * quiverSpeed) * quiverIntensity;
            float offsetY = Mathf.Cos(Time.time * quiverSpeed * 0.8f) * quiverIntensity;
            juiceTarget.transform.localPosition =
                originalPosition + new Vector3(offsetX, offsetY, 0f);
        }
        else
        {
            // Reset the position to the original
            juiceTarget.transform.localPosition = originalPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the trigger
        if (other.CompareTag("Player"))
        {
            isActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player has exited the trigger
        if (other.CompareTag("Player"))
        {
            isActive = false;
            // Reset the position to the original
            juiceTarget.transform.localPosition = originalPosition;
        }
    }
}

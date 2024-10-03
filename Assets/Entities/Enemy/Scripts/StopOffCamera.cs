using UnityEngine;

public class StopOffCamera : MonoBehaviour
{
    public float offScreenPercentage = 0.35f;  // Percentage of screen size to be considered off-screen
    private Rigidbody2D rb;
    private bool isOnScreen = true;  // Flag to track if the enemy is currently on screen

    void Start()
    {
        // Get reference to the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckOffScreen();
    }

    void CheckOffScreen()
    {
        // Get the camera's viewport bounds in world coordinates
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        // Calculate how far the enemy can be off-screen (25% beyond the screen size)
        float offScreenBoundaryX = 1f + offScreenPercentage;  // 25% off-screen on the X axis
        float offScreenBoundaryY = 1f + offScreenPercentage;  // 25% off-screen on the Y axis

        // Check if the enemy is beyond the screen boundaries (with the specified percentage)
        bool isBeyondLeft = viewPos.x < -offScreenPercentage;
        bool isBeyondRight = viewPos.x > offScreenBoundaryX;
        bool isBeyondBottom = viewPos.y < -offScreenPercentage;
        bool isBeyondTop = viewPos.y > offScreenBoundaryY;

        // If the enemy is off-screen and was previously on-screen
        if ((isBeyondLeft || isBeyondRight || isBeyondBottom || isBeyondTop) && isOnScreen)
        {
            // Stop the velocity
            rb.velocity = Vector2.zero;

            // Set the flag so this action only happens once
            isOnScreen = false;

            Debug.Log("Enemy went off-screen and stopped.");
        }
        // If the enemy is back on screen, reset the flag
        else if (!isBeyondLeft && !isBeyondRight && !isBeyondBottom && !isBeyondTop && !isOnScreen)
        {
            isOnScreen = true;
            Debug.Log("Enemy came back on-screen.");
        }
    }
}
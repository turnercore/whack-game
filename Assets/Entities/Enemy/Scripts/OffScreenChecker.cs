using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OffScreenChecker : MonoBehaviour
{
    public float checkInterval = 0.25f; // Interval for checking off-screen
    public float offScreenPercentage = 0.35f; // Percentage of screen size to be considered off-screen
    private Rigidbody2D rb;
    private bool isOnScreen = true; // Tracks whether the object is currently on-screen
    public bool hasEnteredScreen = false; // Tracks whether the object has entered the screen ever
    public delegate void OffScreenEvent(bool isOffScreen);
    public event OffScreenEvent OnScreenStatusChanged; // Event fired when the screen status changes

    // Event for the first time the enemy enters the camera
    public delegate void OnFirstEnterScreenEvent();
    public event OnFirstEnterScreenEvent OnFirstEnterScreen;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(CheckOffScreenInterval());
    }

    private IEnumerator CheckOffScreenInterval()
    {
        while (true)
        {
            CheckOffScreen();
            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void CheckOffScreen()
    {
        // Get the camera's viewport bounds in world coordinates
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        // Calculate the boundaries
        float offScreenBoundaryX = 1f + offScreenPercentage;
        float offScreenBoundaryY = 1f + offScreenPercentage;

        // Check if the object is beyond the screen boundaries
        bool isBeyondLeft = viewPos.x < -offScreenPercentage;
        bool isBeyondRight = viewPos.x > offScreenBoundaryX;
        bool isBeyondBottom = viewPos.y < -offScreenPercentage;
        bool isBeyondTop = viewPos.y > offScreenBoundaryY;

        // Determine if the object is currently off-screen
        bool currentlyOffScreen = (isBeyondLeft || isBeyondRight || isBeyondBottom || isBeyondTop);

        // Trigger event if the on-screen status changes
        if (currentlyOffScreen != isOnScreen)
        {
            isOnScreen = !currentlyOffScreen;
            OnScreenStatusChanged?.Invoke(!isOnScreen);
        }

        // If the object is currently on-screen, trigger the first enter screen event
        if (!hasEnteredScreen && !currentlyOffScreen)
        {
            Debug.Log("First Enter Screen");
            hasEnteredScreen = true;
            OnFirstEnterScreen?.Invoke();
        }
    }

    public bool IsOnScreen => isOnScreen; // Expose isOnScreen status
}

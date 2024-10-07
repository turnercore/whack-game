using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    // List of all the screens
    public List<Screen> screens = new();

    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private LevelManager levelManager;

    private Screen currentScreen;
    private Screen transitionScreen;

    // Start is called before the first frame update
    void Start()
    {
        // Loop through all the screens
        foreach (Screen screen in screens)
        {
            // Deactivate the screen
            screen.gameObject.SetActive(false);
        }

        // Register Self with GameManager
        GameManager.Instance.screenManager = this;

        // Activate the first screen
        ZoomTransition(screens[0]);
    }

    public void ZoomTransition(Screen newScreen)
    {
        // Set the transition screen
        transitionScreen = newScreen;
        cameraController.ZoomIn(GameManager.Instance.Player.transform.position);
        EventBus.Instance.OnCameraZoomIn += OnZoomInFinished;
    }

    private void OnZoomInFinished()
    {
        // Change the screen
        ChangeScreen();

        // Snap the camera to the player
        cameraController.SnapToPlayer();

        // Unsubscribe to EventBus Zoomed out event
        EventBus.Instance.OnCameraZoomIn -= OnZoomInFinished;

        // Subscribe to EventBus Zoomed out event
        EventBus.Instance.OnCameraZoomOut += OnZoomOutFinished;

        // Zoom out
        // If the screen is a menu screen, zoom out to the menu position, otherwise zoom out to the player position
        if (currentScreen.isMenuScreen)
            cameraController.ZoomOut(cameraController.menuPosition);
        else
            cameraController.ZoomOut(GameManager.Instance.Player.transform.position);
    }

    private void OnZoomOutFinished()
    {
        // Unsubscribe to EventBus Zoomed out event
        EventBus.Instance.OnCameraZoomOut -= OnZoomOutFinished;
    }

    private void ChangeScreen()
    {
        // Deactivate the current screen
        if (currentScreen != null)
        {
            currentScreen.gameObject.SetActive(false);
        }

        // Activate the new screen
        transitionScreen.gameObject.SetActive(true);

        //Move the player to the new screen's player reference point if it exists
        if (transitionScreen.playerReferencePoint != null)
        {
            GameManager.Instance.Player.transform.position = transitionScreen
                .playerReferencePoint
                .position;
        }

        // Set the new screen as the current screen
        currentScreen = transitionScreen;

        // Set the transition screen to null
        transitionScreen = null;

        // Call the new screen's OnScreenActivated method
    }
}

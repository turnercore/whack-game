using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField]
    private List<Screen> screens = new();

    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private LevelManager levelManager;

    private ScreenType currentScreen = ScreenType.None;
    private Screen CurrentScreenObject => screens[(int)currentScreen - 1];
    private ScreenType transitionScreen = ScreenType.None;
    private Screen TransitionScreenObject => screens[(int)transitionScreen - 1];

    [SerializeField]
    private MenuScreenBarrier menuScreenBarrier;

    void Start()
    {
        // Deactivate all screens at the start
        foreach (Screen screen in screens)
        {
            screen.gameObject.SetActive(false);
        }

        // Register Self with GameManager
        GameManager.Instance.screenManager = this;

        // Activate the first screen
        TransitionToScreen(ScreenType.MainMenu, ScreenTransitionType.ZoomInOut);
    }

    public void TransitionToScreen(
        ScreenType newScreen,
        ScreenTransitionType transitionType = ScreenTransitionType.ZoomInOut
    )
    {
        // If the new screen is the same as the current screen, do nothing
        if (newScreen == currentScreen || newScreen == ScreenType.None)
        {
            return;
        }

        menuScreenBarrier.gameObject.SetActive(false);

        // Set the transition screen
        transitionScreen = newScreen;

        // Transition based on the transition type
        switch (transitionType)
        {
            case ScreenTransitionType.ZoomInOut:
                ZoomInOutTransition(newScreen);
                break;

            case ScreenTransitionType.None:
                InstantTransition(newScreen);
                break;

            default:
                Debug.LogWarning($"Transition type {transitionType} is not implemented.");
                break;
        }
    }

    private void ZoomInOutTransition(ScreenType newScreen)
    {
        // Subscribe to ZoomIn event
        EventBus.Instance.OnCameraZoomIn -= OnZoomInFinished;
        EventBus.Instance.OnCameraZoomIn += OnZoomInFinished;

        // Zoom in to initiate the transition
        if (newScreen == ScreenType.Level)
        {
            // If transitioning to the game level, zoom in on the player's current position
            cameraController.ZoomIn(GameManager.Instance.Player.transform.position);
        }
        else
        {
            // Otherwise, zoom in on the menu position
            cameraController.ZoomIn(cameraController.menuPosition);
        }
    }

    private void InstantTransition(ScreenType newScreen)
    {
        // Immediately change the screen without any transition
        ChangeScreen();
    }

    private void OnZoomInFinished()
    {
        // Change the screen after zooming in
        ChangeScreen();

        // Snap the camera to the player
        cameraController.SnapToPlayer();

        // Unsubscribe from the ZoomIn event
        EventBus.Instance.OnCameraZoomIn -= OnZoomInFinished;

        // Subscribe to the ZoomOut event to handle the next part of the transition
        EventBus.Instance.OnCameraZoomOut -= OnZoomOutFinished;
        EventBus.Instance.OnCameraZoomOut += OnZoomOutFinished;

        // Zoom out
        if (CurrentScreenObject.isMenuScreen)
            cameraController.ZoomOut(cameraController.menuPosition);
        else
            cameraController.ZoomOut(GameManager.Instance.Player.transform.position);
    }

    private void OnZoomOutFinished()
    {
        // Unsubscribe from the ZoomOut event
        EventBus.Instance.OnCameraZoomOut -= OnZoomOutFinished;

        // If the current screen is a menu, activate the menu screen barrier
        if (CurrentScreenObject.isMenuScreen)
            menuScreenBarrier.gameObject.SetActive(true);
    }

    private void ChangeScreen()
    {
        // Deactivate the current screen
        if (currentScreen != ScreenType.None)
        {
            CurrentScreenObject.gameObject.SetActive(false);
        }

        // Activate the new screen
        TransitionScreenObject.gameObject.SetActive(true);

        // Move the player to the new screen's reference point if it exists
        if (TransitionScreenObject.playerReferencePoint != null)
        {
            GameManager.Instance.Player.transform.position = TransitionScreenObject
                .playerReferencePoint
                .position;
        }

        // Update the current screen
        currentScreen = transitionScreen;

        // Reset the transition screen
        transitionScreen = ScreenType.None;

        // Hide the menu screen barrier if transitioning to a non-menu screen
        if (!CurrentScreenObject.isMenuScreen)
        {
            menuScreenBarrier.gameObject.SetActive(false);
        }
    }
}

public enum ScreenType
{
    None,
    MainMenu,
    Credits,
    Pause,
    Level,
}

public enum ScreenTransitionType
{
    None, // Instant screen transition
    ZoomInOut, // Zoom in and zoom out transition
}

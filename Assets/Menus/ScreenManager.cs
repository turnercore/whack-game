using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    // List of all the screens
    [SerializeField]
    private List<Screen> screens = new();

    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private LevelManager levelManager;

    private ScreenType currentScreen = ScreenType.None;
    private Screen currentScreenObject => screens[(int)currentScreen - 1];
    private ScreenType transitionScreen = ScreenType.None;
    private Screen transitionScreenObject => screens[(int)transitionScreen - 1];

    [SerializeField]
    private MenuScreenBarrier menuScreenBarrier;

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
        ZoomTransition(ScreenType.MainMenu);
    }

    public void ZoomTransition(ScreenType newScreen)
    {
        // Set the transition screen
        menuScreenBarrier.gameObject.SetActive(false);
        transitionScreen = newScreen;
        cameraController.ZoomIn(GameManager.Instance.Player.transform.position);
        EventBus.Instance.OnCameraZoomIn += OnZoomInFinished;
    }

    public void LoadScreen(ScreenType screenType)
    {
        // Set the transition screen
        transitionScreen = screenType;
        cameraController.ZoomIn(cameraController.menuPosition);
        EventBus.Instance.OnCameraZoomIn += OnZoomInFinished;
    }

    private void StartGame()
    {
        // Change to no screen aka the levelmanager takes over
        TransitionToScreen(ScreenType.Level);
    }

    private void UnloadScreens()
    {
        foreach (Screen screen in screens)
        {
            screen.gameObject.SetActive(false);
        }
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
        if (currentScreenObject.isMenuScreen)
            cameraController.ZoomOut(cameraController.menuPosition);
        else
            cameraController.ZoomOut(GameManager.Instance.Player.transform.position);
    }

    private void OnZoomOutFinished()
    {
        // Unsubscribe to EventBus Zoomed out event
        EventBus.Instance.OnCameraZoomOut -= OnZoomOutFinished;
        if (currentScreenObject.isMenuScreen)
            menuScreenBarrier.gameObject.SetActive(true);
    }

    private void ChangeScreen()
    {
        if (currentScreen == transitionScreen || transitionScreen == ScreenType.None)
        {
            return;
        }

        if (currentScreen == ScreenType.None)
        {
            currentScreen = ScreenType.MainMenu;
        }

        // Deactivate the current screen
        if (currentScreen != ScreenType.None)
        {
            currentScreenObject.gameObject.SetActive(false);
        }

        // Activate the new screen
        transitionScreenObject.gameObject.SetActive(true);

        //Move the player to the new screen's player reference point if it exists
        if (transitionScreenObject.playerReferencePoint != null)
        {
            GameManager.Instance.Player.transform.position = transitionScreenObject
                .playerReferencePoint
                .position;
        }

        // Set the new screen as the current screen
        currentScreen = transitionScreen;

        // Set the transition screen to null
        transitionScreen = ScreenType.None;

        if (!currentScreenObject.isMenuScreen)
        {
            menuScreenBarrier.gameObject.SetActive(false);
        }
    }

    public void TransitionToScreen(ScreenType screen)
    {
        transitionScreen = screen;
        ZoomTransition(screen);
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

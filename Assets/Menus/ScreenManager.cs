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
        ZoomTransition(screens[0]);
    }

    public void ZoomTransition(Screen newScreen)
    {
        // Set the transition screen
        transitionScreen = newScreen;
        cameraController.ZoomIn(GameManager.Instance.Player.transform.position);
        EventBus.Instance.OnCameraZoomIn += OnZoomInFinished;
    }

    public void LoadScreen(ButtonTypes buttonType)
    {
        Screen newScreen = null;
        switch (buttonType)
        {
            case ButtonTypes.Play:
                StartGame();
                break;
            case ButtonTypes.MainMenu:
                TransitionToScreen(screens[(int)ScreenType.MainMenu]);
                break;
            case ButtonTypes.Options:
                break;
            case ButtonTypes.Credits:
                break;
            case ButtonTypes.Pause:
                break;
        }

        if (newScreen != null)
        {
            ZoomTransition(newScreen);
        }
    }

    private void StartGame()
    {
        // Change to no screen
        currentScreen = null;
        UnloadScreens();
        levelManager.gameObject.SetActive(true);
        levelManager.enabled = true;
        menuScreenBarrier.gameObject.SetActive(false);
        menuScreenBarrier.enabled = true;
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
        if (currentScreen.isMenuScreen)
            cameraController.ZoomOut(cameraController.menuPosition);
        else
            cameraController.ZoomOut(GameManager.Instance.Player.transform.position);
    }

    private void OnZoomOutFinished()
    {
        // Unsubscribe to EventBus Zoomed out event
        EventBus.Instance.OnCameraZoomOut -= OnZoomOutFinished;
        menuScreenBarrier.UpdateEdgeCollider();
    }

    private void ChangeScreen()
    {
        if (transitionScreen == null)
        {
            return;
        }

        // Deactivate the current screen
        if (currentScreen != null)
        {
            currentScreen.gameObject.SetActive(false);
        }

        // Activate the new screen
        transitionScreen.gameObject.SetActive(true);

        //Move the player to the new screen's player reference point if it exists
        if (transitionScreen != null && transitionScreen.playerReferencePoint != null)
        {
            GameManager.Instance.Player.transform.position = transitionScreen
                .playerReferencePoint
                .position;
        }

        // Set the new screen as the current screen
        currentScreen = transitionScreen;

        // Set the transition screen to null
        transitionScreen = null;

        // If Screen is a menu screen, show the menu screen barrier
        if (currentScreen.isMenuScreen)
        {
            menuScreenBarrier.gameObject.SetActive(true);
            levelManager.enabled = false;
        }
        else
        {
            menuScreenBarrier.gameObject.SetActive(false);
        }
    }

    public void TransitionToScreen(Screen screen)
    {
        transitionScreen = screen;
        ZoomTransition(screen);
    }
}

public enum ScreenType
{
    MainMenu,
    Credits,
    Pause,
}

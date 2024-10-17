using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseOverlay; // Reference to your Pause UI

    [SerializeField]
    private Image vhsPauseEffect; // Reference to your VHS pause effect Image
    private bool isPaused = false;

    private void Start()
    {
        // Ensure the pauseOverlay and VHS effect are disabled initially
        pauseOverlay.SetActive(false);
        if (vhsPauseEffect != null)
        {
            vhsPauseEffect.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        // Subscribe to the pause action
        InputSystem.onActionChange += OnActionChange;
    }

    private void OnDisable()
    {
        // Unsubscribe from the pause action
        InputSystem.onActionChange -= OnActionChange;
    }

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (
            obj is InputAction action
            && action.name == "Pause"
            && change == InputActionChange.ActionPerformed
        )
        {
            OnPause(new InputAction.CallbackContext());
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        Debug.Log("Pause action performed");
        // Toggle pause state
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        // Activate the pause UI, VHS effect, and stop time
        pauseOverlay.SetActive(true);
        if (vhsPauseEffect != null)
        {
            vhsPauseEffect.gameObject.SetActive(true);
            // Optionally, add some animation or effect to mimic VHS static
        }
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void ResumeGame()
    {
        // Deactivate the pause UI, VHS effect, and resume time
        pauseOverlay.SetActive(false);
        if (vhsPauseEffect != null)
        {
            vhsPauseEffect.gameObject.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;
    }
}

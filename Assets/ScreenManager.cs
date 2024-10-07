using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    // List of all the screens
    public List<Screen> screens = new List<Screen>();

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
    }
}

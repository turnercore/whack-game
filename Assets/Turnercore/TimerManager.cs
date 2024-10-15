using UnityEngine;

// Singleton TimerManager to handle coroutines
public class TimerManager : MonoBehaviour
{
    // Singleton code
    private static TimerManager _instance;
    public static TimerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject timerManagerObject = new("TimerManager");
                _instance = timerManagerObject.AddComponent<TimerManager>();
                DontDestroyOnLoad(timerManagerObject);
            }
            return _instance;
        }
        private set { _instance = value; }
    }

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Detach from any parent object
        transform.SetParent(null);
        // If no other instance exists, make this the singleton instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager objects
        }
    }

    public void RemoveTimer(Timercore.Timer timer)
    {
        Timercore.RemoveTimer(timer);
    }
}

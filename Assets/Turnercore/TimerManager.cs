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

    public void RemoveTimer(Timercore.Timer timer)
    {
        Timercore.RemoveTimer(timer);
    }

    public void StopAllTimers()
    {
        Timercore.StopAllTimers();
        Timercore.RemoveAllTimers();
    }
}

using UnityEngine;

// Singleton TimerManager to handle coroutines
public class TimerManager : MonoBehaviour
{
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
    }

    public void RemoveTimer(Timercore.Timer timer)
    {
        Timercore.RemoveTimer(timer);
    }
}

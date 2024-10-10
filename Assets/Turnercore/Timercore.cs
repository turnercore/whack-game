using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Timercore
{
    public class Timer
    {
        public float length;
        public float elapsedTime;
        public bool isRepeating;
        public bool isRunning;
        public bool isPaused;
        public bool isReusable;
        public bool runsOnGamePaused;

        public event Action OnComplete;
        public event Action OnStart;
        public event Action OnPause;
        public event Action OnUnpause;
        public event Action OnStop;
        public event Action OnReset;
        public event Action OnUpdate;
        public event Action OnDestroy;

        public Coroutine TimerCoroutine;

        public void Start()
        {
            elapsedTime = 0;
            isRunning = true;
            InvokeOnStart();

            // Start the timer coroutine through TimerManager
            TimerCoroutine = TimerManager.Instance.StartCoroutine(RunTimer());
        }

        public void Stop()
        {
            if (TimerCoroutine != null)
            {
                TimerManager.Instance.StopCoroutine(TimerCoroutine);
                TimerCoroutine = null;
            }
            isRunning = false;
            InvokeOnStop();
        }

        public void Pause()
        {
            isPaused = true;
            isRunning = false;
            InvokeOnPause();
        }

        public void Unpause()
        {
            isPaused = false;
            isRunning = true;
            InvokeOnUnpause();
        }

        public void Reset()
        {
            elapsedTime = 0;
            isRunning = false;
            InvokeOnReset();
            UnsubscribeAll();
        }

        public void UnsubscribeAll()
        {
            OnComplete = null;
            OnStart = null;
            OnPause = null;
            OnUnpause = null;
            OnStop = null;
            OnReset = null;
            OnUpdate = null;
            OnDestroy = null;
        }

        private IEnumerator RunTimer()
        {
            while (isRunning)
            {
                if (!isPaused)
                {
                    elapsedTime += Time.deltaTime;
                    InvokeOnUpdate();

                    if (elapsedTime >= length)
                    {
                        InvokeOnComplete();

                        if (isRepeating)
                        {
                            elapsedTime = 0;
                        }
                        else
                        {
                            Stop();
                            if (!isReusable)
                            {
                                InvokeOnDestroy();
                                TimerManager.Instance.RemoveTimer(this);
                            }
                        }
                    }
                }

                yield return null;
            }
        }

        #region Invoke events

        public void InvokeOnComplete() => OnComplete?.Invoke();

        public void InvokeOnStart() => OnStart?.Invoke();

        public void InvokeOnPause() => OnPause?.Invoke();

        public void InvokeOnUnpause() => OnUnpause?.Invoke();

        public void InvokeOnStop() => OnStop?.Invoke();

        public void InvokeOnReset() => OnReset?.Invoke();

        public void InvokeOnUpdate() => OnUpdate?.Invoke();

        public void InvokeOnDestroy() => OnDestroy?.Invoke();

        #endregion
    }

    private static Dictionary<string, Timer> timers = new();

    public static TimerBuilder CreateTimer(string timerName = "")
    {
        return new TimerBuilder(timerName);
    }

    public class TimerBuilder
    {
        private Timer timer;
        private string timerName;

        public TimerBuilder(string name)
        {
            timerName = string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString() : name;
            timer = new Timer();
        }

        public TimerBuilder SetLength(float length)
        {
            timer.length = length;
            return this;
        }

        public TimerBuilder SetRepeating(bool repeat)
        {
            timer.isRepeating = repeat;
            return this;
        }

        public TimerBuilder OnComplete(Action callback)
        {
            timer.OnComplete += callback;
            return this;
        }

        public TimerBuilder OnStart(Action callback)
        {
            timer.OnStart += callback;
            return this;
        }

        public TimerBuilder OnPause(Action callback)
        {
            timer.OnPause += callback;
            return this;
        }

        public TimerBuilder OnUnpause(Action callback)
        {
            timer.OnUnpause += callback;
            return this;
        }

        public TimerBuilder OnStop(Action callback)
        {
            timer.OnStop += callback;
            return this;
        }

        public TimerBuilder OnReset(Action callback)
        {
            timer.OnReset += callback;
            return this;
        }

        public TimerBuilder OnUpdate(Action callback)
        {
            timer.OnUpdate += callback;
            return this;
        }

        public TimerBuilder RunOnGamePaused(bool runOnGamePaused)
        {
            timer.runsOnGamePaused = runOnGamePaused;
            return this;
        }

        public string Start()
        {
            if (!timers.ContainsKey(timerName))
            {
                timers.Add(timerName, timer);
                timer.Start();
            }
            return timerName;
        }
    }

    public static void UnsubscribeCallback(string timerName, Action callback)
    {
        if (timers.ContainsKey(timerName))
        {
            Timer timer = timers[timerName];
            timer.OnComplete -= callback;
            timer.OnStart -= callback;
            timer.OnPause -= callback;
            timer.OnUnpause -= callback;
            timer.OnStop -= callback;
            timer.OnReset -= callback;
            timer.OnUpdate -= callback;
        }
    }

    public static void RemoveTimer(Timer timer)
    {
        string key = null;
        foreach (var kvp in timers)
        {
            if (kvp.Value == timer)
            {
                key = kvp.Key;
                break;
            }
        }
        if (key != null)
        {
            timers.Remove(key);
        }
    }
}

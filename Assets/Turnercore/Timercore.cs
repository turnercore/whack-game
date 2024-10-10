// using System;
// using System.Collections.Generic;
// using UnityEngine;

// public static class Timercore {
//     private class Timer {
//         public float length;
//         public float startTime;
//         public float elapsedTime;
//         public bool isRepeating;
//         public bool isRunning;
//         public bool isPaused;
//         public bool runsOnGamePaused;

//         public event Action onComplete;
//         public event Action onStart;
//         public event Action onPause;
//         public event Action onUnpause;
//         public event Action onStop;
//         public event Action onReset;
//         public event Action onUpdate;



//         public void Start() {
//             startTime = Time.time;
//             isRunning = true;
//             onStart?.Invoke();
//         }

//         public void Stop() {
//             isRunning = false;
//             onStop?.Invoke();
//         }

//         public void Reset() {
//             startTime = 0;
//             elapsedTime = 0;
//             isRunning = false;
//             onReset?.Invoke();
//             UnsubscribeAll();
//         }

//         public void Pause() {
//             isPaused = true;
//             isRunning = false;
//             onPause?.Invoke();
//         }

//         public void Unpause() {
//             isPaused = false;
//             isRunning = true;
//             onUnpause?.Invoke();
//         }

//         public void UnsubscribeAll() {
//             onComplete = null;
//             onStart = null;
//             onPause = null;
//             onUnpause = null;
//             onStop = null;
//             onReset = null;
//             onUpdate = null;
//         }
//     }

//     private static Dictionary<string, Timer> timers = new();

//     public static TimerBuilder CreateTimer(string timerName = "") {
//         return new TimerBuilder(timerName);
//     }

//     public class TimerBuilder {
//         private Timer timer;
//         private string timerName;

//         public TimerBuilder(string name) {
//             timerName = string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString() : name;
//             timer = new Timer();
//         }

//         public TimerBuilder SetLength(float length) {
//             timer.length = length;
//             return this;
//         }

//         public TimerBuilder SetRepeating(bool repeat) {
//             timer.isRepeating = repeat;
//             return this;
//         }

//         public TimerBuilder OnComplete(Action callback) {
//             timer.onComplete += callback;
//             return this;
//         }

//         public TimerBuilder OnStart(Action callback) {
//             timer.onStart += callback;
//             return this;
//         }

//         public TimerBuilder OnPause(Action callback) {
//             timer.onPause += callback;
//             return this;
//         }

//         public TimerBuilder OnUnpause(Action callback) {
//             timer.onUnpause += callback;
//             return this;
//         }

//         public TimerBuilder OnStop(Action callback) {
//             timer.onStop += callback;
//             return this;
//         }

//         public TimerBuilder OnReset(Action callback) {
//             timer.onReset += callback;
//             return this;
//         }

//         public TimerBuilder OnUpdate(Action callback) {
//             timer.onUpdate += callback;
//             return this;
//         }

//         public TimerBuilder Reset() {
//             timer.Reset();
//             return this;
//         }

//         public TimerBuilder Pause() {
//             timer.Pause();
//             return this;
//         }

//         public TimerBuilder Unpause() {
//             timer.Unpause();
//             return this;
//         }

//         public TimerBuilder Stop() {
//             timer.Stop();
//             return this;
//         }

//         public TimerBuilder UnsubscribeAll() {
//             timer.UnsubscribeAll();
//             return this;
//         }

//         public TimerBuilder RunOnGamePaused(bool runOnGamePaused) {
//             timer.runsOnGamePaused = runOnGamePaused;
//             return this;
//         }

//         public string Start() {
//             if (!timers.ContainsKey(timerName)) {
//                 timer.Start();
//                 timers.Add(timerName, timer);
//             }
//             return timerName;
//         }
//     }

//     public static void UnsubscribeCallback(string timerName, Action callback) {
//         if (timers.ContainsKey(timerName)) {
//             Timer timer = timers[timerName];
//             // Manually remove the specific callback from the appropriate event
//             timer.onComplete -= callback;
//             timer.onStart -= callback;
//             timer.onPause -= callback;
//             timer.onUnpause -= callback;
//             timer.onStop -= callback;
//             timer.onReset -= callback;
//             timer.onUpdate -= callback;
//         }
//     }

//   public static void Update(bool isGamePaused = false) {
//       foreach (KeyValuePair<string, Timer> timer in timers) {
//           // Check if the timer is running
//           if (timer.Value.isRunning) {
//               // If the game is paused, only update timers that should run when paused
//               if (isGamePaused && !timer.Value.runsOnGamePaused) {
//                   continue;
//               }

//               // Update the elapsed time
//               timer.Value.elapsedTime = Time.time - timer.Value.startTime;
//               timer.Value.onUpdate?.Invoke();

//               // Check if the timer has reached its length
//               if (timer.Value.elapsedTime >= timer.Value.length) {
//                   timer.Value.onComplete?.Invoke();

//                   if (timer.Value.isRepeating) {
//                       timer.Value.Start();
//                   } else {
//                       timer.Value.Stop();
//                   }
//               }
//           }
//       }
//   }
// }

using System;
using System.Collections.Generic;
using UnityEngine;

namespace JTUtility
{
    public class Timer : IDisposable
    {
        private class InnerTImer : MonoBehaviour
        {
            public List<int> keys = new List<int>();
            public Dictionary<int, WeakReference<Timer>> timers = new Dictionary<int, WeakReference<Timer>>();

            private void Update()
            {
                WeakReference<Timer> refTimer;
                Timer timer;

                for (int i = 0; i < keys.Count; i++)
                {
                    var k = keys[i];
                    if (!timers.TryGetValue(k, out refTimer))
                    {
                        keys.Remove(k);
                        i--;
                    }
                    else if (!refTimer.TryGetTarget(out timer))
                    {
                        keys.Remove(k);
                        timers.Remove(k);
                        i--;
                    }
                    else if (timer.disposing)
                    {
                        keys.Remove(k);
                        timers.Remove(k);
                        i--;
                    }
                }

                foreach (var k in keys)
                {
                    timers[k].TryGetTarget(out timer);
                    if (timer == null || timer.timeLeft <= 0) continue;

                    if (timer.RealTime)
                        timer.timeLeft -= Time.unscaledDeltaTime;
                    else if (timer.CustomDeltaTimeSource != null)
                    {
                        timer.timeLeft -= timer.CustomDeltaTimeSource();
                    }
                    else
                    {
                        timer.timeLeft -= Time.deltaTime;
                    }
                    timer.OnProgress?.Invoke(timer);

                    if (timer.timeLeft > 0) continue;

                    if (timer.Repeat)
                    {
                        timer.timeLeft += timer.startTime;
                    }
                    else
                    {
                        timer.timeLeft = 0;
                    }

                    if (timer.OnTimeOut != null)
                    {
                        timer.OnTimeOut.Invoke(timer);
                    }

                    if (timer.raiseTimeOut != null)
                    {
                        timer.raiseTimeOut.Invoke(timer);
                        timer.raiseTimeOut = null;
                    }
                }
            }
        }

        private static InnerTImer innerTimer;

        private static InnerTImer InnerTimer
        {
            get
            {
                if (innerTimer != null)
                    return innerTimer;

                innerTimer = GlobalObject.Instance.GetOrAddComponent<InnerTImer>();
                return innerTimer;
            }
        }

        private int timerID;
        private bool disposing = false;

        private float startTime;
        private float timeLeft;

        public event Action<Timer> OnTimeOut;

        public event Action<Timer> OnProgress;

        private Action<Timer> raiseTimeOut;

        public bool Repeat { get; set; }
        public bool RealTime { get; set; }
        public Func<float> CustomDeltaTimeSource { get; set; }

        public Timer()
        {
            timerID = GetHashCode();
            InnerTimer.keys.Add(timerID);
            InnerTimer.timers.Add(timerID, new WeakReference<Timer>(this));
        }

        public bool IsReachedTime()
        {
            return timeLeft <= 0;
        }

        public float PassedTime
        {
            get { return startTime - timeLeft; }
        }

        public float LeftTime
        {
            get { return timeLeft; }
        }

        public float PassedPercentage
        {
            get { return timeLeft / startTime; }
        }

        public void Start(float sec)
        {
            startTime = sec;
            timeLeft = sec;
        }

        public void Start(float sec, Action<Timer> timeOutCallback)
        {
            startTime = sec;
            timeLeft = sec;
            raiseTimeOut = timeOutCallback;
        }

        public void Abort()
        {
            timeLeft = -1;
        }

        public void Dispose()
        {
            disposing = true;
        }
    }
}
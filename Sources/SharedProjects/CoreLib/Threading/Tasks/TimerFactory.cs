using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mohammad.Helpers;
using static Mohammad.Helpers.CodeHelper;

namespace Mohammad.Threading.Tasks
{
    partial class Timer
    {
        public static class Factory
        {
            private static readonly List<Timer> _Timers = new List<Timer>();

            public static Timer SetTimer<TArg>(TimeSpan interval, Action<TArg> action, TArg tag, bool startImmediately = true, int? tickCount = null)
            {
                return SetTimer(interval, t => action(t.Tag.To<TArg>()), tag, startImmediately, tickCount);
            }

            public static Timer SetTimer(TimeSpan interval, Action action, bool startImmediately = true, int? tickCount = null)
            {
                return SetTimer(interval, (Timer t) => action(), null, startImmediately, tickCount);
            }

            public static Timer SetTimer(TimeSpan interval, Action<Timer> action, object tag = null, bool startImmediately = true, int? tickCount = null)
            {
                _Timers.CleanNulls();
                var newTimer = new Timer((_Timers.Any() ? _Timers.Max(t => t.Id) : 0) + 1, tickCount) {Tag = tag, Interval = interval};
                var timer = new System.Threading.Timer(delegate(object state)
                    {
                        var t = state.As<Timer>();
                        if (t.IsStopRequested)
                            return;
                        if (tickCount.HasValue)
                        {
                            if (t.TickIndex == tickCount.Value)
                            {
                                Stop(t);
                                return;
                            }
                            t.TickIndex = (t.TickIndex ?? 0) + 1;
                        }
                        t.SignalTime = DateTime.Now;
                        action(t);
                    },
                    newTimer,
                    startImmediately ? TimeSpan.FromMilliseconds(0) : TimeSpan.FromMilliseconds(-1),
                    interval);

                newTimer.InnerTimer = timer;
                _Timers.Add(newTimer);

                return newTimer;
            }

            public static void Start(int id)
            {
                var timer = _Timers.FirstOrDefault(t => t.Id == id);
                if (timer == null)
                    return;
                Start(timer);
            }

            public static void Pause(int id)
            {
                var timer = _Timers.FirstOrDefault(t => t.Id == id);
                if (timer == null)
                    return;
                Pause(timer);
            }

            public static void Stop(int id)
            {
                var timer = _Timers.FirstOrDefault(t => t.Id == id);
                if (timer == null)
                    return;
                Stop(timer);
            }

            private static void Start(Timer timer) { timer.InnerTimer.Change(TimeSpan.FromMilliseconds(0), timer.Interval); }
            private static void Pause(Timer timer) { timer.InnerTimer.Change(TimeSpan.FromMilliseconds(-1), timer.Interval); }

            public static void Stop(Timer timer)
            {
                if (timer == null)
                    return;
                LockAndCatch(() =>
                    {
                        timer.IsStopRequested = true;
                        timer.Working?.Set();
                        if (_Timers.Contains(timer))
                            _Timers.Remove(timer);
                        timer.InnerTimer?.Change(TimeSpan.FromMilliseconds(-1), timer.Interval);
                        timer.InnerTimer?.Dispose();
                        GC.Collect();
                    },
                    timer);
            }

            public static void WaitForAllToStop() { Parallel.ForEach(_Timers.Copy(), t => t?.WaitForStop()); }

            public static void StopAll() { Parallel.ForEach(_Timers.Copy(), Stop); }

            public static Timer GetTimerById(int id) => _Timers.FirstOrDefault(t => t.Id == id);

            public static IEnumerable<Timer> GetAll() => _Timers.Select(t => t);
        }
    }
}
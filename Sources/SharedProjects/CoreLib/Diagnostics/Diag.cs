using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Mohammad.Helpers;
using Mohammad.Logging;

namespace Mohammad.Diagnostics
{
    public static class Diag
    {
        private static ILogger _Out;

        public static ILogger Out
        {
            get
            {
                if (_Out != null)
                    return _Out;
                _Out = new Logger("ConsoleHelper.Out");
                //_Out.Logged += (_, e) => { DebugWriteLine(e.Log, CodeHelper.GetCallerMethodName(3)); };
#if DEBUG
                _Out.IsDebugModeEnabled = true;
#endif
                return _Out;
            }
        }

        public static IEnumerable<ExceptionHandlingClause> GetActiveTryClauses(Type type)
        {
            var stackTrace = new StackTrace();
            var frames = stackTrace.GetFrames();

            if (frames == null)
                yield break;
            for (var i = 1; i < frames.Length; i++)
            {
                var frame = frames[i];
                var method = frame.GetMethod();
                var body = method.GetMethodBody();

                if (body == null)
                    continue;
                var clauses =
                    body.ExceptionHandlingClauses.Where(clause => clause.Flags == ExceptionHandlingClauseOptions.Clause)
                        .Select(clause => new {clause, offsetInFrame = frame.GetILOffset()})
                        .Select(t => new {t, tryStartOffset = t.clause.TryOffset})
                        .Select(t => new {t, tryEndOffset = t.tryStartOffset + t.t.clause.TryLength})
                        .Where(t => t.t.t.offsetInFrame >= t.t.tryStartOffset && t.t.t.offsetInFrame < t.tryEndOffset)
                        .Where(t => t.t.t.clause.CatchType != null)
                        .Where(t => t.t.t.clause.CatchType != null && (type == null || t.t.t.clause.CatchType.IsAssignableFrom(type)))
                        .Select(t => t.t.t.clause);
                foreach (var clause in clauses)
                    yield return clause;
            }
        }

        public static MethodBase ScrollupToSubclassOf(Type type) => ScrollupTo(t => t.IsSubclassOf(type));
        public static MethodBase ScrollupTo(Type type) { return ScrollupTo(t => t == type); }

        public static MethodBase ScrollupTo(Predicate<Type> isTarget)
        {
            var st = new StackTrace(true);
            for (var i = 0; i < st.FrameCount; i++)
            {
                var frame = st.GetFrame(i);
                var method = frame.GetMethod();
                var t = method.DeclaringType;
                if (t == null || !isTarget(t))
                    continue;
                return method;
            }
            return null;
        }

        public static bool IsCaught(Type type) => EnumerableHelper.Any(GetActiveTryClauses(type));

        public static Stopwatch Stopwatch(Action action)
        {
            var result = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                action();
            }
            finally
            {
                result.Stop();
            }
            return result;
        }

        public static Stopwatch Stopwatch(Delegate action, params object[] args)
        {
            var result = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                action.DynamicInvoke(args);
            }
            finally
            {
                result.Stop();
            }
            return result;
        }

        public static Stopwatch Stopwatch(Delegate action, out object result, params object[] args)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                result = action.DynamicInvoke(args);
            }
            finally
            {
                stopwatch.Stop();
            }
            return stopwatch;
        }

        public static Stopwatch Stopwatch<TResult>(Func<TResult> action, out TResult result)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                result = action();
            }
            finally
            {
                stopwatch.Stop();
            }
            return stopwatch;
        }

        public static Stopwatch Stopwatch<T1>(Action<T1> action, T1 arg)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                action(arg);
            }
            finally
            {
                stopwatch.Stop();
            }
            return stopwatch;
        }

        public static Stopwatch Stopwatch<T1, TResult>(Func<T1, TResult> action, T1 arg1, out TResult result)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                result = action(arg1);
            }
            finally
            {
                stopwatch.Stop();
            }
            return stopwatch;
        }

        public static bool? TestExpect<TResult>(Func<TResult> func, TResult result, Action onSucced = null, Action onFailure = null,
            Action<Exception> onException = null, string actionName = null)
            => TestExpect(func, r => result == null && r == null || result != null && result.Equals(r), r => onSucced?.Invoke(), onFailure, onException, actionName);

        public static bool? TestExpect(Action action, Action onSucced = null, Action onFailure = null, Action<Exception> onException = null, string actionName = null)
        {
            return TestExpect<object>(() =>
                {
                    action();
                    return null;
                },
                onSucced: r => onSucced?.Invoke(),
                onFailure: onFailure,
                onException: onException,
                actionName: actionName);
        }

        public static bool? TestExpect<TResult>(Func<TResult> action, Func<TResult, bool> resultPredict = null, Action<TResult> onSucced = null,
            Action onFailure = null, Action<Exception> onException = null, string actionName = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            Func<TResult, bool> True = r =>
            {
                DebugWriteLine("succeed.", actionName.IfNullOrEmpty("Action"));
                onSucced?.Invoke(r);
                return true;
            };
            Func<bool> False = () =>
            {
                DebugWriteLine("failed.", actionName.IfNullOrEmpty("Action"));
                onFailure?.Invoke();
                return false;
            };
            try
            {
                var r = RunDebug(action, actionName);
                if (resultPredict == null)
                    return True(r);
                return resultPredict(r) ? True(r) : False();
            }
            catch (Exception ex)
            {
                DebugWriteLine($"has exception: {ex.GetBaseException().Message}", actionName.IfNullOrEmpty("Action"));
                onException?.Invoke(ex);
                return null;
            }
        }

        public static bool? TestExpect(Delegate action, Func<object, bool> resultPredict = null, Action<object> onSucced = null, Action onFailure = null,
            Action<Exception> onException = null, string actionName = null, params object[] args)
        {
            return TestExpect(() => action.DynamicInvoke(args), resultPredict, onSucced, onFailure, onException, actionName);
        }

        public static object RunDebug(Delegate action, string actionName = null, params object[] args)
        {
            return RunDebug(() => action.DynamicInvoke(args), actionName.IfNullOrEmpty(action.Method.Name));
        }

        public static void RunDebug(Action action, string actionName = null, bool showStartingPrompts = true)
        {
            RunDebug<object>(() =>
                {
                    action();
                    return null;
                },
                actionName,
                showStartingPrompts);
        }

        public static TResult RunDebug<TResult>(Func<TResult> action, string actionName = null, bool showStartingPrompts = true)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (actionName.IsNullOrEmpty())
                actionName = action.Method.Name;
            TResult r;
            if (showStartingPrompts)
                DebugWriteLine("is starting", actionName.IfNullOrEmpty("Action"));
            var stopwatch = Stopwatch(action, out r);
            DebugWriteLine($"is done and took {stopwatch.Elapsed}", actionName.IfNullOrEmpty("Action"));
            return r;
        }

        public static void DebugWriteLine(object log, object sender = null, string format = null)
        {
            Trace.WriteLine(Logger.FormatLogText(log, sender: sender ?? CodeHelper.GetCallerMethodName(3), logTextFormat: format));
        }

        public static TResult RunDebug<TArg1, TResult>(Func<TArg1, TResult> action, TArg1 arg1, string actionName = null)
            => (TResult) RunDebug(action, actionName, arg1);

        public static TResult RunDebug<TArg1, TArg2, TResult>(Func<TArg1, TArg2, TResult> action, TArg1 arg1, TArg2 arg2, string actionName = null)
            => (TResult) RunDebug(action, actionName, arg1, arg2);

        public static void RedirectDebugsToOutputPane(bool redirect) { RedirectDebugsToListeren<DefaultTraceListener>(redirect); }

        public static void RedirectDebugsToConsole(bool redirect) { RedirectDebugsToListeren(redirect ? new TextWriterTraceListener(Console.Out) : null, redirect); }

        public static void RedirectDebugsToListeren<TListenerType>(bool redirect) where TListenerType : TraceListener, new()
        {
            RedirectDebugsToListeren(redirect ? new TListenerType() : null, redirect);
        }

        public static void RedirectDebugsToListeren<TListenerType>(TListenerType listener, bool redirect) where TListenerType : TraceListener
        {
            if (redirect)
            {
                if (Trace.Listeners.Cast<TraceListener>().All(ts => ts.GetType() != typeof(TListenerType)))
                    Trace.Listeners.Add(listener);
            }
            else
            {
                if (Trace.Listeners.Cast<TraceListener>().Any(ts => ts.GetType() == typeof(TListenerType)))
                    Trace.Listeners.Remove(Trace.Listeners.Cast<TraceListener>().First(ts => ts.GetType() == typeof(TListenerType)));
            }
        }
    }
}
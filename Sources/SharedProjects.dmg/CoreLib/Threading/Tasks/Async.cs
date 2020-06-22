using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mohammad.Collections.Specialized;
using Mohammad.Helpers;
using static Mohammad.Helpers.CodeHelper;

#pragma warning disable 4014

namespace Mohammad.Threading.Tasks
{
    public static class Async
    {
        public static void RunAndWait(params Action[] actions) => Task.WaitAll(actions.Select(action => Task.Factory.StartNew(action)).ToArray());

        public static bool RunAndWait(TimeSpan timeout, params Action[] actions)
            => Task.WaitAll(actions.Select(action => Task.Factory.StartNew(action)).ToArray(), timeout);

        public static void WaitAll(params Task[] tasks) => Task.WaitAll(tasks);
        public static void WaitAll(IEnumerable<Task> tasks) => Task.WaitAll(tasks.ToArray());
        public static void WaitAny(params Task[] tasks) => Task.WaitAny(tasks);

        public static void WaitAll(params Action[] actions)
        {
            var tasks = new TaskList();
            tasks.AddRange(actions.Select(action => Run(action)));
            tasks.WaitAll();
        }

        public static bool WaitAll(TimeSpan timeout, params Action[] actions)
        {
            var tasks = new TaskList();
            tasks.AddRange(actions.ForEachFunc(action => Run(action)));
            return tasks.WaitAll(timeout);
        }

        public static async Task Run(Action action, CancellationToken token = default(CancellationToken), TaskScheduler scheduler = null, Action onEnded = null,
            string actionName = "")
        {
            if (!actionName.IsNullOrEmpty())
                LibrarySupervisor.Logger.Debug($"{actionName} running...");
            if (scheduler != null)
                await Task.Factory.StartNew(action, token.Equals(default(CancellationToken)) ? CancellationToken.None : token, TaskCreationOptions.None, scheduler);
            else
                await Task.Factory.StartNew(action, token.Equals(default(CancellationToken)) ? CancellationToken.None : token);
            onEnded?.Invoke();
            if (!actionName.IsNullOrEmpty())
                LibrarySupervisor.Logger.Debug($"{actionName} done.");
        }

        public static async Task<TResult> Run<TResult>(Func<TResult> action, CancellationToken token = default(CancellationToken), TaskScheduler scheduler = null,
            Action<TResult> onDone = null, string actionName = "")
        {
            var result = default(TResult);
            await Run(() => { result = action(); }, token, scheduler, () => onDone?.Invoke(result));
            return result;
        }

        public static async Task<bool> Run(Action action, TimeSpan timeout, bool cancelOnTimeout = true, Action onDone = null, Action onNotDone = null)
            => await Run(t => action(), timeout, cancelOnTimeout, onDone, onNotDone);

        public static async Task<bool> Run(Action<Task> action, TimeSpan timeout, bool cancelOnTimeout = true, Action onDone = null, Action onNotDone = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            using (var source = new CancellationTokenSource())
            {
                var watcher = Task.Delay(timeout, source.Token);
                var task = new Task(() => { }, source.Token);
                task = task.ContinueWith(action, source.Token);
                task.Start();
                await Task.WhenAny(watcher, task);
                if (task.IsCompleted)
                {
                    onDone?.Invoke();
                }
                else
                {
                    if (cancelOnTimeout)
                        source.Cancel();
                    onNotDone?.Invoke();
                }
                if (task.Exception != null)
                    throw task.Exception;
                return task.IsCompleted;
            }
        }

        public static async Task<TResult> Run<TResult>(Func<TResult> action, TimeSpan timeout, bool cancelOnTimeout = true, Action onTimeout = null,
            TResult defaultResult = default(TResult), Action<TResult> onEnd = null, Action onNotEnded = null)
        {
            var result = defaultResult;
            if (await Run(_ => { result = action(); }, timeout, cancelOnTimeout, () => onEnd?.Invoke(result), onNotEnded))
                return result;
            return defaultResult;
        }

        public static async Task<bool> Run<T>(Action<T> action, T arg, TimeSpan timeout, bool cancelOnTimeout = true, Action onDone = null, Action onNotDone = null)
        {
            return await Run(_ => action(arg), timeout, cancelOnTimeout, onDone, onNotDone);
        }

        public static async Task Run(params Action[] actions) => await Task.Run(() => actions.AsEnumerable());

        public static async Task Run(IEnumerable<Action> actions)
        {
            foreach (var action in actions)
                await Task.Factory.StartNew(action);
        }

        public static async Task<RunCancellableResult<TResult>> RunCancellable<TResult>(Func<TResult> action, CancellationTokenSource cancellationTokenSource)
        {
            var result = new RunCancellableResult<TResult> {CancellationTokenSource = cancellationTokenSource};
            var isDoneWaitHandle = new ManualResetEvent(false);
            try
            {
                var t = new Thread(() =>
                {
                    LibrarySupervisor.Logger.Debug("Action started");
                    result.Result = action();
                    LibrarySupervisor.Logger.Debug("Action done");
                    result.IsDone = true;
                    isDoneWaitHandle.Set();
                });
                t.Start();
                await Run(() => { WaitHandle.WaitAny(new[] {cancellationTokenSource.Token.WaitHandle, isDoneWaitHandle}); });
                if (result.IsDone)
                    return result;
                Catch(t.Abort);
                LibrarySupervisor.Logger.Debug("Action cancelled");
                return result;
            }
            finally
            {
                Catch(() => isDoneWaitHandle.Dispose());
            }
        }

        public static async Task<RunCancellableResult<TResult>> RunCancellable<TResult>(Func<TResult> action, TimeSpan? timeout = null)
        {
            using (var cancellationTokenSource = timeout.HasValue ? new CancellationTokenSource(timeout.Value) : new CancellationTokenSource())
                return await RunCancellable(action, cancellationTokenSource);
        }

        public static async Task<RunCancellableResult> RunCancellable(Action action, CancellationTokenSource cancellationTokenSource)
        {
            return await RunCancellable(() =>
                {
                    action();
                    return true;
                },
                cancellationTokenSource);
        }

        public static async Task<RunCancellableResult> RunCancellable(Action action, TimeSpan? timeout = null)
        {
            return await RunCancellable(() =>
                {
                    action();
                    return true;
                },
                timeout);
        }

        public static CancellationTokenSource RunCancellationTokenSource(Action action, TimeSpan? timeout = null)
        {
            var cancellationTokenSource = timeout.HasValue ? new CancellationTokenSource(timeout.Value) : new CancellationTokenSource();
            RunCancellable(action, cancellationTokenSource);
            return cancellationTokenSource;
        }
    }
}
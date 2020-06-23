using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.Diagnostics;
using Mohammad.Exceptions;
using Mohammad.Threading.Tasks;

namespace Mohammad.Helpers
{
    public static class CodeHelper
    {
        public static readonly Action EmptyDelegate = () => { };

        public static void Break() => throw new BreakException();

        public static Exception? Catch(Action tryMethod, Action<Exception> catchMethod = null, Action finallyMethod = null, ExceptionHandling handling = null, bool throwException = false)
        {
            if (tryMethod == null)
            {
                throw new ArgumentNullException(nameof(tryMethod));
            }

            handling?.Reset();
            try
            {
                tryMethod();
                return null;
            }
            catch (Exception ex)
            {
                catchMethod?.Invoke(ex);
                handling?.HandleException(ex);
                if (throwException)
                {
                    throw;
                }

                return ex;
            }
            finally
            {
                finallyMethod?.Invoke();
            }
        }

        public static TResult CatchFunc<TResult>(Func<TResult> tryAction,
            Func<Exception, TResult> catchAction,
            out Exception exception,
            Action finallyAction = null,
            ExceptionHandling handling = null,
            bool throwException = false)
        {
            if (tryAction == null)
            {
                throw new ArgumentNullException(nameof(tryAction));
            }

            if (catchAction == null)
            {
                throw new ArgumentNullException(nameof(catchAction));
            }

            handling?.Reset();
            exception = null;
            try
            {
                return tryAction();
            }
            catch (Exception ex)
            {
                var result = catchAction(ex);
                handling?.HandleException(ex);
                if (throwException)
                {
                    throw;
                }

                exception = ex;
                return result;
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }

        public static TResult CatchFunc<TResult>(Func<TResult> tryAction,
            Func<Exception, TResult> catchAction,
            Action finallyAction = null,
            ExceptionHandling handling = null,
            bool throwException = false) =>
            CatchFunc(tryAction, catchAction, out var _, finallyAction, handling, throwException);

        public static TResult CatchFunc<TResult>(Func<TResult> tryAction,
            TResult defaultResult = default,
            Action finallyAction = null,
            ExceptionHandling handling = null,
            bool throwException = false) =>
            CatchFunc(tryAction, ex => defaultResult, out var _, finallyAction, handling, throwException);

        public static TResult CatchFunc<TResult>(Func<TResult> tryAction,
            out Exception exception,
            TResult defaultResult = default,
            Action finallyAction = null,
            ExceptionHandling handling = null,
            bool throwException = false)
        {
            var result = CatchFunc(tryAction, ex => defaultResult, out var buffer, finallyAction, handling, throwException);
            exception = buffer;
            return result;
        }

        public static Exception CatchSpecExc<TException>(Action action, Action<TException> catchMethod = null, Action finallyMethod = null)
            where TException : Exception
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            Exception exception;
            try
            {
                action();
                exception = null;
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(TException))
                {
                    catchMethod?.Invoke(ex.As<TException>());
                    exception = ex;
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                finallyMethod?.Invoke();
            }

            return exception;
        }

        public static void Compute<TResult>(Func<TResult> func) => func();

        public static void Dispose<TDisposable>(Func<TDisposable> creator, Action<TDisposable> action)
            where TDisposable : IDisposable
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }

            creator().Dispose(action);
        }

        public static TResult Dispose<TDisposable, TResult>(Func<TDisposable> creator, Func<TDisposable, TResult> action)
            where TDisposable : IDisposable => creator().Dispose(action);

        public static void Dispose<TDisposable>(Action<TDisposable> action)
            where TDisposable : IDisposable, new()
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using var disposable = new TDisposable();
            action(disposable);
        }

        public static TResult Dispose<TDisposable, TResult>(Func<TDisposable, TResult> action)
            where TDisposable : IDisposable, new()
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using var disposable = new TDisposable();
            return action(disposable);
        }

        public static void Do(IEnumerable<Action> actions, Action<int> next = null) => Do(actions,
            index =>
            {
                next?.Invoke(index);
                return true;
            });

        public static void Do(IEnumerable<Action> actions, Func<int, bool> next)
        {
            if (actions == null)
            {
                throw new ArgumentNullException(nameof(actions));
            }

            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            actions.Enumerate((action, index) =>
            {
                action();
                return next(index);
            });
        }

        public static void DoFor(Action<int> action, int count)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            for (var i = 0; i < count; i++)
            {
                action(i);
            }
        }

        public static void DoFor(Action action, int count, Action<int> onGoNext = null)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            for (var i = 0; i < count; i++)
            {
                action();
                onGoNext?.Invoke(i);
            }
        }

        public static void DoForAsParallel(Action action, int count)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            for (var i = 0; i < count; i++)
            {
                _ = Async.Run(action);
            }
        }

        public static async void DoForAsync(Action action, int count)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            for (var i = 0; i < count; i++)
            {
                await Async.Run(action);
            }
        }

        public static void DoWhile(Action action, Func<bool> predicate)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            do
            {
                action();
            } while (predicate());
        }

        public static void DoWhile(Func<bool> predicate) => DoWhile(() =>
            {
            },
            predicate);

        public static IEnumerable<TResult> DoWhile<TResult>(Func<TResult> action, Func<bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            do
            {
                yield return action();
            } while (predicate());
        }

        public static void ExecOnDebugger(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (Debugger.IsAttached)
            {
                action();
            }
        }

        public static MethodBase GetCallerMethod(int index = 2) => new StackTrace(true).GetFrame(index)?.GetMethod();

        public static MethodBase GetCallerMethod(int index, bool parsePrevIfNull)
        {
            var stackTrace = new StackTrace(true);
            if (stackTrace.GetFrame(index) != null || !parsePrevIfNull)
            {
                return stackTrace.GetFrame(index)?.GetMethod();
            }

            while (stackTrace.GetFrame(--index) == null)
            {
            }

            return stackTrace.GetFrame(index)?.GetMethod();
        }

        public static string GetCallerMethodName(int index = 2) => GetCallerMethod(index).Name;

        public static MethodBase GetCurrentMethod() => GetCallerMethod();

        public static TDelegate GetDelegate<TType, TDelegate>(string methodName) =>
            (TDelegate)(ISerializable)Delegate.CreateDelegate(typeof(TDelegate), typeof(TType).GetMethod(methodName) ?? throw new InvalidOperationException());

        public static IEnumerable<Action> GetRepeat(Action del, int count)
        {
            for (var index = 0; index < count; index++)
            {
                yield return del;
            }
        }

        public static bool HasException(Action tryFunc) => Catch(tryFunc) != null;

        public static void If(this bool condition, Action ifTrue = null, Action ifFalse = null)
        {
            if (condition)
            {
                ifTrue?.Invoke();
            }
            else
            {
                ifFalse?.Invoke();
            }
        }

        public static void If<T>(this T obj, Func<T, bool> condition, Action<T> ifTrue = null, Action<T> ifFalse = null)
        {
            if (condition(obj))
            {
                ifTrue?.Invoke(obj);
            }
            else
            {
                ifFalse?.Invoke(obj);
            }
        }

        public static TResult If<T, TResult>(this T obj, Func<T, bool> condition, Func<T, TResult> ifTrue, Func<T, TResult> ifFalse)
            => condition(obj) ? ifTrue(obj) : ifFalse(obj);

        public static TResult If<T, TResult>(this T obj, Func<T, bool> condition, TResult ifTrue, TResult ifFalse) => condition(obj) ? ifTrue : ifFalse;

        public static void IfFalse(this bool condition, Action ifFalse)
        {
            if (!condition)
            {
                ifFalse?.Invoke();
            }
        }

        public static void IfTrue(this bool condition, Action ifTrue)
        {
            if (condition)
            {
                ifTrue?.Invoke();
            }
        }

        /// <summary>
        ///     Invokes the specified target.
        /// </summary>
        /// <typeparam name="TType">The type of the target type.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static object Invoke<TType>(TType target, string methodName, params object[] parameters)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                if (parameters != null && parameters.Length > 0)
                {
                    var types = new Type[parameters.Length];
                    for (var counter = 0; counter != parameters.Length; counter++)
                    {
                        types[counter] = parameters.GetType();
                    }
                }

                var constructors = typeof(TType).GetConstructors();
                return constructors.Where(constructor => parameters != null)
                    .Where(constructor => parameters != null && constructor.GetParameters().GetLength(0) == parameters.Length)
                    .Select(constructor => constructor.Invoke(parameters)).FirstOrDefault();
            }

            var methods = typeof(TType).GetMethods();
            return methods.Where(method => string.Compare(method.Name, methodName, StringComparison.Ordinal) == 0)
                .Where(method => method.GetParameters().GetLength(0) == parameters.Length)
                .Select(method => method.Invoke(target, parameters)).FirstOrDefault();
        }

        /// <summary>
        ///     Determines whether [is design time].
        /// </summary>
        /// <returns>
        ///     <c>true</c> if [is design time]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDesignTime()
        {
            var result = true;
#if CORELIB
            return result = false;
#endif
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
#pragma warning disable 162
            return result;
#pragma warning restore 162
        }

        public static void Lock(Action action, object lockObject = null)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            Lock(() =>
                {
                    action();
                    return true;
                },
                lockObject);
        }

        public static TResult Lock<TResult>(Func<TResult> action, object lockObject = null)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // ReSharper disable once PossibleNullReferenceException
            lock (lockObject ?? GetCallerMethod().DeclaringType)
            {
                return action();
            }
        }

        public static void LockAndCatch(Action tryMethod,
            object lockObject = null,
            Action<Exception> catchMethod = null,
            Action finallyMethod = null,
            ExceptionHandling handling = null,
            bool throwException = false)
            => Lock(() => Catch(tryMethod, catchMethod, finallyMethod, handling, throwException), lockObject ?? GetCallerMethod().DeclaringType);

        public static (bool IsSucceed, int RetryCount) Retry(Func<bool> func, int retryCount, TimeSpan waitForRetry = default)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            var index = 0;
            for (; index < retryCount; index++)
            {
                if (func())
                {
                    return (true, index);
                }

                if (waitForRetry != default)
                {
                    Thread.Sleep(waitForRetry);
                }
            }

            return (false, index);
        }

        public static (bool IsSucceed, int RetryCount) Retry(Func<int, bool> func, int retryCount, TimeSpan waitForRetry = default)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            var index = 0;
            for (; index < retryCount; index++)
            {
                if (func(index))
                {
                    return (true, index);
                }

                if (waitForRetry != default)
                {
                    Thread.Sleep(waitForRetry);
                }
            }

            return (false, index);
        }

        public static (bool IsSucceed, int RetryCount, TResult Result) Retry<TResult>(Func<(bool isOk, TResult result)> func,
            int retryCount,
            TimeSpan waitForRetry = default)
        {
            var index = 0;
            for (; index < retryCount; index++)
            {
                var oper = func();
                if (oper.isOk)
                {
                    return (true, index, oper.result);
                }

                if (waitForRetry != default)
                {
                    Thread.Sleep(waitForRetry);
                }
            }

            return (false, index, default);
        }

        public static void RunAndCleanupMemory(Action action, object owner = null, Action disposer = null)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var mp = MemoryProfiler.StartNew();
            try
            {
                action();
            }
            finally
            {
                if (disposer != null)
                {
                    Catch(disposer);
                }

                mp.Stop();
                mp.Cleanup(owner);
            }
        }

        public static TResult RunAndCleanupMemory<TResult>(Func<TResult> action, object owner = null, Action disposer = null)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var mp = MemoryProfiler.StartNew();
            try
            {
                return action();
            }
            finally
            {
                if (disposer != null)
                {
                    Catch(disposer);
                }

                mp.Stop();
                mp.Cleanup(owner);
            }
        }

        //public static T Wait<T>(this T t, double timeout) => Wait(t, TimeSpan.FromMilliseconds(timeout));

        //public static T Wait<T>(this T t, TimeSpan timeout)
        //{
        //    Thread.Sleep(timeout);
        //    return t;
        //}

        public static void While(Func<bool> predicate, Action action = null)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            while (predicate())
            {
                action?.Invoke();
            }
        }

        public static IEnumerable<TResult> While<TResult>(Func<bool> predicate, Func<TResult> action, Action onIterationDone = null)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            while (predicate())
            {
                yield return action();
            }

            onIterationDone?.Invoke();
        }

        public static async Task<TResult> DisposeAsync<TDisposable, TResult>(Func<TDisposable, Task<TResult>> func)
            where TDisposable : IDisposable, new()
        {
            using var disposable = new TDisposable();
            return await func(disposable);
        }

        public static async Task<TResult> DisposeAsync<TDisposable, TResult>(Func<TDisposable> getDisposable, Func<TDisposable, Task<TResult>> func)
            where TDisposable : IDisposable
        {
            using var disposable = getDisposable();
            return await func(disposable);
        }

        public static async Task<TResult> DisposeAsync<TDisposable, TResult>(this TDisposable disposable, Func<TDisposable, Task<TResult>> func)
            where TDisposable : IDisposable
        {
            using (disposable)
            {
                return await func(disposable);
            }
        }
    }
}
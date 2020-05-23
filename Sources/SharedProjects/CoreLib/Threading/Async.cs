using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.EventsArgs;
using Mohammad.Helpers;

namespace Mohammad.Threading
{
    [Obsolete("Use async/await instead.")]
    public abstract class Async : IEquatable<Async>, IExceptionHandlerContainer, IDisposable
    {
        private CancellationTokenSource _CancellationTokenSource;
        private Lazy<ExceptionHandling> _ExceptionHandling;
        private string _Name;
        private AsyncPool _Pool;
        protected Task Core { get; private set; }

        public string Name
        {
            get { return this._Name; }
            set
            {
                if (this.Core != null)
                    throw new Exception("Cannot change thread's name while running.");
                this._Name = value;
            }
        }

        public List<object> Parameteres { get; private set; }

        public AsyncPool Pool
        {
            get { return this._Pool; }
            protected set
            {
                if (this._Pool == value)
                    return;
                this._Pool?.Unregister(this);
                this._Pool = value;
                this._Pool?.Register(this);
            }
        }

        public AsyncStatus Status { get; protected set; }
        public object Tag { get; set; }
        public object Group { get; set; }
        public TaskScheduler MainTaskScheduler { get; protected set; }

        public CancellationTokenSource CancellationTokenSource
        {
            get { return this._CancellationTokenSource ?? (this._CancellationTokenSource = new CancellationTokenSource()); }
            set
            {
                if (this.Core != null)
                    throw new Exception("Cannot change thread's CancellationToken while running.");
                this._CancellationTokenSource = value;
            }
        }

        protected Async()
        {
            this._ExceptionHandling = new Lazy<ExceptionHandling>(() =>
            {
                ExceptionHandling result = null;
                if (this.Pool != null)
                    result = this.Pool.ExceptionHandling;
                return result ?? new ExceptionHandling();
            });
        }

        protected Async(IEnumerable<object> parameteres)
            : this()
        {
            this.Parameteres = parameteres.ToList();
        }

        protected Async(AsyncPool pool)
            : this()
        {
            this.Pool = pool;
        }

        public event EventHandler Ended;
        public event EventHandler<ActingEventArgs> Starting;

        public void Abort()
        {
            this.Status = AsyncStatus.Ended;
            if (this.Core != null)
                this.CancellationTokenSource.Cancel();
        }

        protected virtual void AsyncStart()
        {
            if (MustStopIt())
                return;
            if (this.OnStarting())
                try
                {
                    this.Execute();
                }
                catch (Exception ex)
                {
                    this.ExceptionHandling.HandleException(ex);
                }
            this.OnEnded();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return ReferenceEquals(this, obj) || obj.GetType() == typeof(Async) && this.Equals((Async) obj);
        }

        protected abstract void Execute();

        public static Async GetAsyncInstance<T>(Action<T> method, T arg1, string name = null, AsyncPool pool = null, object group = null, object tag = null)
        {
            return GetAsyncInstance(method, EnumerableHelper.AsEnuemrable(arg1), name, pool, group, tag);
        }

        public static Async GetAsyncInstance(Action method, string name = null, AsyncPool pool = null, object group = null, object tag = null)
        {
            return GetAsyncInstance(method, null, name, pool, group, tag);
        }

        public static Async GetAsyncInstance(Delegate methodInfo, IEnumerable args = null, string name = null, AsyncPool pool = null, object group = null,
            object tag = null)
        {
            var result = new AsyncImp(methodInfo, pool, args) {Name = name, Group = group, Tag = tag};
            return result;
        }

        public override int GetHashCode() { return this.Core?.GetHashCode() ?? 0; }

        public void Wait() { this.Core?.Wait(); }

        protected virtual void OnEnded()
        {
            this.Core = null;
            GC.Collect();
            this.Status = AsyncStatus.Ended;
            this.Ended.RaiseAsync(this, this.MainTaskScheduler, this.Core);
        }

        protected virtual bool OnStarting()
        {
            var e = new ActingEventArgs();
            this.Starting.RaiseAsync(this, e, this.Core, this.MainTaskScheduler);
            this.Status = e.Handled ? AsyncStatus.Ended : AsyncStatus.Running;
            return !e.Handled;
        }

        public static bool operator ==(Async left, Async right) { return Equals(left, right); }
        public static bool operator !=(Async left, Async right) { return !Equals(left, right); }
        public static void Sleep(int millisecondsTimeout) { Thread.Sleep(millisecondsTimeout); }
        public static void Sleep(TimeSpan timeout) { Thread.Sleep(timeout); }

        public void Start()
        {
            if (this.Pool != null)
                this.Pool.Start();
            else
                this.InnerStart();
        }

        internal void InnerStart()
        {
            if (MustStopIt())
                return;
            this.Name = this.Name.IfNullOrEmpty(Guid.NewGuid().ToString());
            if (this.CancellationTokenSource == null)
                this.CancellationTokenSource = new CancellationTokenSource();
            this.Core = new Task(this.AsyncStart, this.CancellationTokenSource.Token, TaskCreationOptions.LongRunning);
            this.Core.Start();
            this.MainTaskScheduler = TaskScheduler.Current;
        }

        public override string ToString() { return this.Name; }

        public static Async Run<T1, T2>(Action<T1, T2> method, T1 arg1, T2 arg2, string name = null, AsyncPool pool = null, object group = null, object tag = null)
        {
            return Run(method, EnumerableHelper.AsEnuemrable(arg1, arg2), name, pool, group, tag);
        }

        public static Async Run<T1>(Action<T1> method, T1 arg1, string name = null, AsyncPool pool = null, object group = null, object tag = null)
        {
            return Run(method, EnumerableHelper.AsEnuemrable(arg1), name, pool, group, tag);
        }

        public static Async Run(Action method, string name = null, AsyncPool pool = null, object group = null, object tag = null)
        {
            return Run(method, null, name, pool, group, tag);
        }

        public static Async Run(Delegate methodInfo, IEnumerable args = null, string name = null, AsyncPool pool = null, object group = null, object tag = null)
        {
            var instance = GetAsyncInstance(methodInfo, args, name, pool, group, tag);
            instance.Start();
            return instance;
        }

        public static bool MustStopIt() { return !EnumHelper.IsEnumInRange(Thread.CurrentThread.ThreadState, ThreadState.Running, ThreadState.Background); }

        public static void ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> actor, int threadCount = 2, bool join = true, Action ended = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));
            if (threadCount < 0)
                throw new ArgumentOutOfRangeException(nameof(threadCount), threadCount, "must be positive and integer.");

            if (source.Count() == threadCount)
            {
                var asyncs1 = source.Select(item => Run(actor, item)).ToList();
                foreach (var async1 in asyncs1)
                    async1.Wait();
                return;
            }

            var index = 0;
            var list = source.ToList(); //Performance
            var groups = new List<List<TSource>>(); // List of source items' groups
            //Init' list of source items' groups
            for (var i = 0; i < threadCount; i++)
            {
                groups.Add(new List<TSource>());
                var session = index;
                while (session < index + list.Count / threadCount)
                {
                    groups[i].Add(list[session]);
                    session++;
                }
                index = session;
            }
            var asyncs = new List<Async>();
            //var remainingThreads = 0;
            var remainingThreads = threadCount;
            using (var semaphore = new Semaphore(0, 1))
            {
                for (var j = 0; j < groups.Count; j++)
                {
                    var groups1 = groups;
                    var asyncInstance = GetAsyncInstance(l => groups1[l].ForEach(actor), j);

                    //asyncInstance.Starting += ((sender, e) => remainingThreads++);
                    asyncInstance.Ended += delegate
                    {
                        remainingThreads--;
                        if (remainingThreads > 0)
                            return;
                        ended?.Invoke();
                        if (join)
                            semaphore.Release();
                    };
                    asyncs.Add(asyncInstance);
                }
                asyncs.ForEach(async1 => async1.Start());
                if (join)
                    semaphore.WaitOne();
            }
            groups = null;
            GC.Collect();
        }

        public static void For(int start, int count, Action<int> actor, int threadCount = 2) { ForEach(Enumerable.Range(start, count), actor, threadCount); }

        public void Dispose()
        {
            this.CancellationTokenSource?.Dispose();
            if (this.Core != null)
                CodeHelper.Catch(this.Core.Dispose);
        }

        public bool Equals(Async other)
        {
            if (ReferenceEquals(null, other))
                return false;
            return ReferenceEquals(this, other) || Equals(other.Name, this.Name);
        }

        public ExceptionHandling ExceptionHandling
        {
            get { return this._ExceptionHandling.Value; }
            set { this._ExceptionHandling = new Lazy<ExceptionHandling>(() => value); }
        }
    }
}
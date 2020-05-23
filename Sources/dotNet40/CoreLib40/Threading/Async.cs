#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library40.Bcl;
using Library40.EventsArgs;
using Library40.ExceptionHandlingPattern;
using Library40.Helpers;

namespace Library40.Threading
{
	public abstract class Async : IEquatable<Async>, IExceptionHandlerContainer, IDisposable
	{
		private readonly LazyInit<ExceptionHandling> _ExceptionHandling;
		private CancellationTokenSource _CancellationTokenSource;
		private string _Name;
		private AsyncPool _Pool;

		protected Async()
		{
			this._ExceptionHandling = new LazyInit<ExceptionHandling>(() =>
			                                                          {
				                                                          ExceptionHandling result = null;
				                                                          if (this.Pool != null)
					                                                          result = this.Pool.ExceptionHandling;
				                                                          return result ?? (new ExceptionHandling());
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
				if (this._Pool != null)
					this._Pool.Unregister(this);
				this._Pool = value;
				if (this._Pool != null)
					this._Pool.Register(this);
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

		#region IDisposable Members
		public void Dispose()
		{
			if (this.CancellationTokenSource != null)
				this.CancellationTokenSource.Dispose();
			if (this.Core != null)
				MethodHelper.Catch(this.Core.Dispose);
		}
		#endregion

		#region IEquatable<Async> Members
		public bool Equals(Async other)
		{
			if (ReferenceEquals(null, other))
				return false;
			return (ReferenceEquals(this, other) || Equals(other.Name, this.Name));
		}
		#endregion

		#region IExceptionHandlerContainer Members
		public ExceptionHandling ExceptionHandling
		{
			get { return this._ExceptionHandling; }
			set { this._ExceptionHandling.Value = value; }
		}
		#endregion

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
			return (ReferenceEquals(this, obj) || ((obj.GetType() == typeof (Async)) && this.Equals((Async)obj)));
		}

		protected abstract void Execute();

		public static Async GetAsyncInstance<T>(Action<T> method, T arg1, string name = null, AsyncPool pool = null, Object group = null, Object tag = null)
		{
			return GetAsyncInstance(method, EnumerableHelper.AsEnuemrable(arg1), name, pool, group, tag);
		}

		public static Async GetAsyncInstance(Action method, string name = null, AsyncPool pool = null, Object group = null, Object tag = null)
		{
			return GetAsyncInstance(method, null, name, pool, group, tag);
		}

		public static Async GetAsyncInstance(Delegate methodInfo, IEnumerable args = null, string name = null, AsyncPool pool = null, Object group = null, Object tag = null)
		{
			var result = new AsyncImp(methodInfo, pool, args)
			             {
				             Name = name,
				             Group = group,
				             Tag = tag
			             };
			return result;
		}

		public override int GetHashCode()
		{
			return ((this.Core != null) ? this.Core.GetHashCode() : 0);
		}

		public void Wait()
		{
			if (this.Core != null)
				this.Core.Wait();
		}

		protected virtual void OnEnded()
		{
			this.Core = null;
			GC.Collect();
			this.Status = AsyncStatus.Ended;
			this.Ended.Raise(this, this.MainTaskScheduler, this.Core);
		}

		protected virtual bool OnStarting()
		{
			var e = new ActingEventArgs();
			this.Starting.Raise(this, e, this.MainTaskScheduler, this.Core);
			this.Status = e.Handled ? AsyncStatus.Ended : AsyncStatus.Running;
			return !e.Handled;
		}

		public static bool operator ==(Async left, Async right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Async left, Async right)
		{
			return !Equals(left, right);
		}

		public static void Sleep(int millisecondsTimeout)
		{
			Thread.Sleep(millisecondsTimeout);
		}

		public static void Sleep(TimeSpan timeout)
		{
			Thread.Sleep(timeout);
		}

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
			this.Name = this.Name.IsNull(Guid.NewGuid().ToString());
			if (this.CancellationTokenSource == null)
				this.CancellationTokenSource = new CancellationTokenSource();
			this.Core = new Task(this.AsyncStart, this.CancellationTokenSource.Token, TaskCreationOptions.LongRunning);
			this.Core.Start();
			this.MainTaskScheduler = TaskScheduler.Current;
		}

		public override string ToString()
		{
			return this.Name;
		}

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

		public static bool MustStopIt()
		{
			return !Thread.CurrentThread.ThreadState.IsEnumInRange(ThreadState.Running, ThreadState.Background);
		}

		public static void ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> actor, int threadCount = 2, bool join = true, Action ended = null)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (actor == null)
				throw new ArgumentNullException("actor");
			if (threadCount < 0)
				throw new ArgumentOutOfRangeException("threadCount", threadCount, "must be positive and integer.");

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
				while (session < (index + (list.Count / threadCount)))
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
					var asyncInstance = GetAsyncInstance(l => groups[l].ForEach(actor), j);

					//asyncInstance.Starting += ((sender, e) => remainingThreads++);
					asyncInstance.Ended += delegate
					                       {
						                       remainingThreads--;
						                       if (remainingThreads > 0)
							                       return;
						                       if (ended != null)
							                       ended();
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

		public static void For(int start, int count, Action<int> actor, int threadCount = 2)
		{
			ForEach(Enumerable.Range(start, count), actor, threadCount);
		}
	}
}
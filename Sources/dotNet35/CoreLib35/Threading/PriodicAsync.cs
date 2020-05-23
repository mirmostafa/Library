#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Threading;
using Library35.EventsArgs;
using Library35.Helpers;

namespace Library35.Threading
{
	public abstract class PriodicAsync : Async
	{
		#region Events

		#region PriodEnded
		public event EventHandler PriodEnded;
		#endregion

		#region PriodStarting
		public event EventHandler<ActingEventArgs> PriodStarting;
		#endregion

		#endregion

		#region Properties

		#region MaxCount
		public long? MaxCount { get; set; }
		#endregion

		#region Count
		public long Count { get; protected set; }
		#endregion

		#region Interval
		public virtual TimeSpan Interval { get; set; }
		#endregion

		#region ContinueOnError
		public bool ContinueOnError { get; set; }
		#endregion

		#region PriodPool
		private AsyncPool _PriodPool;

		public AsyncPool PriodPool
		{
			get
			{
				if (this._PriodPool == null)
					this._PriodPool = this.Pool;
				return this._PriodPool;
			}
			set { this._PriodPool = value; }
		}
		#endregion

		#endregion

		#region Methods

		#region .Ctor
		protected PriodicAsync(TimeSpan interval)
		{
			this.Interval = interval;
		}
		#endregion

		#region .Ctor
		protected PriodicAsync(TimeSpan interval, long maxCount)
		{
			this.Interval = interval;
			this.MaxCount = maxCount;
		}
		#endregion

		#region AsyncStart
		protected override void AsyncStart()
		{
			try
			{
				if (this.OnStarting())
					while (this.Status == AsyncStatus.Running)
					{
						try
						{
							try
							{
								this.Count++;
								if (this.OnPriodStarting())
									this.Execute();
							}
							catch (Exception ex)
							{
								this.ExceptionHandling.HandleException(ex);
								if (!this.ContinueOnError)
									return;
							}
						}
						finally
						{
							this.OnPriodEnded();
						}
						if (this.MaxCount.HasValue && this.MaxCount.Value <= this.Count)
							break;
						Thread.Sleep(this.Interval);
					}
			}
			finally
			{
				this.OnEnded();
			}
		}
		#endregion

		#region OnEnded
		protected override void OnEnded()
		{
			this.Status = AsyncStatus.Ended;
			base.OnEnded();
		}
		#endregion

		#region OnPriodEnded
		protected virtual void OnPriodEnded()
		{
			this.PriodEnded.Raise(this);
		}
		#endregion

		#region OnPriodStarting
		protected virtual bool OnPriodStarting()
		{
			var e = new ActingEventArgs();
			this.PriodStarting.Raise(this, e);
			return !e.Handled;
		}
		#endregion

		#region OnStarting
		protected override bool OnStarting()
		{
			var result = base.OnStarting();
			if (result)
				this.Status = AsyncStatus.Running;
			return result;
		}
		#endregion

		#endregion

		public static PriodicAsync GetPriodicAsyncInstance(Delegate methodInfo, TimeSpan interval, long maxCount)
		{
			var result = new PriodicAsyncImp(methodInfo, interval, maxCount);
			return result;
		}

		public static PriodicAsync GetPriodicAsyncInstance(Delegate methodInfo, TimeSpan interval)
		{
			var result = new PriodicAsyncImp(methodInfo, interval);
			return result;
		}
	}
}
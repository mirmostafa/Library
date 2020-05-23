#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Threading;
using Library40.EventsArgs;
using Library40.Helpers;

namespace Library40.Threading
{
	public abstract class PriodicAsync : Async
	{
		private AsyncPool _PriodPool;

		protected PriodicAsync(TimeSpan interval)
		{
			this.Interval = interval;
		}

		protected PriodicAsync(TimeSpan interval, long maxCount)
		{
			this.Interval = interval;
			this.MaxCount = maxCount;
		}

		public long? MaxCount { get; set; }

		public long Count { get; protected set; }

		public virtual TimeSpan Interval { get; set; }

		public bool ContinueOnError { get; set; }

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

		public event EventHandler PriodEnded;

		public event EventHandler<ActingEventArgs> PriodStarting;

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

		protected override void OnEnded()
		{
			this.Status = AsyncStatus.Ended;
			base.OnEnded();
		}

		protected virtual void OnPriodEnded()
		{
			this.PriodEnded.Raise(this);
		}

		protected virtual bool OnPriodStarting()
		{
			var e = new ActingEventArgs();
			this.PriodStarting.Raise(this, e);
			return !e.Handled;
		}

		protected override bool OnStarting()
		{
			var result = base.OnStarting();
			if (result)
				this.Status = AsyncStatus.Running;
			return result;
		}

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
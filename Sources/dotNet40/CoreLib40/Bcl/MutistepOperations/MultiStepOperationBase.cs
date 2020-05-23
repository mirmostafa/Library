#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Library40.EventsArgs;
using Library40.ExceptionHandlingPattern;
using Library40.Helpers;

namespace Library40.Bcl.MutistepOperations
{
	/// <summary>
	/// </summary>
	public abstract class MultiStepOperation : IDisposable, IExceptionHandlerContainer
	{
		private readonly CancellationTokenSource _CancellationTokenSource;
		private bool _Disposed;
		private ExceptionHandling _ExceptionHandling;

		protected MultiStepOperation()
		{
			this._CancellationTokenSource = new CancellationTokenSource();
		}

		/// <summary>
		///     Gets the main synchronization context.
		/// </summary>
		/// <value> The main synchronization context. </value>
		protected TaskScheduler MainSynchronizationContext { get; set; }

		/// <summary>
		///     Gets or sets the name of current instance
		/// </summary>
		/// <value> The name. </value>
		public string Name { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether [continue on exception].
		/// </summary>
		/// <value>
		///     <c>true</c> if [continue on exception]; otherwise, <c>false</c> .
		/// </value>
		public bool ContinueOnException { get; set; }

		protected Task Task { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether this instance is cancellation requested.
		/// </summary>
		/// <value>
		///     <c>true</c> if this instance is cancellation requested; otherwise, <c>false</c> .
		/// </value>
		protected bool IsCancellationRequested { get; set; }

		/// <summary>
		///     Gets or sets the index of the step.
		/// </summary>
		/// <value> The index of the step. </value>
		public int StepIndex { get; protected set; }

		/// <summary>
		///     Gets or sets the steps count.
		/// </summary>
		/// <value> The steps count. </value>
		public int StepsCount { get; protected set; }

		#region IDisposable Members
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region IExceptionHandlerContainer Members
		/// <summary>
		///     Gets the exception handler.
		/// </summary>
		/// <value> The exception handling. </value>
		public ExceptionHandling ExceptionHandling
		{
			get { return this._ExceptionHandling ?? (this._ExceptionHandling = new ExceptionHandling()); }
		}
		#endregion

		/// <summary>
		///     Occurs when [ended].
		/// </summary>
		public event EventHandler<OperationEndedEventArgs> Ended;

		/// <summary>
		///     Occurs when [progress increased].
		/// </summary>
		public event EventHandler<ProgressIncreaseEventArgs> ProgressIncreased;

		/// <summary>
		///     Occurs when [progress increasing].
		/// </summary>
		public event EventHandler<ProgressIncreaseEventArgs> ProgressIncreasing;

		/// <summary>
		///     Occurs when [started].
		/// </summary>
		public event EventHandler Started;

		/// <summary>
		///     Occurs when [starting].
		/// </summary>
		public event EventHandler<ActingEventArgs> Starting;

		/// <summary>
		///     Occurs when [cancellation requested].
		/// </summary>
		public event EventHandler CancellationRequested;

		/// <summary>
		///     Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		///     A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return this.Name ?? base.ToString();
		}

		/// <summary>
		///     When ovverriden Initializes the operation steps.
		/// </summary>
		/// <param name="steps"> The steps. </param>
		protected abstract void InitializeOperationSteps(Dictionary<Action, string> steps);

		protected virtual void OnEnded(OperationEndedEventArgs e)
		{
			this.Ended.Raise(this, e);
		}

		protected virtual void OnProgressIncreased(ProgressIncreaseEventArgs e)
		{
			this.ProgressIncreased.Raise(this, e);
		}

		protected virtual void OnProgressIncreasing(ProgressIncreaseEventArgs e)
		{
			this.ProgressIncreasing.Raise(this, e);
		}

		protected virtual void OnStarting(ActingEventArgs e)
		{
			this.Starting.Raise(this, e);
		}

		/// <summary>
		///     Starts this instance.
		/// </summary>
		public virtual void Start()
		{
			this.IsCancellationRequested = false;
			var mrs = new ManualResetEventSlim();
			var steps = new Dictionary<Action, string>();
			this.InitializeOperationSteps(steps);
			this.StepsCount = steps.Count;
			this.MainSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext();
			const TaskCreationOptions creationOptions = TaskCreationOptions.LongRunning;
			const TaskContinuationOptions continuationOptions = TaskContinuationOptions.PreferFairness;
			this.Task = Task.Factory.StartNew(() =>
			                                  {
				                                  mrs.Wait();
				                                  var e = new ActingEventArgs();
				                                  this.OnStarting(e);
				                                  if (e.Handled)
					                                  this.Cancel();
			                                  },
				this._CancellationTokenSource.Token,
				creationOptions,
				this.MainSynchronizationContext);
			this.Task = this.Task.ContinueWith(ent =>
			                                   {
				                                   mrs.Wait();
				                                   this.OnStarted(EventArgs.Empty);
			                                   },
				this._CancellationTokenSource.Token,
				continuationOptions,
				this.MainSynchronizationContext);
			var num = 0;
			this.StepIndex = 0;
			foreach (var step in steps)
			{
				num++;
				var numBuffer = num;
				var stepBuffer = step;
				this.Task = this.Task.ContinueWith(ent =>
				                                   {
					                                   mrs.Wait();
					                                   this.OnProgressIncreasing(new ProgressIncreaseEventArgs(this.StepsCount, numBuffer, stepBuffer.Value));
				                                   },
					this._CancellationTokenSource.Token,
					continuationOptions,
					this.MainSynchronizationContext);

				var step3 = step;
				this.Task = this.Task.ContinueWith(ent =>
				                                   {
					                                   mrs.Wait();
					                                   try
					                                   {
						                                   step3.Key();
						                                   lock (this)
						                                   {
							                                   this.StepIndex++;
						                                   }
					                                   }
					                                   catch (Exception ex)
					                                   {
						                                   this.ExceptionHandling.HandleException(this, ex);
						                                   if (!this.ContinueOnException)
							                                   this.Cancel();
					                                   }
				                                   },
					this._CancellationTokenSource.Token);

				var num2 = num;
				var step2 = step;
				this.Task = this.Task.ContinueWith(ent =>
				                                   {
					                                   mrs.Wait();
					                                   this.OnProgressIncreased(new ProgressIncreaseEventArgs(this.StepsCount, num2, step2.Value));
				                                   },
					this._CancellationTokenSource.Token,
					continuationOptions,
					this.MainSynchronizationContext);
			}
			this.Task = this.Task.ContinueWith(ent =>
			                                   {
				                                   mrs.Wait();
				                                   this.OnEnded(new OperationEndedEventArgs(!this.IsCancellationRequested));
			                                   },
				CancellationToken.None,
				continuationOptions,
				this.MainSynchronizationContext);
			mrs.Set();
		}

		protected virtual void OnStarted(EventArgs e)
		{
			this.Started.Raise(this, e);
		}

		protected virtual void OnCancellationRequested(EventArgs e)
		{
			this.CancellationRequested.Raise(this, e);
		}

		/// <summary>
		///     Cancels this instance.
		/// </summary>
		public void Cancel()
		{
			this.IsCancellationRequested = true;
			this._CancellationTokenSource.Cancel();
			this.OnCancellationRequested(EventArgs.Empty);
		}

		~MultiStepOperation()
		{
			this.Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			if (this._Disposed)
				return;
			if (disposing)
			{
				this.Task.Dispose();
				this._CancellationTokenSource.Dispose();
			}
			this._Disposed = true;
		}

		public static MultiStepOperation GetMultiStepOperation(Dictionary<Action, string> steps)
		{
			var result = new MultiStepOperationImpl(steps)
			             {
				             StepsCount = steps.Count
			             };
			return result;
		}

		private sealed class MultiStepOperationImpl : MultiStepOperation
		{
			public MultiStepOperationImpl(Dictionary<Action, string> steps)
			{
				this.Steps = steps;
			}

			private Dictionary<Action, string> Steps { get; set; }

			protected override void InitializeOperationSteps(Dictionary<Action, string> steps)
			{
				steps.AddMany(this.Steps);
			}
		}
	}
}
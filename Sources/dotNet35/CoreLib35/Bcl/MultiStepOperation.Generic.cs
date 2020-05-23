#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Library35.Collections.ObjectModel;
using Library35.EventsArgs;
using Library35.ExceptionHandlingPattern;
using Library35.Helpers;

namespace Library35.Bcl
{
	/// <summary>
	/// </summary>
	/// <typeparam name="TException"> The type of the exception. </typeparam>
	public abstract class MultiStepOperation<TException> : IExceptionHandlerContainer
		where TException : Exception
	{
		/// <summary>
		///     If true, the steps will be run in a thread.
		/// </summary>
		protected bool RunInThread = true;
		private bool _EnableRaisingEvents = true;
		private ExceptionHandling _ExceptionHandling;
		private ReadOnlyDictionary<Action, string> _Steps;
		/// <summary>
		///     Gets or sets a value indicating whether [enable raising events].
		/// </summary>
		/// <value>
		///     <c>true</c> if [enable raising events]; otherwise, <c>false</c> .
		/// </value>
		public bool EnableRaisingEvents
		{
			get { return this._EnableRaisingEvents; }
			set { this._EnableRaisingEvents = value; }
		}

		/// <summary>
		///     Gets or sets a value indicating whether an abort requested by user.
		/// </summary>
		/// <value>
		///     <c>true</c> if abort requested; otherwise, <c>false</c> .
		/// </value>
		protected bool AbortRequested { get; private set; }

		/// <summary>
		///     Gets or sets a value indicating whether continue on exception. While executing steps, the process will be kept on,
		///     if exception occurred, if true.
		/// </summary>
		/// <value>
		///     <c>true</c> if [continue on exception]; otherwise, <c>false</c> .
		/// </value>
		public bool ContinueOnException { get; set; }

		protected Thread Thread { get; set; }

		#region IExceptionHandlerContainer Members
		/// <summary>
		///     Gets or sets the exception handling.
		/// </summary>
		/// <value> The exception handling. </value>
		public ExceptionHandling ExceptionHandling
		{
			get
			{
				return this._ExceptionHandling ?? (this._ExceptionHandling = new ExceptionHandling
				                                                             {
					                                                             RaiseExceptions = false
				                                                             });
			}
			set { this._ExceptionHandling = value; }
		}
		#endregion

		///// <summary>
		/////   Occurs when exception occurred while executing steps
		///// </summary>
		//[Obsolete("Please handle 'ExceptionHandling' instead.", true)]
		//public event EventHandler<ExceptionOccurredEventArgs<TException>> ExceptionOccurred;

		/// <summary>
		///     Occurs when the operation is done. This event occurs even any exception is occurred or not.
		/// </summary>
		public event EventHandler<OperationEndedEventArgs> Ended;

		/// <summary>
		///     Occurs when a stop is completed.
		/// </summary>
		public event EventHandler<ProgressIncreaseEventArgs> ProgressIncreased;

		/// <summary>
		///     Occurs when a stop is starting.
		/// </summary>
		public event EventHandler<ProgressIncreaseEventArgs> ProgressIncreasing;

		/// <summary>
		///     Occurs when current progress increased, when overridden.
		/// </summary>
		public event EventHandler<ProgressIncreaseEventArgs> CurrentProgressIncreased;

		/// <summary>
		///     Occurs when current while progress increasing, when overridden.
		/// </summary>
		public event EventHandler<ProgressIncreaseEventArgs> CurrentProgressIncreasing;

		/// <summary>
		///     Occurs when executing the steps is started.
		/// </summary>
		public event EventHandler Started;

		/// <summary>
		///     Occurs when [starting].
		/// </summary>
		public event EventHandler<ActingEventArgs> Starting;

		/// <summary>
		///     Initializes the operation steps before starting to execute them.
		/// </summary>
		/// <param name="steps"> The steps. </param>
		protected abstract void InitializeOperationSteps(Dictionary<Action, string> steps);

		/// <summary>
		///     Generates the exception when implemented.
		/// </summary>
		/// <param name="ex"> The ex. </param>
		/// <returns> </returns>
		protected abstract TException GenerateException(Exception ex);

		/// <summary>
		///     Called when abort requested.
		/// </summary>
		protected virtual void OnAbortRequested()
		{
		}

		/// <summary>
		///     Raises the <see cref="E:Ended" /> event.
		/// </summary>
		/// <param name="e">
		///     The <see cref="Library35.EventsArgs.OperationEndedEventArgs" /> instance containing the event data.
		/// </param>
		protected virtual void OnEnded(OperationEndedEventArgs e)
		{
			this.Ended.Raise(this, e);
			this.AbortRequested = false;
		}

		/// <summary>
		///     Raises the <see cref="E:ProgressIncreased" /> event.
		/// </summary>
		/// <param name="e">
		///     The <see cref="Library35.EventsArgs.ProgressIncreaseEventArgs" /> instance containing the event data.
		/// </param>
		protected virtual void OnProgressIncreased(ProgressIncreaseEventArgs e)
		{
			//if (!this.EnableRaisingEvents)
			//    return;
			this.ProgressIncreased.Raise(this, e);
		}

		/// <summary>
		///     Raises the <see cref="E:ProgressIncreasing" /> event.
		/// </summary>
		/// <param name="e">
		///     The <see cref="Library35.EventsArgs.ProgressIncreaseEventArgs" /> instance containing the event data.
		/// </param>
		protected virtual void OnProgressIncreasing(ProgressIncreaseEventArgs e)
		{
			//if (!this.EnableRaisingEvents)
			//    return;
			this.ProgressIncreasing.Raise(this, e);
		}

		/// <summary>
		///     Raises the <see cref="E:CurrentProgressIncreased" /> event.
		/// </summary>
		/// <param name="e">
		///     The <see cref="Library35.EventsArgs.ProgressIncreaseEventArgs" /> instance containing the event data.
		/// </param>
		protected virtual void OnCurrentProgressIncreased(ProgressIncreaseEventArgs e)
		{
			if (!this.EnableRaisingEvents)
				return;
			this.CurrentProgressIncreased.Raise(this, e);
		}

		/// <summary>
		///     Raises the <see cref="E:ExceptionOccurred" /> event.
		/// </summary>
		/// <param name="e">
		///     The <see cref="Library35.EventsArgs.ExceptionOccurredEventArgs&lt;TException&gt;" /> instance containing the event
		///     data.
		/// </param>
		protected virtual void OnExceptionOccurred(ExceptionOccurredEventArgs<TException> e)
		{
			this.ExceptionHandling.HandleException(this, e.Exception);
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
			var e = new ActingEventArgs();
			this.OnStarting(e);
			if (e.Handled)
				return;
			this.StartCore();
			this.OnStarted(EventArgs.Empty);
		}

		/// <summary>
		///     Raises the <see cref="E:Started" /> event.
		/// </summary>
		/// <param name="e">
		///     The <see cref="System.EventArgs" /> instance containing the event data.
		/// </param>
		protected virtual void OnStarted(EventArgs e)
		{
			this.Started.Raise(this, e);
		}

		/// <summary>
		///     The main method to initialize the steps an manages the execution of each step and catches the exceptions if any.
		/// </summary>
		protected virtual void StartCore()
		{
			ThreadStart operation = this.OnStart;
			if (this.RunInThread)
			{
				this.Thread = new Thread(operation);
				this.Thread.Start();
			}
			else
				operation();
		}

		/// <summary>
		///     Called by StartCore
		/// </summary>
		protected virtual void OnStart()
		{
			var succeed = true;
			try
			{
				this.OnStarted(EventArgs.Empty);
				var steps = new Dictionary<Action, string>();
				this.InitializeOperationSteps(steps);
				this._Steps = new ReadOnlyDictionary<Action, string>(steps.AsEnumerable());
				this.MoveOn(this._Steps.ToDictionary());
			}
			catch (Exception ex)
			{
				succeed = false;
				this.OnExceptionOccurred(new ExceptionOccurredEventArgs<TException>(this.GenerateException(ex)));
			}
			finally
			{
				this.OnEnded(new OperationEndedEventArgs(succeed));
			}
		}

		/// <summary>
		///     Sets the [AbortRequested] flag and calls [OnAbortRequested()] once.
		/// </summary>
		public void Abort()
		{
			if (this.AbortRequested)
				return;
			this.AbortRequested = true;
			this.OnAbortRequested();
		}

		protected virtual void MoveOnCurrent(Dictionary<Action, string> steps)
		{
			this.MoveOn(steps, this.OnCurrentProgressIncreasing, this.OnCurrentProgressIncreased, this.ContinueOnException);
		}

		protected virtual void MoveOnCurrent(params KeyValuePair<Action, string>[] steps)
		{
			this.MoveOn(steps.ToDictionary(), this.OnCurrentProgressIncreasing, this.OnCurrentProgressIncreased, this.ContinueOnException);
		}

		/// <summary>
		///     Executes the steps one by one and calls the [progress increase] related methods.
		/// </summary>
		/// <param name="steps"> The steps. </param>
		/// <param name="onProgressIncreasing"> </param>
		/// <param name="onProgressIncreased"> </param>
		/// <param name="continueOnException"> </param>
		protected virtual void MoveOn(Dictionary<Action, string> steps,
			Action<ProgressIncreaseEventArgs> onProgressIncreasing,
			Action<ProgressIncreaseEventArgs> onProgressIncreased,
			bool continueOnException)
		{
			var num = 0;
			var max = steps.Count();
			foreach (var step in steps)
			{
				if (this.AbortRequested)
				{
					this.OnAborted(EventArgs.Empty);
					return;
				}
				num++;
				if (onProgressIncreasing != null)
					onProgressIncreasing(new ProgressIncreaseEventArgs(max, num, step.Value));
				try
				{
					step.Key();
				}
				catch
				{
					if (!continueOnException)
						throw;
				}
				if (onProgressIncreased != null)
					onProgressIncreased(new ProgressIncreaseEventArgs(max, num, step.Value));
			}
		}

		/// <summary>
		///     Occurs when [aborted].
		/// </summary>
		public event EventHandler Aborted;

		/// <summary>
		///     Raises the <see cref="E:Aborted" /> event.
		/// </summary>
		/// <param name="e">
		///     The <see cref="System.EventArgs" /> instance containing the event data.
		/// </param>
		protected virtual void OnAborted(EventArgs e)
		{
			this.Aborted.Raise(this);
		}

		private void MoveOn(Dictionary<Action, string> steps)
		{
			this.MoveOn(steps, this.OnProgressIncreasing, this.OnProgressIncreased, this.ContinueOnException);
		}

		/// <summary>
		///     Gets a new instance of MultiStepOperation.
		/// </summary>
		/// <param name="steps"> The steps. </param>
		/// <returns> </returns>
		public static MultiStepOperation GetMultiStepOperation(Dictionary<Action, string> steps)
		{
			return new SimpleOperation(steps);
		}

		/// <summary>
		///     Raises the <see cref="E:CurrentProgressIncreasing" /> event.
		/// </summary>
		/// <param name="e">
		///     The <see cref="Library35.EventsArgs.ProgressIncreaseEventArgs" /> instance containing the event data.
		/// </param>
		protected virtual void OnCurrentProgressIncreasing(ProgressIncreaseEventArgs e)
		{
			if (!this.EnableRaisingEvents)
				return;
			this.CurrentProgressIncreasing.Raise(this, e);
		}

		/// <summary>
		/// </summary>
		public void Join()
		{
			this.Thread.Join();
		}

		#region Nested type: SimpleOperation
		protected sealed class SimpleOperation : MultiStepOperation
		{
			public SimpleOperation(Dictionary<Action, string> steps)
			{
				this.Steps = steps;
			}

			private Dictionary<Action, string> Steps { get; set; }

			protected override void InitializeOperationSteps(Dictionary<Action, string> steps)
			{
				steps.AddMany(this.Steps);
			}
		}
		#endregion
	}
}
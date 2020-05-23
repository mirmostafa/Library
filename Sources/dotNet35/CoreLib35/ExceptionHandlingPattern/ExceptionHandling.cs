#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library35.EventsArgs;
using Library35.Helpers;

namespace Library35.ExceptionHandlingPattern
{
	public class ExceptionHandling<TException>
		where TException : Exception
	{
		public ExceptionHandling()
		{
			this.RaiseExceptions = true;
		}

		public TException LastException { get; private set; }

		public bool RaiseExceptions { get; set; }

		private object _Sender { get; set; }

		public event EventHandler<ExceptionOccurredEventArgs<TException>> ExceptionOccurred;

		internal void HandleException(TException ex)
		{
			this.HandleException(this._Sender, ex);
		}

		internal void HandleException(object sender, TException ex)
		{
			this.LastException = ex;
			this.ExceptionOccurred.Raise(sender, new ExceptionOccurredEventArgs<TException>(ex));
			if (this.RaiseExceptions)
				throw ex;
			//MethodHelper.Break();
		}

		internal void Reset()
		{
			this.LastException = null;
		}

		internal void SetSender(object sender)
		{
			this._Sender = sender;
		}

		public override string ToString()
		{
			return this.LastException == null ? "No error" : this.LastException.ToString();
		}
	}

	public class ExceptionHandling : ExceptionHandling<Exception>
	{
	}

	public class LazyExceptionHandling<TException> : LazyInitNew<ExceptionHandling<TException>>
		where TException : Exception
	{
	}

	public class LazyExceptionHandling : LazyInitNew<ExceptionHandling>
	{
	}
}
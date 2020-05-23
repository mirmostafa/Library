#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library40.EventsArgs
{
	public sealed class ExceptionOccurredEventArgs<TException> : EventArgs
		where TException : Exception
	{
		public ExceptionOccurredEventArgs(TException exception)
		{
			this.Exception = exception;
		}

		public bool Throw { get; set; }

		public TException Exception { get; private set; }
	}

	public sealed class ExceptionOccurredEventArgs : EventArgs
	{
		public ExceptionOccurredEventArgs(Exception exception)
		{
			this.Exception = exception;
		}

		public Exception Exception { get; private set; }
	}
}
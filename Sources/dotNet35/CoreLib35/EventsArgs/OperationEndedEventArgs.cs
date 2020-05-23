#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.EventsArgs
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

	public sealed class OperationEndedEventArgs : EventArgs
	{
		public OperationEndedEventArgs(bool succeed)
		{
			this.Succeed = succeed;
		}

		public bool Succeed { get; private set; }
	}
}
#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;

namespace Library35.Exceptions
{
	[Serializable]
	public class OperationAbortedException : ExceptionBase
	{
		public OperationAbortedException()
		{
		}

		public OperationAbortedException(string message)
			: base(message)
		{
		}

		public OperationAbortedException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected OperationAbortedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
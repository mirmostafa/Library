#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;

namespace Library40.Exceptions
{
	[Serializable]
	public class OperationAbortedException : Exception
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
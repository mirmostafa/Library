#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;
using Library40.Exceptions;

namespace Library40.Validation.Exceptions
{
	[Serializable]
	public class ValidationExceptionBase : LibraryExceptionBase
	{
		public ValidationExceptionBase()
		{
		}

		public ValidationExceptionBase(string message)
			: base(message)
		{
		}

		public ValidationExceptionBase(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected ValidationExceptionBase(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
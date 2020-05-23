#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;
using Library35.Exceptions;

namespace Library35.Validation.Exceptions
{
	[Serializable]
	public class ValidationException : CompanyException
	{
		public ValidationException()
		{
		}

		public ValidationException(string message)
			: base(message)
		{
		}

		public ValidationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected ValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;
using Library35.Exceptions;

namespace Library35.Data.Linq.Exceptions
{
	[Serializable]
	public class DataValidationException : CompanyException
	{
		// Methods
		public DataValidationException()
		{
		}

		public DataValidationException(string message)
			: base(message)
		{
		}

		protected DataValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public DataValidationException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;
using Library40.Exceptions;

namespace Library40.Data.Common.Exceptions
{
	[Serializable]
	public class DataValidationExceptionBase : ExceptionBase
	{
		// Methods
		public DataValidationExceptionBase()
		{
		}

		public DataValidationExceptionBase(string message)
			: base(message)
		{
		}

		protected DataValidationExceptionBase(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public DataValidationExceptionBase(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
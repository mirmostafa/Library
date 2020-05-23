#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;

namespace Library35.Validation.Exceptions
{
	[Serializable]
	public class NotNullOrZeroValidationException : ValidationException
	{
		public NotNullOrZeroValidationException()
		{
		}

		protected NotNullOrZeroValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public NotNullOrZeroValidationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		public NotNullOrZeroValidationException(string parameterName)
			: this()
		{
			this.ParameterName = parameterName;
		}

		public string ParameterName { get; set; }
	}
}
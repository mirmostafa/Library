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
	public class NotLessThanValueValidationException : ValidationException
	{
		public NotLessThanValueValidationException()
		{
		}

		protected NotLessThanValueValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public NotLessThanValueValidationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		public NotLessThanValueValidationException(string parameterName, object value)
			: this()
		{
			this.ParameterName = parameterName;
			this.Value = value;
		}

		public string ParameterName { get; set; }

		public object Value { get; set; }
	}
}
#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;

namespace Library40.Validation.Exceptions
{
	[Serializable]
	public class NotLessThanValueValidationExceptionBase : ValidationExceptionBase
	{
		public NotLessThanValueValidationExceptionBase()
		{
		}

		protected NotLessThanValueValidationExceptionBase(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public NotLessThanValueValidationExceptionBase(string message, Exception inner)
			: base(message, inner)
		{
		}

		public NotLessThanValueValidationExceptionBase(string parameterName, object value)
			: this()
		{
			this.ParameterName = parameterName;
			this.Value = value;
		}

		public string ParameterName { get; set; }

		public object Value { get; set; }
	}
}
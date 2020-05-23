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
	public class NotNullOrZeroValidationExceptionBase : ValidationExceptionBase
	{
		public NotNullOrZeroValidationExceptionBase()
		{
		}

		protected NotNullOrZeroValidationExceptionBase(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public NotNullOrZeroValidationExceptionBase(string message, Exception inner)
			: base(message, inner)
		{
		}

		public NotNullOrZeroValidationExceptionBase(string parameterName)
			: this()
		{
			this.ParameterName = parameterName;
		}

		public string ParameterName { get; set; }
	}
}
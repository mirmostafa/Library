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
	public class NotBetweenMaxAndMinValuesValidationExceptionBase : ValidationExceptionBase
	{
		public NotBetweenMaxAndMinValuesValidationExceptionBase()
		{
		}

		public NotBetweenMaxAndMinValuesValidationExceptionBase(string message)
			: base(message)
		{
		}

		public NotBetweenMaxAndMinValuesValidationExceptionBase(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected NotBetweenMaxAndMinValuesValidationExceptionBase(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public NotBetweenMaxAndMinValuesValidationExceptionBase(object value, object minValue, object maxValue)
		{
			this.Value = value;
			this.MinValue = minValue;
			this.MaxValue = maxValue;
		}

		public object Value { get; set; }

		public object MinValue { get; set; }

		public object MaxValue { get; set; }
	}
}
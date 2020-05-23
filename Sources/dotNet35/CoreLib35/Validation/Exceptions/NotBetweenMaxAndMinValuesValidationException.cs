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
	public class NotBetweenMaxAndMinValuesValidationException : ValidationException
	{
		public NotBetweenMaxAndMinValuesValidationException()
		{
		}

		public NotBetweenMaxAndMinValuesValidationException(string message)
			: base(message)
		{
		}

		public NotBetweenMaxAndMinValuesValidationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected NotBetweenMaxAndMinValuesValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public NotBetweenMaxAndMinValuesValidationException(object value, object minValue, object maxValue)
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
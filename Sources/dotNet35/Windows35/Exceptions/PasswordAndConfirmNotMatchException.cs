#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;
using Library35.Validation.Exceptions;
using Library35.Windows.Properties;

namespace Library35.Windows.Exceptions
{
	[Serializable]
	public class PasswordAndConfirmNotMatchException : ValidationException
	{
		public PasswordAndConfirmNotMatchException()
			: base(Resources.PasswordAnConfirmNotMatch)
		{
		}

		public PasswordAndConfirmNotMatchException(string message)
			: base(message)
		{
		}

		public PasswordAndConfirmNotMatchException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected PasswordAndConfirmNotMatchException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
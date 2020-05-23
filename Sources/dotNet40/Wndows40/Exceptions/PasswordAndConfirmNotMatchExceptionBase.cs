#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;
using Library40.Validation.Exceptions;
using Library40.Win.Properties;

namespace Library40.Win.Exceptions
{
	[Serializable]
	public class PasswordAndConfirmNotMatchExceptionBase : ValidationExceptionBase
	{
		public PasswordAndConfirmNotMatchExceptionBase()
			: base(Resources.PasswordAnConfirmNotMatch)
		{
		}

		public PasswordAndConfirmNotMatchExceptionBase(string message)
			: base(message)
		{
		}

		public PasswordAndConfirmNotMatchExceptionBase(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected PasswordAndConfirmNotMatchExceptionBase(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
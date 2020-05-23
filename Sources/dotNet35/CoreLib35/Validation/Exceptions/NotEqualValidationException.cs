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
	public class NotEqualValidationException : ValidationException
	{
		public NotEqualValidationException()
		{
		}

		protected NotEqualValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public NotEqualValidationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		public NotEqualValidationException(object obj1Name, object obj2Name)
			: this()
		{
			this.Obj1Name = obj1Name;
			this.Obj2Name = obj2Name;
		}

		public object Obj1Name { get; set; }

		public object Obj2Name { get; set; }
	}
}
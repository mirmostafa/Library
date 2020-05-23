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
	public class NotEqualValidationExceptionBase : ValidationExceptionBase
	{
		public NotEqualValidationExceptionBase()
		{
		}

		protected NotEqualValidationExceptionBase(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public NotEqualValidationExceptionBase(string message, Exception inner)
			: base(message, inner)
		{
		}

		public NotEqualValidationExceptionBase(object obj1Name, object obj2Name)
			: this()
		{
			this.Obj1Name = obj1Name;
			this.Obj2Name = obj2Name;
		}

		public object Obj1Name { get; set; }

		public object Obj2Name { get; set; }
	}
}
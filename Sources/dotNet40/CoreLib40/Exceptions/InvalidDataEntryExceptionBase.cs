#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;

namespace Library40.Exceptions
{
	[Serializable]
	public class InvalidDataEntryExceptionBase : LibraryExceptionBase
	{
		public InvalidDataEntryExceptionBase()
		{
		}

		public InvalidDataEntryExceptionBase(string message)
			: base(message)
		{
		}

		public InvalidDataEntryExceptionBase(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected InvalidDataEntryExceptionBase(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public static void Throw()
		{
			Throw<InvalidDataEntryExceptionBase>();
		}

		public static void Throw(string message)
		{
			throw new InvalidDataEntryExceptionBase(message);
		}

		public static void Assert(Func<bool> predicate, string message)
		{
			if (predicate())
				throw new InvalidDataEntryExceptionBase(message);
		}
	}
}
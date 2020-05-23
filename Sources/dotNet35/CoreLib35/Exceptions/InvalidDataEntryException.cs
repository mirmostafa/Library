#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;

namespace Library35.Exceptions
{
	[Serializable]
	public class InvalidDataEntryException : LibraryException
	{
		public InvalidDataEntryException()
		{
		}

		public InvalidDataEntryException(string message)
			: base(message)
		{
		}

		public InvalidDataEntryException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected InvalidDataEntryException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public static void Throw()
		{
			Throw<InvalidDataEntryException>();
		}

		public static void Throw(string message)
		{
			throw new InvalidDataEntryException(message);
		}

		public static void Assert(Func<bool> predicate, string message)
		{
			if (predicate())
				throw new InvalidDataEntryException(message);
		}
	}
}
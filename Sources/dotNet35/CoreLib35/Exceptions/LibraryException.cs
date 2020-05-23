#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;

namespace Library35.Exceptions
{
	/// <summary>
	/// </summary>
	[Serializable]
	public class LibraryException : ExceptionBase
	{
		/// <summary>
		/// </summary>
		public LibraryException()
		{
		}

		protected LibraryException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="message"> </param>
		public LibraryException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="message"> </param>
		/// <param name="inner"> </param>
		public LibraryException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
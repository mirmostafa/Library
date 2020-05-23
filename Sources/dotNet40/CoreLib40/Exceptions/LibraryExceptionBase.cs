#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;

namespace Library40.Exceptions
{
	/// <summary>
	/// </summary>
	[Serializable]
	public class LibraryExceptionBase : ExceptionBase
	{
		/// <summary>
		/// </summary>
		public LibraryExceptionBase()
		{
		}

		protected LibraryExceptionBase(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="message"></param>
		public LibraryExceptionBase(string message)
			: base(message)
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public LibraryExceptionBase(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
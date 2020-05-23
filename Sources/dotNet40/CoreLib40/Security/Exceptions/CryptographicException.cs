#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;

namespace Library40.Security.Exceptions
{
	/// <summary>
	///     You can handle exception of the Cryptography in Company assembly.
	/// </summary>
	[Serializable]
	public class CryptographicException : System.Security.Cryptography.CryptographicException
	{
		public CryptographicException()
		{
		}

		public CryptographicException(string message)
			: base(message)
		{
		}

		public CryptographicException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected CryptographicException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
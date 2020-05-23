#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.Security.Cryptography
{
	/// <summary>
	///     You can handle exception of the Cryptography in Company assembly.
	/// </summary>
	public class CryptographicException : System.Security.Cryptography.CryptographicException
	{
		/// <summary>
		/// </summary>
		public CryptographicException()
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="message"> </param>
		public CryptographicException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="message"> </param>
		/// <param name="innerException"> </param>
		public CryptographicException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
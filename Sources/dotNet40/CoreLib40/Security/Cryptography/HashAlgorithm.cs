#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Security.Cryptography;
using System.Text;
using Library40.Security.Cryptography.Interfaces;

namespace Library40.Security.Cryptography
{
	/// <summary>
	/// </summary>
	public class HashAlgorithm : IUnidirectionalCryptographicAlgorithm
	{
		/// <summary>
		/// </summary>
		public string AppendString { get; set; }

		#region IUnidirectionalCryptographicAlgorithm Members
		/// <summary>
		///     Encrypts a string.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns>String expression to be encrypted.</returns>
		public string Encrypt(string expression)
		{
			var md5 = new MD5CryptoServiceProvider();
			var str = Encoding.ASCII.GetBytes(expression + this.AppendString);
			var result = md5.ComputeHash(str);
			var strs = Convert.ToBase64String(result);
			var builder = new StringBuilder();
			foreach (var hashByte in result)
				builder.Append(hashByte.ToString("x2"));
			return strs.Trim();
		}
		#endregion
	}
}
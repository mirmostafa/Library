#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Library35.Security.Cryptography.Interfaces;

namespace Library35.Security.Cryptography
{
	/// <summary>
	///     This class can encrypt and decrypt of the string that you
	///     want.
	/// </summary>
	/// <example>
	///     <code>try
	///     <para> { </para>
	///     <para>
	///             Library.Security.Cryptography.Symmetric.Key = new byte[] { 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2,
	///             5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2,
	///             5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2 };
	///         </para>
	///     <para> </para>
	///     <para> string id = &quot; admin &quot; ; </para>
	///     <para> </para>
	///     <para> string password = &quot; 123*_+#$#@$(#*$%(#*@$ &quot; ; </para>
	///     <para> </para>
	///     <para> password = new Library.Security.Cryptography.Symmetric().Encrypt(password); </para>
	///     <para> </para>
	///     <para> Response.Write(password + &quot; &lt; br &gt; &quot; ); </para>
	///     <para> </para>
	///     <para> password = new Library.Security.Cryptography.Symmetric().Decrypt(password); </para>
	///     <para> </para>
	///     <para> Response.Write(password); </para>
	///     <para> } </para>
	///     <para> catch (Library.Security.Cryptography.CryptographyException ex) </para>
	///     <para> { </para>
	///     <para> Response.Write(ex.Message); </para>
	///     <para> } </para>
	///   </code>
	/// </example>
	/// <author>Mohammad</author>
	public class SymmetricAlgorithm : IBidirectionalCryptographicAlgorithm
	{
		/// <summary>
		///     Gets or sets the Public Key must be the same at all a project.
		/// </summary>
		/// <remarks>
		///     Public Key must be the same at all a project
		/// </remarks>
		public IEnumerable<byte> PublicKey { get; set; }

		#region IBidirectionalCryptographicAlgorithm Members
		/// <summary>
		///     Encrypts a string.
		/// </summary>
		/// <param name="expression"> </param>
		/// <returns> String expression to be encrypted. </returns>
		public string Encrypt(string expression)
		{
			return this.DoSymmetric(expression, true);
		}

		/// <summary>
		///     Decrypts an encripted string.
		/// </summary>
		/// <param name="encryptedExpression"> </param>
		/// <returns> String encryptedExpression to be decrypted. </returns>
		/// <exception cref="CryptographicException">Exception:</exception>
		public string Decrypt(string encryptedExpression)
		{
			return this.DoSymmetric(encryptedExpression, false);
		}
		#endregion

		private string DoSymmetric(string input, bool isEncrypt)
		{
			if (this.PublicKey == null)
				throw new CryptographicException("Key is null.");

			byte[] toEncryptArray;

			if (isEncrypt)
				toEncryptArray = Encoding.UTF8.GetBytes(input);
			else
				try
				{
					toEncryptArray = Convert.FromBase64String(input);
				}
				catch
				{
					throw new CryptographicException("Original data has been changed.");
				}

			var hashmd5 = new MD5CryptoServiceProvider();
			var keyArray = hashmd5.ComputeHash(this.PublicKey.ToArray());
			hashmd5.Clear();
			var tdes = new TripleDESCryptoServiceProvider
			           {
				           Key = keyArray,
				           Mode = CipherMode.ECB,
				           Padding = PaddingMode.PKCS7
			           };

			byte[] resultArray;

			if (isEncrypt)
				resultArray = tdes.CreateEncryptor().TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			else
				try
				{
					resultArray = tdes.CreateDecryptor().TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
				}
				catch
				{
					throw new CryptographicException("Original data has been changed.");
				}

			tdes.Clear();

			return (isEncrypt) ? Convert.ToBase64String(resultArray, 0, resultArray.Length) : Encoding.UTF8.GetString(resultArray);
		}

		/* Example:

            try
            {
                Library.Security.Cryptography.Symmetric.Key = new byte[] { 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2, 5, 2 };

                string id = "admin";

                string password = "123*_+#$#@$(#*$%(#*@$";

                password = new Company.Security.Cryptography.Symmetric().Encrypt(password);

                Response.Write(password + "<br>");

                password = new Company.Security.Cryptography.Symmetric().Decrypt(password);

                Response.Write(password);
            }
            catch (Company.Security.Cryptography.CryptographyException ex)
            {
                Response.Write(ex.Message);
            }

         */
	}
}
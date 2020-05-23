#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Library35.Security.Cryptography
{
	public static class RijndaelEncryption
	{
		private static readonly byte[] _SaltSize =
		{
			0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
		};

		public static byte[] Encrypt(byte[] clearData, byte[] key, byte[] iv)
		{
			byte[] encryptedData;
			using (var ms = new MemoryStream())
			{
				using (var alg = Rijndael.Create())
				{
					alg.Key = key;
					alg.IV = iv;
					using (var cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearData, 0, clearData.Length);
						cs.Close();
					}
				}
				encryptedData = ms.ToArray();
			}
			return encryptedData;
		}

		public static string Encrypt(string clearText, string password)
		{
			var clearBytes = Encoding.Unicode.GetBytes(clearText);
			var pdb = new Rfc2898DeriveBytes(password, _SaltSize);
			var encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
			return Convert.ToBase64String(encryptedData);
		}

		public static byte[] Encrypt(byte[] clearData, string password)
		{
			var pdb = new Rfc2898DeriveBytes(password, _SaltSize);
			return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));
		}

		public static void Encrypt(string fileIn, string fileOut, string password)
		{
			using (var fsIn = new FileStream(fileIn, FileMode.Open, FileAccess.Read))
			{
				using (var fsOut = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write))
				{
					var pdb = new Rfc2898DeriveBytes(password, _SaltSize);
					using (var alg = Rijndael.Create())
					{
						alg.Key = pdb.GetBytes(32);
						alg.IV = pdb.GetBytes(16);
						using (var cs = new CryptoStream(fsOut, alg.CreateEncryptor(), CryptoStreamMode.Write))
						{
							const int bufferLen = 4096;
							var buffer = new byte[bufferLen];
							int bytesRead;
							do
							{
								bytesRead = fsIn.Read(buffer, 0, bufferLen);
								cs.Write(buffer, 0, bytesRead);
							}
							while (bytesRead != 0);
							cs.Close();
						}
					}
				}
				fsIn.Close();
			}
		}

		public static byte[] Decrypt(byte[] cipherData, byte[] key, byte[] iv)
		{
			using (var memoryStream = new MemoryStream())
			{
				using (var rijndael = Rijndael.Create())
				{
					rijndael.Key = key;
					rijndael.IV = iv;
					using (var cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(cipherData, 0, cipherData.Length);
						cryptoStream.Close();
					}
				}
				return memoryStream.ToArray();
			}
		}

		public static string Decrypt(string cipherText, string password)
		{
			var cipherBytes = Convert.FromBase64String(cipherText);
			byte[] decryptedData;
			var pdb = new Rfc2898DeriveBytes(password, _SaltSize);
			decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
			return Encoding.Unicode.GetString(decryptedData);
		}

		public static byte[] Decrypt(byte[] cipherData, string password)
		{
			var pdb = new Rfc2898DeriveBytes(password, _SaltSize);
			return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
		}

		public static void Decrypt(string fileIn, string fileOut, string password)
		{
			using (var fsIn = new FileStream(fileIn, FileMode.Open, FileAccess.Read))
			{
				using (var fsOut = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write))
				{
					var pdb = new Rfc2898DeriveBytes(password, _SaltSize);
					using (var alg = Rijndael.Create())
					{
						alg.Key = pdb.GetBytes(32);
						alg.IV = pdb.GetBytes(16);
						using (var cs = new CryptoStream(fsOut, alg.CreateDecryptor(), CryptoStreamMode.Write))
						{
							const int bufferLen = 4096;
							var buffer = new byte[bufferLen];
							int bytesRead;
							do
							{
								bytesRead = fsIn.Read(buffer, 0, bufferLen);
								cs.Write(buffer, 0, bytesRead);
							}
							while (bytesRead != 0);
							cs.Close();
						}
					}
				}

				fsIn.Close();
			}
		}
	}
}
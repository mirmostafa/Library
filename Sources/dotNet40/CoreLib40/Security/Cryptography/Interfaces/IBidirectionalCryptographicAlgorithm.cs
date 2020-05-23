#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library40.Security.Cryptography.Interfaces
{
	/// <summary>
	/// </summary>
	public interface IBidirectionalCryptographicAlgorithm : IUnidirectionalCryptographicAlgorithm
	{
		/// <summary>
		///     Decrypts an encripted string.
		/// </summary>
		/// <param name="encryptedExpression"></param>
		/// <returns>String encryptedExpression to be decrypted.</returns>
		/// <exception cref="CryptographicException">Exception:</exception>
		string Decrypt(string encryptedExpression);
	}
}
#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library35.Security.Cryptography.Interfaces
{
	/// <summary>
	/// </summary>
	public interface IUnidirectionalCryptographicAlgorithm
	{
		/// <summary>
		///     Encrypts a string.
		/// </summary>
		/// <param name="expression"> </param>
		/// <returns> String expression to be encrypted. </returns>
		string Encrypt(string expression);
	}
}
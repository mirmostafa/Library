#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

namespace Mohammad.Security.Cryptography.Interfaces
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
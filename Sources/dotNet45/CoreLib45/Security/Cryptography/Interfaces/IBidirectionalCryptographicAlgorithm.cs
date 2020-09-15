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
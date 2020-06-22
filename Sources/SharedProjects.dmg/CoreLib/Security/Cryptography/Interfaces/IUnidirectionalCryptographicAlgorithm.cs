namespace Mohammad.Security.Cryptography.Interfaces
{
    /// <summary>
    /// </summary>
    public interface IUnidirectionalCryptographicAlgorithm
    {
        /// <summary>
        ///     Encrypts a string.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>String expression to be encrypted.</returns>
        string Encrypt(string expression);
    }
}
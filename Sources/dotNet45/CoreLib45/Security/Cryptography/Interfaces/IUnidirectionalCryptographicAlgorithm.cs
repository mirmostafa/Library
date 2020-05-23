#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

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
using System;

namespace Mohammad.Security.Cryptography
{
    /// <summary>
    ///     You can handle exception of the Cryptography in Company assembly.
    /// </summary>
    public class CryptographicException : System.Security.Cryptography.CryptographicException
    {
        /// <summary>
        /// </summary>
        public CryptographicException() { }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        public CryptographicException(string message)
            : base(message) { }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CryptographicException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
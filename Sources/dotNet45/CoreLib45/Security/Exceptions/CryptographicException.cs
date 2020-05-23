#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Runtime.Serialization;

namespace Mohammad.Security.Exceptions
{
    /// <summary>
    ///     You can handle exception of the Cryptography in Company assembly.
    /// </summary>
    [Serializable]
    public class CryptographicException : System.Security.Cryptography.CryptographicException
    {
        public CryptographicException()
        {
        }

        public CryptographicException(string message)
            : base(message)
        {
        }

        public CryptographicException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected CryptographicException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
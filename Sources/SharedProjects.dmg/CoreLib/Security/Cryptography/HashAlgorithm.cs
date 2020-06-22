using System;
using System.Security.Cryptography;
using System.Text;
using Mohammad.Security.Cryptography.Interfaces;

namespace Mohammad.Security.Cryptography
{
    /// <summary>
    /// </summary>
    public class HashAlgorithm : IUnidirectionalCryptographicAlgorithm
    {
        /// <summary>
        /// </summary>
        public string AppendString { get; set; }

        /// <summary>
        ///     Encrypts a string.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>String expression to be encrypted.</returns>
        public string Encrypt(string expression)
        {
            //var md5 = new MD5CryptoServiceProvider();
            //var str = Encoding.ASCII.GetBytes(expression + this.AppendString);
            //var result = md5.ComputeHash(str);
            //var strs = Convert.ToBase64String(result);
            //var builder = new StringBuilder();
            //foreach (var hashByte in result)
            //    builder.Append(hashByte.ToString("x2"));
            //return strs.Trim();
            var sha1 = new SHA1CryptoServiceProvider();
            var result = sha1.ComputeHash(Encoding.UTF8.GetBytes(expression));
            var hexString = BitConverter.ToString(result).Replace("-", "").ToLower();
            return hexString;
        }
    }
}
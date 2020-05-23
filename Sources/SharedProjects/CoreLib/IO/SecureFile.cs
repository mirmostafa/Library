using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using Mohammad.Helpers;
using Mohammad.Security.Cryptography;

namespace Mohammad.IO
{
    public class SecureFile
    {
        private readonly FileInfo _File;
        private readonly string _Key;
        private const string _Separator = "tyupoi";

        public FileInfo OriginalFile
        {
            get
            {
                if (!this._File.Directory.Exists)
                    this._File.Directory.Create();
                if (!this._File.Exists)
                    this._File.Create().Close();
                return this._File;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SecureFile" /> class.
        /// </summary>
        /// <param name="file"> The file. </param>
        /// <param name="key"> The key. </param>
        public SecureFile(FileInfo file, string key)
        {
            Contract.Requires(file != null);
            this._File = file;
            this._Key = key;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SecureFile" /> class.
        /// </summary>
        /// <param name="file"> The file. </param>
        /// <param name="key"> The key. </param>
        public SecureFile(string file, string key)
            : this(new FileInfo(file), key)
        {
            Contract.Requires(file != null);
            Contract.Requires(file.Length > 0);
        }

        //public void Write(string text)
        //{
        //    if (!this._File.Directory.Exists)
        //        this._File.Directory.Create();
        //    using (var stream = new FileStream(this._File.FullName, FileMode.OpenOrCreate, FileAccess.Write))
        //    using (var des = new DESCryptoServiceProvider
        //    {
        //        Key = Encoding.ASCII.GetBytes(this._Key),
        //        IV = Encoding.ASCII.GetBytes(this._Key)
        //    })
        //    using (var desEncrypt = des.CreateEncryptor())
        //    using (var cryptoStream = new CryptoStream(stream, desEncrypt, CryptoStreamMode.Write))
        //    {
        //        var fsInput = Encoding.UTF8.GetBytes(text);
        //        stream.Seek(0, SeekOrigin.End);
        //        cryptoStream.Write(fsInput, 0, fsInput.Length);
        //    }
        //}

        //public string Read()
        //{
        //    var des = new DESCryptoServiceProvider
        //    {
        //        Key = Encoding.ASCII.GetBytes(this._Key),
        //        IV = Encoding.ASCII.GetBytes(this._Key)
        //    };

        //    using (var stream = new FileStream(this._File.FullName, FileMode.Open, FileAccess.Read))
        //    using (var desdecrypt = des.CreateDecryptor())
        //    using (var cryptoStream = new CryptoStream(stream, desdecrypt, CryptoStreamMode.Read))
        //    using (var reader = new StreamReader(cryptoStream))
        //        return reader.ReadToEnd();
        //}

        //public void Write(string text)
        //{
        //    //var hash = new SymmetricAlgorithm
        //    //{
        //    //    PublicKey = Encoding.UTF8.GetBytes(this._Key)
        //    //};
        //    //var encrypt = hash.Encrypt(text);
        //    var encrypt = RijndaelEncryption.Encrypt(Encoding.UTF8.GetBytes(text), this._Key);
        //    using (var stream = this._File.OpenWrite())
        //    {
        //        stream.Seek(0, SeekOrigin.End);
        //        //var bytes = Encoding.UTF8.GetBytes(encrypt);
        //        //stream.Write(bytes, 0, bytes.Length);
        //        stream.Write(encrypt, 0, encrypt.Length);
        //    }
        //}

        //public string Read()
        //{
        //    //var hash = new SymmetricAlgorithm
        //    //{
        //    //    PublicKey = Encoding.UTF8.GetBytes(this._Key)
        //    //};
        //    using (var stream = this._File.OpenRead())
        //    {
        //        stream.Seek(0, SeekOrigin.End);
        //        var length = stream.Position;
        //        stream.Seek(0, SeekOrigin.Begin);
        //        var buffer = new byte[length];
        //        stream.Read(buffer, 0, buffer.Length.ToInt());
        //        //return hash.Decrypt(Encoding.UTF8.GetString(buffer));
        //        return Encoding.UTF8.GetString(RijndaelEncryption.Decrypt(buffer, this._Key));
        //    }
        //}

        /// <summary>
        ///     Writes the specified text.
        /// </summary>
        /// <param name="text"> The text. </param>
        public void Write(string text)
        {
            Contract.Requires(text != null);
            if (!this.OriginalFile.Directory.Exists)
                this.OriginalFile.Directory.Create();

            var encrypt = RijndaelEncryption.Encrypt(text, this._Key);
            File.AppendAllText(this.OriginalFile.FullName, string.Concat(_Separator, encrypt));
        }

        public void WriteLine(string text) { this.Write(string.Concat(text, Environment.NewLine)); }

        public IEnumerable<string> ReadLines()
        {
            var lines = File.ReadAllText(this.OriginalFile.FullName).Split(new[] {_Separator}, StringSplitOptions.None).Compact();
            foreach (var line in lines)
                if (line.Equals(_Separator))
                    yield return Environment.NewLine;
                else
                    yield return RijndaelEncryption.Decrypt(line, this._Key);
        }

        public string Read() { return string.Concat(this.ReadLines()); }
        public override string ToString() { return this.OriginalFile.FullName; }
    }
}
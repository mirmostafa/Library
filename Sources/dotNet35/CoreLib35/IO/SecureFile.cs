#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Library35.Helpers;
using Library35.Security.Cryptography;

namespace Library35.IO
{
	public class SecureFile
	{
		private const string _Separator = "tyupoi";
		private readonly FileInfo _File;
		private readonly string _Key;

		/// <summary>
		///     Initializes a new instance of the <see cref="SecureFile" /> class.
		/// </summary>
		/// <param name="file"> The file. </param>
		/// <param name="key"> The key. </param>
		public SecureFile(FileInfo file, string key)
		{
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
		}

		public FileInfo OriginalFile
		{
			get
			{
				this._File.Refresh();
				if (!this._File.Directory.Exists)
					this._File.Directory.Create();
				if (!this._File.Exists)
					this._File.Create().Close();
				return this._File;
			}
		}

		/// <summary>
		///     Writes the specified text.
		/// </summary>
		/// <param name="text"> The text. </param>
		public void Write(string text)
		{
			if (!this.OriginalFile.Directory.Exists)
				this.OriginalFile.Directory.Create();

			var encrypt = RijndaelEncryption.Encrypt(text, this._Key);
			File.AppendAllText(this.OriginalFile.FullName, string.Concat(_Separator, encrypt));
		}

		public void WriteLine(string text)
		{
			this.Write(string.Concat(text, Environment.NewLine));
		}

		public IEnumerable<string> ReadLines()
		{
			var lines = File.ReadAllText(this.OriginalFile.FullName).Split(new[]
			                                                               {
				                                                               _Separator
			                                                               },
				StringSplitOptions.None).Compact();
			foreach (var line in lines)
				if (line.Equals(_Separator))
					yield return Environment.NewLine;
				else
					yield return RijndaelEncryption.Decrypt(line, this._Key);
		}

		public string Read()
		{
			return string.Concat(this.ReadLines());
		}

		public override string ToString()
		{
			return this.OriginalFile.FullName;
		}
	}
}
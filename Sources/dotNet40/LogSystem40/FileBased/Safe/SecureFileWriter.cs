#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library40.IO;
using Library40.LogSystem.Entities;
using Library40.LogSystem.Internals;

namespace Library40.LogSystem.FileBased.Safe
{
	public class SecureFileWriter : IWriter<LogEntity>
	{
		private readonly SecureFile _Storage;

		public SecureFileWriter(string key)
			: this(LoggerDefaults.GenerateLogFileSpec(), key)
		{
		}

		public SecureFileWriter(string fileName, string key)
		{
			this._Storage = new SecureFile(fileName, key);
		}

		public SecureFile Storage
		{
			get { return this._Storage; }
		}

		#region IWriter<LogEntity> Members
		public bool ShowInDebuggerTracer { get; set; }

		public void Write(LogEntity logEntity)
		{
			this.Storage.Write(logEntity.ToString());
		}

		public void LoadLastLog()
		{
			throw new NotImplementedException();
		}
		#endregion

		public override string ToString()
		{
			return this.Storage.ToString();
		}
	}
}
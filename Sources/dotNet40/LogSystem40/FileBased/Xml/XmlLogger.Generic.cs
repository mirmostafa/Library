#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.IO;
using Library40.LogSystem.Entities;
using Library40.LogSystem.Internals;

namespace Library40.LogSystem.FileBased.Xml
{
	/// <summary>
	///     Facade for writing a log entry(es) optimized for XML log writer.
	/// </summary>
	public class XmlLogger<TLogEntity> : Logger<XmlWriter<TLogEntity>, TLogEntity>, IStorageReadable
		where TLogEntity : LogEntity, new()
	{
		public XmlLogger(DirectoryInfo logPath = null, bool useLogRotation = false)
		{
			this.Initialize(logPath, useLogRotation);
		}

		#region IStorageReadable Members
		public string StorageFilePath
		{
			get { return this.Writer.Log.FullName; }
		}
		#endregion

		public override string ToString()
		{
			return this.StorageFilePath;
		}

		private void Initialize(DirectoryInfo logPath, bool useLogRotation)
		{
			var asm = Utilities.GetCaller();
			var storageName = useLogRotation
				? Utilities.GernerateFileSpec(new DirectoryInfo(Path.Combine(logPath != null ? logPath.FullName : new FileInfo(asm.Location).Directory.FullName, "Logs")),
					asm.FullName.Substring(0, asm.FullName.IndexOf(',')),
					".log.xml")
				: String.Concat(logPath != null ? Path.Combine(logPath.FullName, asm.FullName.Substring(0, asm.FullName.IndexOf(','))) : asm.Location, ".log.xml");
			this.Writer = new XmlWriter<TLogEntity>(storageName);
			if (LoadLastLogOnInitialize)
				this.Writer.LoadLastLog();
		}
	}
}
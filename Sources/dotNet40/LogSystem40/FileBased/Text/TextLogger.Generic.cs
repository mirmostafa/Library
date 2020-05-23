#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.IO;
using Library40.LogSystem.Entities;
using Library40.LogSystem.Internals;

namespace Library40.LogSystem.FileBased.Text
{
	/// <summary>
	///     Facade for writing a log entry(es) optimized for text log writer. This class cannot be inherited.
	/// </summary>
	public class TextLogger<TLogEntity> : Logger<TextWriter<TLogEntity>, TLogEntity>, IStorageReadable
		where TLogEntity : LogEntity, new()
	{
		public TextLogger(DirectoryInfo logPath, bool useLogRotation)
		{
			this.Initialize(logPath, useLogRotation);
		}

		public TextLogger(bool useLogRotation)
		{
			var asm = Utilities.GetCaller();
			var storageName = useLogRotation
				? Utilities.GernerateFileSpec(new DirectoryInfo(string.Concat(new FileInfo(asm.Location).Directory.FullName, "\\Logs")),
					asm.FullName.Substring(0, asm.FullName.IndexOf(',')),
					".log.txt")
				: string.Concat(asm.Location, ".log.txt");
			this.Writer = new TextWriter<TLogEntity>(storageName);
			if (LoadLastLogOnInitialize)
				this.Writer.LoadLastLog();
		}

		#region IStorageReadable Members
		public string StorageFilePath
		{
			get { return this.Writer.Log.FullName; }
		}
		#endregion

		private void Initialize(DirectoryInfo logPath, bool useLogRotation)
		{
			var asm = Utilities.GetCaller();
			var storageName = useLogRotation
				? Utilities.GernerateFileSpec(new DirectoryInfo(string.Concat(logPath != null ? logPath.FullName : new FileInfo(asm.Location).Directory.FullName, "\\Logs")),
					asm.FullName.Substring(0, asm.FullName.IndexOf(',')),
					".log.txt")
				: string.Concat(logPath != null ? logPath.FullName + asm.FullName.Substring(0, asm.FullName.IndexOf(',')) + ".log.txt" : asm.Location, ".log.txt");
			this.Writer = new TextWriter<TLogEntity>(storageName);
			if (LoadLastLogOnInitialize)
				this.Writer.LoadLastLog();
		}
	}
}
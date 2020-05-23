#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.IO;
using Library40.Helpers;
using Library40.LogSystem.Entities;
using Library40.LogSystem.Internals;

namespace Library40.LogSystem.FileBased.Text
{
	public class TextWriter<TLogEntity> : Writer<FileInfo, TLogEntity>
		where TLogEntity : LogEntity
	{
		public TextWriter(string textLogFileSpec)
			: base(new FileInfo(textLogFileSpec))
		{
		}

		public string LogFormat { get; set; }

		private string FormatLogEntity(TLogEntity logEntity)
		{
			if (string.IsNullOrEmpty(this.LogFormat))
				return logEntity.ToString();

			var result = this.LogFormat;

			ObjectHelper.ReflectProperties(logEntity).ForEach(prop => result = result.Replace(prop.Key, prop.Value != null ? prop.Value.ToString() : ""));
			return result;
		}

		protected override void InnerWrite(TLogEntity logEntity)
		{
			lock (this)
			{
				this.Log.Refresh();
				if (!this.Log.Directory.Exists)
				{
					this.Log.Directory.Create();
					this.Log.Create().Close();
				}
				if (!this.Log.Exists)
					this.Log.Create().Close();
				using (var writer = this.Log.AppendText())
				{
					writer.WriteLine(this.FormatLogEntity(logEntity));
					writer.Close();
				}
			}
		}

		public override void LoadLastLog()
		{
			//throw new NotSupportedException("This operation is not supported.");
		}
	}
}
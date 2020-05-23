using System.IO;
using Mohammad.Helpers;
using Mohammad.Logging.Entities;
using Mohammad.Logging.Internals;

namespace Mohammad.Logging.FileBased.Text
{
    public class FileWriter<TLogEntity> : Writer<FileInfo, TLogEntity>
        where TLogEntity : LogEntity
    {
        public string LogFormat { get; set; }

        public FileWriter(string textLogFileSpec)
            : base(new FileInfo(textLogFileSpec)) { }

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
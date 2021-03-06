using System.IO;
using Mohammad.Helpers;
using Mohammad.Logging.Entities;
using Mohammad.Logging.Internals;

namespace Mohammad.Logging.FileBased.Text
{
    public class FileWriter<TLogEntity> : Writer<FileInfo, TLogEntity>
        where TLogEntity : LogEntity
    {
        public FileWriter(string textLogFileSpec)
            : base(new FileInfo(textLogFileSpec))
        {
        }

        public string LogFormat { get; set; }

        public override void LoadLastLog()
        {
            //throw new NotSupportedException("This operation is not supported.");
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
                {
                    this.Log.Create().Close();
                }

                using (var writer = this.Log.AppendText())
                {
                    writer.WriteLine(this.FormatLogEntity(logEntity));
                    writer.Close();
                }
            }
        }

        private string FormatLogEntity(TLogEntity logEntity)
        {
            if (string.IsNullOrEmpty(this.LogFormat))
            {
                return logEntity.ToString();
            }

            var result = this.LogFormat;

            ObjectHelper.GetProperties(logEntity).ForEach(prop => result = result.Replace(prop.Name, prop.Value != null ? prop.Value.ToString() : ""));
            return result;
        }
    }
}
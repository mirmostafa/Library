using System.IO;
using Mohammad.Logging.Entities;
using Mohammad.Logging.Internals;

namespace Mohammad.Logging.FileBased.Xml
{
    /// <summary>
    ///     Facade for writing a log entry(es) optimized for XML log writer.
    /// </summary>
    public class XmlLogger<TLogEntity> : Logger<XmlWriter<TLogEntity>, TLogEntity>, IStorageReadable
        where TLogEntity : LogEntity, new()
    {
        public XmlLogger(DirectoryInfo logPath = null, bool useLogRotation = false) { this.Initialize(logPath, useLogRotation); }

        public override string ToString() { return this.StorageFilePath; }

        private void Initialize(DirectoryInfo logPath, bool useLogRotation)
        {
            var asm = Utilities.GetCaller();
            var storageName = useLogRotation
                ? Utilities.GernerateFileSpec(
                    new DirectoryInfo(Path.Combine(logPath != null ? logPath.FullName : new FileInfo(asm.Location).Directory.FullName, "Logs")),
                    asm.FullName.Substring(0, asm.FullName.IndexOf(',')),
                    ".log.xml")
                : string.Concat(logPath != null ? Path.Combine(logPath.FullName, asm.FullName.Substring(0, asm.FullName.IndexOf(','))) : asm.Location, ".log.xml");
            this.Writer = new XmlWriter<TLogEntity>(storageName);
            if (LoadLastLogOnInitialize)
                this.Writer.LoadLastLog();
        }

        #region IStorageReadable Members

        public string StorageFilePath { get { return this.Writer.Log.FullName; } }

        #endregion
    }
}
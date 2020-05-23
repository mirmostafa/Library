using System;
using Mohammad.IO;
using Mohammad.Logging.Entities;
using Mohammad.Logging.Internals;

namespace Mohammad.Logging.FileBased.Safe
{
    public class SecureFileWriter : IWriter<LogEntity>
    {
        public SecureFile Storage { get; }

        public SecureFileWriter(string key)
            : this(Utilities.GenerateLogFileSpec(), key) { }

        public SecureFileWriter(string fileName, string key) { this.Storage = new SecureFile(fileName, key); }
        public override string ToString() { return this.Storage.ToString(); }

        #region IWriter<LogEntity> Members

        public bool ShowInDebuggerTracer { get; set; }

        public void Write(LogEntity logEntity) { this.Storage.Write(logEntity.ToString()); }

        public void LoadLastLog() { throw new NotImplementedException(); }

        #endregion
    }
}
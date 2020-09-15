using System;
using Mohammad.IO;
using Mohammad.Logging.Entities;
using Mohammad.Logging.Internals;

namespace Mohammad.Logging.FileBased.Safe
{
    public class SecureFileWriter : IWriter<LogEntity>
    {
        public SecureFileWriter(string key)
            : this(Utilities.GenerateLogFileSpec(), key)
        {
        }

        public SecureFileWriter(string fileName, string key) => this.Storage = new SecureFile(fileName, key);
        public SecureFile Storage { get; }

        public bool ShowInDebuggerTracer { get; set; }

        public void Write(LogEntity logEntity)
        {
            this.Storage.Write(logEntity.ToString());
        }

        public void LoadLastLog()
        {
            throw new NotImplementedException();
        }

        public override string ToString() => this.Storage.ToString();
    }
}
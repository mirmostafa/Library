using System.IO;
using Mohammad.Logging.Entities;
using Mohammad.Logging.Internals;

namespace Mohammad.Logging.FileBased.Text
{
    /// <summary>
    ///     Facade for writing a log entry(es) optimized for text log writer. This class cannot be inherited.
    /// </summary>
    public class FileLogger<TLogEntity> : Logger<FileWriter<TLogEntity>, TLogEntity>, IStorageReadable
        where TLogEntity : LogEntity, new()
    {
        protected FileLogger(FileWriter<TLogEntity> writer, bool raiseEventOnly = false)
            : base(writer, raiseEventOnly) {}

        public static FileLogger<TLogEntity> Initialize(bool useLogRotation = false) { return InitializeByDirectory(); }

        public static FileLogger<TLogEntity> InitializeByDirectory(DirectoryInfo logPath = null, bool useLogRotation = false)
        {
            var result = new FileLogger<TLogEntity>(null);
            var asm = Utilities.GetCaller();
            var storageName = useLogRotation
                ? Utilities.GernerateFileSpec(
                    new DirectoryInfo(string.Concat(logPath != null ? logPath.FullName : new FileInfo(asm.Location).Directory.FullName, "\\Logs")),
                    asm.FullName.Substring(0, asm.FullName.IndexOf(',')),
                    ".log.txt")
                : string.Concat(logPath != null ? logPath.FullName + asm.FullName.Substring(0, asm.FullName.IndexOf(',')) + ".log.txt" : asm.Location, ".log.txt");
            result.Writer = new FileWriter<TLogEntity>(storageName);
            if (LoadLastLogOnInitialize)
                result.Writer.LoadLastLog();
            return result;
        }

        public static FileLogger<TLogEntity> InitializeByFile(string logFile, bool useLogRotation = false)
        {
            return InitializeByFile(new FileInfo(logFile), useLogRotation);
        }

        public static FileLogger<TLogEntity> InitializeByFile(bool useLogRotation = false) { return InitializeByFile((FileInfo) null, useLogRotation); }

        public static FileLogger<TLogEntity> InitializeByFile(FileInfo logFile, bool useLogRotation = false)
        {
            var result = new FileLogger<TLogEntity>(null);
            var asm = Utilities.GetCaller();
            var storageName = useLogRotation
                ? Utilities.GernerateFileSpec(
                    new FileInfo(string.Concat(logFile != null ? logFile.Directory.FullName : new FileInfo(asm.Location).Directory.FullName, "\\Logs")),
                    asm.FullName.Substring(0, asm.FullName.IndexOf(',')))
                : (logFile != null ? logFile.FullName : string.Concat(asm.Location, ".log.txt"));
            result.Writer = new FileWriter<TLogEntity>(storageName);
            if (LoadLastLogOnInitialize)
                result.Writer.LoadLastLog();
            return result;
        }

        public string StorageFilePath { get { return this.Writer.Log.FullName; } }
    }
}
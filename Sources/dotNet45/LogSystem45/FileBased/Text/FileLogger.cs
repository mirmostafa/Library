using Mohammad.Logging.Entities;

namespace Mohammad.Logging.FileBased.Text
{
    /// <summary>
    ///     Facade for writing a log entry(es) optimized for XML log writer. This class cannot be inherited.
    /// </summary>
    public sealed class FileLogger : FileLogger<LogEntity>
    {
        public FileLogger(FileWriter<LogEntity> writer, bool raiseEventOnly)
            : base(writer, raiseEventOnly) { }

        public FileLogger(FileWriter<LogEntity> writer)
            : base(writer) { }
    }
}
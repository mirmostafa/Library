using System.IO;
using Mohammad.Logging.Entities;

namespace Mohammad.Logging.FileBased.Xml
{
    /// <summary>
    ///     Facade for writing a log entry(es) optimized for XML log writer. This class cannot be inherited.
    /// </summary>
    public sealed class XmlLogger : XmlLogger<LogEntity>
    {
        public XmlLogger(DirectoryInfo logPath = null, bool useLogRotation = false)
            : base(logPath, useLogRotation) { }
    }
}
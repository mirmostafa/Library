using System;
using System.IO;

namespace Mohammad.Logging
{
    public sealed class FileLogSplitInfo
    {
        private Func<FileInfo, bool> _IsValid = DefaultIsValid;
        private long _MaxLogFileSize = DefaultMaxLogFileSize;
        public Func<string, FileInfo> GetNextFile = DefaultIsGetNextFile;
        public static readonly Func<FileInfo, bool> DefaultIsValid = f => f.Length < DefaultMaxLogFileSize;

        public static readonly Func<string, FileInfo> DefaultIsGetNextFile = f =>
        {
            if (!f.Contains("{0}"))
                f = string.Concat(Path.GetDirectoryName(f), "\\", Path.GetFileNameWithoutExtension(f), "{0}", Path.GetExtension(f));
            var index = 1;
            while (File.Exists(string.Format(f, index)))
                index++;
            var result = new FileInfo(string.Format(f, index));
            result.CreateText().Close();
            return result;
        };

        public static readonly long DefaultMaxLogFileSize = 2 * 1024 * 1024;
        public bool IsEnabled { get; set; } = true;
        public long MaxLogFileSize { get { return this._MaxLogFileSize; } set { this._MaxLogFileSize = value == 0 ? DefaultMaxLogFileSize : value; } }
        public Func<FileInfo, bool> IsValid { get { return this._IsValid; } set { this._IsValid = value ?? DefaultIsValid; } }
    }
}
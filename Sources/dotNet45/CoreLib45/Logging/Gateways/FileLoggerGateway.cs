﻿using System;
using System.IO;
using System.Text;
using Mohammad.Helpers;

namespace Mohammad.Logging.Gateways
{
    public class FileLoggerGateway : TextWriter, ILogRotator
    {
        private readonly string _FilePath;
        private string _CompleteFilePath;

        public FileLoggerGateway(string filePath = null)
        {
            var fullPath = Path.GetFullPath(filePath.IfNullOrEmpty(Path.Combine(Environment.CurrentDirectory, ApplicationHelper.ApplicationTitle + ".log")));
            if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            }

            this._FilePath = fullPath;
        }

        public override Encoding Encoding => Encoding.UTF8;

        public bool IsLogRotationEnabled { get; set; }

        public override void WriteLine()
        {
            lock (this)
            {
                this.ManageLogFile();
                File.AppendAllText(this._CompleteFilePath, Environment.NewLine);
            }
        }

        public override void WriteLine(string value)
        {
            lock (this)
            {
                this.ManageLogFile();
                File.AppendAllText(this._CompleteFilePath, string.Concat(value, Environment.NewLine));
            }
        }

        private void ManageLogFile() => this._CompleteFilePath = this.IsLogRotationEnabled
            ? Path.Combine(Path.GetDirectoryName(this._FilePath),
                string.Concat(Path.GetFileNameWithoutExtension(this._FilePath),
                    "_",
                    DateTime.Now.ToShortDateString().Replace("/", "-"),
                    Path.GetExtension(this._FilePath)))
            : this._FilePath;
    }
}
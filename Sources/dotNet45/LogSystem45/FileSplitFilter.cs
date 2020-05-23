using System;
using System.IO;
using System.Text;
using Mohammad.Helpers;
using Mohammad.Logging.EventsLogs;

namespace Mohammad.Logging
{
    public class FileSplitFilter : TextWriter, IFilter
    {
        private FileInfo _CurrentLogFile;
        public DirectoryInfo LeafLogDirectory { get; }
        public FileLogSplitInfo Split { get; } = new FileLogSplitInfo();

        public FileInfo CurrentLogFile
        {
            get { return this._CurrentLogFile ?? (this._CurrentLogFile = this.Split.GetNextFile(this.GetFileName())); }
            private set { this._CurrentLogFile = value; }
        }

        public override Encoding Encoding { get { return Encoding.UTF8; } }
        public string LogFileName { get; set; }

        public FileSplitFilter(string leafLogDirectory)
            : this(new DirectoryInfo(leafLogDirectory)) {}

        public FileSplitFilter(DirectoryInfo leafLogDirectory)
        {
            if (leafLogDirectory == null)
                throw new ArgumentNullException("leafLogDirectory");
            if (!leafLogDirectory.Exists)
                leafLogDirectory.Create();
            this.LeafLogDirectory = leafLogDirectory;
            this.LogFileName = "Log.txt";
        }

        private string GetFileName() { return Path.Combine(this.LeafLogDirectory.FullName, this.LogFileName); }

        private string FormatLog(string text)
        {
            var args = new TextFormatingEventArgs(text);
            this.OnTextFormating(args);
            return args.Text;
        }

        public event EventHandler<TextFormatingEventArgs> TextFormating;
        protected virtual void OnTextFormating(TextFormatingEventArgs e) { this.TextFormating.Raise(this, e); }

        public override void WriteLine(string value)
        {
            if (!this.Split.IsEnabled)
                return;
            this.CurrentLogFile.Refresh();
            if (!this.Split.IsValid(this.CurrentLogFile))
                this.CurrentLogFile = this.Split.GetNextFile(this.GetFileName());
            File.AppendAllText(this.CurrentLogFile.FullName, string.Concat(this.FormatLog(value), Environment.NewLine));
            if (this.Out != null)
                this.Out.WriteLine(value);
        }

        public TextWriter Out { get; set; }
    }
}
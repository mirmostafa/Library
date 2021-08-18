using Library.EventsArgs;

namespace Library.Logging
{
    public sealed class FormattedStringFastLogger : FastLoggerBase<object>, ILogger
    {
        private string _Format;

        public event EventHandler<ItemActedEventArgs<string>> Logged;
        public string Format { get => this._Format.IfNullOrEmpty(LogFormat.DEFAULT_FORMAT); set => this._Format = value; }

        public FormattedStringFastLogger(string? format = null)
            => this.Format = format ?? LogFormat.DEFAULT_FORMAT;

        protected override void OnLogging(LogRecord<object> logRecord)
        {
            if (logRecord is not null)
            {
                Logged?.Invoke(this, new(logRecord.Reformat(this._Format)));
            }
        }
    }
}
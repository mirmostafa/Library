﻿using Library.Coding;
using Library.Validations;

namespace Library.Logging
{
    public sealed class TextWriterLogger : FastLoggerBase<string>, ILogger
    {
        public TextWriterLogger()
        {
        }

        public TextWriterLogger(IEnumerable<TextWriter> writers)
            => this.Writers.AddRange(ToAction(writers.ArgumentNotNull(nameof(writers)).Compact()));
        public TextWriterLogger(params TextWriter[] writers)
            => this.Writers.AddRange(ToAction(writers.ArgumentNotNull(nameof(writers)).Compact()));

        public TextWriterLogger(IEnumerable<Action<string>> writers)
            => this.Writers.AddRange(writers.ArgumentNotNull(nameof(writers)).Compact());
        public TextWriterLogger(params Action<string>[] writers)
            => this.Writers.AddRange(writers.ArgumentNotNull(nameof(writers)).Compact());

        private List<Action<string>> Writers { get; } = new();

        private static Action<string> ToAction(TextWriter writer)
            => s => writer.WriteLine(s);

        private static IEnumerable<Action<string>> ToAction(IEnumerable<TextWriter> writers)
            => writers.Select(ToAction);

        protected override void OnLogging(LogRecord<string> logRecord)
        {
            var message = logRecord.Reformat();
            foreach (var writer in this.Writers.Compact())
            {
                _ = Catch(() => writer(message));
            }
        }

        public TextWriterLogger Add(params TextWriter[] writers)
        {
            this.Writers.AddRange(ToAction(writers.ArgumentNotNull(nameof(writers)).Compact()));
            return this;
        }

        public TextWriterLogger Add(params Action<string>[] writers)
        {
            this.Writers.AddRange(writers.ArgumentNotNull(nameof(writers)).Compact());
            return this;
        }

        public void Log(object message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
            => base.Log(message?.ToString() ?? string.Empty, level, sender, time, stackTrace);
    }
}

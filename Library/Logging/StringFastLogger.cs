﻿using Library.EventsArgs;

namespace Library.Logging
{
    public class StringFastLogger : FastLoggerBase<string>
    {
        public event EventHandler<ItemActedEventArgs<LogRecord<string>>> Logging;
        protected override void OnLogging(LogRecord<string> logRecord)
            => Logging?.Invoke(this, new(logRecord));
    }
}

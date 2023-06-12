using Library.EventsArgs;
using Library.Extensions.Options;

namespace Library.Logging;

public sealed class FormattedStringFastLogger : FastLoggerBase<object>, IConfigurableLogger<FormattedStringFastLogger, FormattedStringFastLogger.Options>
{
    private readonly Options _options = new();

    public event EventHandler<ItemActedEventArgs<string>>? Logged;

    public FormattedStringFastLogger Configure(Action<Options> configure)
    {
        configure(this._options);
        return this;
    }

    protected override void OnLogging(LogRecord<object> logRecord)
    {
        if (logRecord is not null)
        {
            var log = !logRecord.Format.IsNullOrEmpty() ? logRecord : logRecord with { Format = this._options.LogFormat };
            Logged?.Invoke(this, new(log.Reformat()));
        }
    }

    public class Options : IOptions
    {
        public string LogFormat { get; set; } = Logging.LogFormat.FORMAT_DEFAULT;
    }
}
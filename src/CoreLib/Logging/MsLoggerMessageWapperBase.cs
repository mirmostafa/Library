using Library.Extensions.Options;
using Library.Helpers;
using Library.Validations;

using Microsoft.Extensions.Logging;

namespace Library.Logging;

public sealed class MsLoggerMessageWrapperOptions : IOptions { }

public abstract class MsLoggerMessageWrapperBase<TMsLoggerMessageWapper> : IConfigurable<TMsLoggerMessageWapper, MsLoggerMessageWrapperOptions>
    where TMsLoggerMessageWapper : MsLoggerMessageWrapperBase<TMsLoggerMessageWapper>
{
    protected readonly IMsLogger _logger;

    protected readonly Action<IMsLogger, string, Exception?> _info;
    protected readonly Action<IMsLogger, string, Exception?> _debug;
    protected readonly Action<IMsLogger, string, Exception?> _error;
    protected readonly Action<IMsLogger, string, Exception?> _warning;

    protected MsLoggerMessageWrapperBase(IMsLogger logger, string name, int eventId)
    {
        this._logger = logger;
        this._info = LoggerMessage.Define<string>(MsLogLevel.Information, new EventId(eventId, name), "{Log}");
        this._debug = LoggerMessage.Define<string>(MsLogLevel.Debug, new EventId(eventId, name), "{Log}");
        this._error = LoggerMessage.Define<string>(MsLogLevel.Error, new EventId(eventId, name), "{Log}");
        this._warning = LoggerMessage.Define<string>(MsLogLevel.Warning, new EventId(eventId, name), "{Log}");
    }

    private MsLoggerMessageWrapperOptions Options { get; } = new();

    public TMsLoggerMessageWapper Configure(Action<MsLoggerMessageWrapperOptions> configure)
    {
        configure.ArgumentNotNull()(this.Options);
        return this.Cast().As<TMsLoggerMessageWapper>()!;
    }

    public virtual void Debug(string log) => this._debug(this._logger, log, null);

    public virtual void Error(string log, Exception? exception = null) => this._error(this._logger, log, exception);

    public virtual void Info(string log) => this._info(this._logger, log, null);

    public virtual void Warn(string log) => this._warning(this._logger, log, null);
}
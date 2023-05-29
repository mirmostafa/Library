using Library.Validations;

namespace Library.Logging;

/// <summary>
/// FastLoggerBase is an abstract class that implements the ILogger interface and provides a base implementation for logging.
/// </summary>
public abstract class FastLoggerBase<TLogMessage> : ILogger<TLogMessage>
{
    private readonly Action<LogRecord<TLogMessage>> _emptyAction = _ => { };
    private readonly Action<LogRecord<TLogMessage>> _logAction;
    private Action<LogRecord<TLogMessage>> _debugAction = null!;
    private Action<LogRecord<TLogMessage>> _errorAction = null!;
    private Action<LogRecord<TLogMessage>> _fatalAction = null!;
    private Action<LogRecord<TLogMessage>> _infoAction = null!;
    private bool _isEnabled = true;
    private LogLevel _logLevel = LogLevel.Normal;
    private Action<LogRecord<TLogMessage>> _traceAction = null!;
    private Action<LogRecord<TLogMessage>> _warnAction = null!;

    protected FastLoggerBase()
    {
        this._logAction = this.Log;
        this.IsEnabled = true;
        this.LogLevel = LogLevel.Normal;
    }

    public bool IsEnabled
    {
        get => this._isEnabled;
        set
        {
            this._isEnabled = value;
            this.ResetAction();
        }
    }

    public LogLevel LogLevel
    {
        get => this._logLevel;
        set
        {
            this._logLevel = value;
            this.ResetAction();
        }
    }

    public void Debug(TLogMessage message, object? sender = null, DateTime? time = null)
        => this._debugAction(new(message, LogLevel.Debug, ProcessSender(sender, LogLevel.Debug), time));

    public void Error(TLogMessage message, object? sender = null, DateTime? time = null)
        => this._errorAction(new(message, LogLevel.Error, ProcessSender(sender, LogLevel.Error), time));

    public void Fatal(TLogMessage message, object? sender = null, DateTime? time = null)
        => this._fatalAction(new(message, LogLevel.Fatal, ProcessSender(sender, LogLevel.Fatal), time));

    public void Info(TLogMessage message, object? sender = null, DateTime? time = null)
        => this._infoAction(new(message, LogLevel.Info, ProcessSender(sender, LogLevel.Info), time));

    public void Log(TLogMessage message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
        => this.Log(new(message, level, ProcessSender(sender, level), time, stackTrace));

    public void Log(LogRecord<TLogMessage> logRecord)
    {
        if (!this.IsEnabled || !logRecord.ArgumentNotNull(nameof(logRecord)).Level.MeetsLevel(this.LogLevel) || (logRecord.Message is null))
        {
            return;
        }

        this.OnLogging(logRecord);
    }

    public void Trace(TLogMessage message, object? sender = null, DateTime? time = null, string? stackTrace = null)
        => this._traceAction(new(message, LogLevel.Trace, ProcessSender(sender, LogLevel.Trace), time, stackTrace));

    public void Warn(TLogMessage message, object? sender = null, DateTime? time = null)
        => this._warnAction(new(message, LogLevel.Warning, ProcessSender(sender, LogLevel.Warning), time));

    protected abstract void OnLogging(LogRecord<TLogMessage> logRecord);

    private static object? ProcessSender(object? sender, LogLevel level)
        => (sender, level) switch
        {
            (not null, _) => sender,
            (_, LogLevel.Debug or LogLevel.Trace) => CodeHelper.GetCallerMethod(3)?.Name,
            (_, _) => CodeHelper.GetCallerMethod(3)?.DeclaringType?.Name,
        };

    private void ResetAction()
    {
        this._traceAction = this._debugAction = this._errorAction = this._fatalAction = this._infoAction = this._warnAction = this._emptyAction;
        if (!this.IsEnabled)
        {
            return;
        }

        if (LogLevel.Trace.MeetsLevel(this.LogLevel))
        {
            this._traceAction = this._logAction;
        }
        if (LogLevel.Debug.MeetsLevel(this.LogLevel))
        {
            this._debugAction = this._logAction;
        }
        if (LogLevel.Info.MeetsLevel(this.LogLevel))
        {
            this._infoAction = this._logAction;
        }
        if (LogLevel.Error.MeetsLevel(this.LogLevel))
        {
            this._errorAction = this._logAction;
        }
        if (LogLevel.Fatal.MeetsLevel(this.LogLevel))
        {
            this._fatalAction = this._logAction;
        }
        if (LogLevel.Warning.MeetsLevel(this.LogLevel))
        {
            this._warnAction = this._logAction;
        }
    }
}
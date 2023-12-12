using Library.Collections;
using Library.DesignPatterns.ExceptionHandlingPattern;

namespace Library.Logging;

public abstract class LoggersBase<TMessage>(IEnumerable<ILogger<TMessage>> loggers, ExceptionHandling? exceptionHandling = null) :
    FluentListBase<ILogger<TMessage>, LoggersBase<TMessage>>(loggers),
    ILoggerCollection<TMessage, ILogger<TMessage>>,
    IExceptionHandlerContainer, ILogger<TMessage>
{
    protected LoggersBase(params ILogger<TMessage>[] loggers)
        : this(loggers.AsEnumerable(), null) { }

    protected LoggersBase(ExceptionHandling? exceptionHandling, params ILogger<TMessage>[] loggers)
        : this(loggers.AsEnumerable(), exceptionHandling) { }

    public ExceptionHandling ExceptionHandling { get; private set; } = exceptionHandling ?? new();
    public bool IsEnabled { get; set; } = true;
    bool ILogger<TMessage>.IsEnabled { get; set; }
    public IEnumerable<ILogger<TMessage>> Loggers => this.AsEnumerable();
    IEnumerable<ILogger<TMessage>> ILoggers<TMessage>.Loggers { get; } = [];
    public LogLevel LogLevel { get; set; } = LogLevel.Normal;
    LogLevel ILogger<TMessage>.LogLevel { get; set; }
    public virtual bool IsReadOnly => throw new NotSupportedException();

    public new void Add(ILogger<TMessage> item)
        => base.Add(item);

    public new void Clear()
        => base.Clear();

    public new bool Contains(ILogger<TMessage> item)
        => base.Contains(item).Result;

    public new void CopyTo(ILogger<TMessage>[] array, int arrayIndex)
        => base.CopyTo(array, arrayIndex);

    public void Log([DisallowNull] TMessage message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null, string? format = null)
    {
        if (!this.IsEnabled || !level.MeetsLevel(this.LogLevel))
        {
            return;
        }
        foreach (var logger in this.Loggers.Compact())
        {
            Catch(() => logger.Log(message, level, sender, time, stackTrace, format), this.ExceptionHandling);
        }
    }

    public new bool Remove(ILogger<TMessage> item)
        => base.Remove(item).Result;
}
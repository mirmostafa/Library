using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Library.Collections;
using Library.DesignPatterns.ExceptionHandlingPattern;

namespace Library.Logging;

public abstract class LoggersBase<TMessage> :
    FluentListBase<ILogger<TMessage>, LoggersBase<TMessage>>,
    ILoggerCollection<TMessage, ILogger<TMessage>>,
    IExceptionHandlerContainer, ILogger<TMessage>
{
    public IEnumerable<ILogger<TMessage>> Loggers => this.AsEnumerable();
    public bool IsEnabled { get; set; }
    public LogLevel LogLevel { get; set; }
    public ExceptionHandling ExceptionHandling { get; private set; }

    protected LoggersBase(IEnumerable<ILogger<TMessage>> loggers, ExceptionHandling? exceptionHandling = null)
        : base(loggers)
    {
        this.IsEnabled = true;
        this.LogLevel = LogLevel.Normal;
        this.ExceptionHandling = exceptionHandling ?? new();
    }

    protected LoggersBase(params ILogger<TMessage>[] loggers)
        : this(loggers.AsEnumerable(), null) { }

    protected LoggersBase(ExceptionHandling? exceptionHandling, params ILogger<TMessage>[] loggers)
        : this(loggers.AsEnumerable(), exceptionHandling) { }

    public new void Add(ILogger<TMessage> item)
        => base.Add(item);
    public new void Clear()
        => base.Clear();
    public new bool Contains(ILogger<TMessage> item)
        => base.Contains(item).Result;
    public new void CopyTo(ILogger<TMessage>[] array, int arrayIndex)
        => base.CopyTo(array, arrayIndex);
    public new bool Remove(ILogger<TMessage> item)
        => base.Remove(item).Result;

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();

    public void Log([DisallowNull] TMessage message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
    {
        if (!this.IsEnabled || !level.MeetsLevel(this.LogLevel))
        {
            return;
        }
        foreach (var logger in this.Loggers.Compact())
        {
            Catch(() => logger.Log(message, level, sender, time, stackTrace), this.ExceptionHandling);
        }
    }
}

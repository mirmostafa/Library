using System.Runtime.CompilerServices;

namespace Library.Logging;

/// <summary>
/// Interface for a generic logger
/// </summary>
public interface ILogger<TLogMessage>
{
    /// <summary>
    /// The empty logger
    /// </summary>
    public static readonly ILogger<TLogMessage> Empty = new EmptyLogger<TLogMessage>();

    /// <summary>
    /// Gets or sets a value indicating whether this instance is enabled.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
    /// </value>
    bool IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the log level.
    /// </summary>
    /// <value>
    /// The log level.
    /// </value>
    LogLevel LogLevel { get; set; }

    /// <summary>
    /// Logs the specified message with LogLevel.Debug.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    void Debug([DisallowNull] TLogMessage message, [CallerMemberName] object? sender = null, DateTime? time = null)
        => this.Log(message, LogLevel.Debug, sender: sender ?? CodeHelper.GetCallerMethod(), time ?? DateTime.Now);

    /// <summary>
    /// Logs the specified message with LogLevel.Error.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    void Error([DisallowNull] TLogMessage message, [CallerMemberName] object? sender = null, DateTime? time = null)
        => this.Log(message, LogLevel.Error, sender: sender ?? CodeHelper.GetCallerMethod(), time ?? DateTime.Now);

    /// <summary>
    /// Logs the specified message with LogLevel.Fatal.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    void Fatal([DisallowNull] TLogMessage message, [CallerMemberName] object? sender = null, DateTime? time = null)
        => this.Log(message, LogLevel.Fatal, sender: sender ?? CodeHelper.GetCallerMethod(), time ?? DateTime.Now);

    /// <summary>
    /// Logs the specified message with LogLevel.Info.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    void Info([DisallowNull] TLogMessage message, [CallerMemberName] object? sender = null, DateTime? time = null)
        => this.Log(message, LogLevel.Info, sender: sender ?? CodeHelper.GetCallerMethod(), time ?? DateTime.Now);

    /// <summary>
    /// Logs the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="level">The level.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    /// <param name="stackTrace">The stack trace.</param>
    void Log([DisallowNull] TLogMessage message, LogLevel level = LogLevel.Info, [CallerMemberName] object? sender = null, DateTime? time = null, string? stackTrace = null, string? format = LogFormat.FORMAT_DEFAULT);

    /// <summary>
    /// Logs the specified message with LogLevel.Trace.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    /// <param name="stackTrace">The stack trace.</param>
    void Trace([DisallowNull] TLogMessage message, [CallerMemberName] object? sender = null, DateTime? time = null, string? stackTrace = null)
        => this.Log(message, LogLevel.Trace, sender: sender ?? CodeHelper.GetCallerMethod(), time ?? DateTime.Now, stackTrace);

    /// <summary>
    /// logs the specified message with LogLevel.Warning.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    void Warn([DisallowNull] TLogMessage message, [CallerMemberName] object? sender = null, DateTime? time = null)
        => this.Log(message, LogLevel.Warning, sender: sender ?? CodeHelper.GetCallerMethod(), time ?? DateTime.Now);
}
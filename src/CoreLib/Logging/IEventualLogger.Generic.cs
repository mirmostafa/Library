using Library.EventsArgs;

namespace Library.Logging;

/// <summary>
/// Represents an interface for a logger that provides an event for when a log message is created.
/// </summary>
public interface IEventualLogger<TLogMessage> : ILogger<TLogMessage>
{
    /// <summary>
    /// Event raised when a log message is created.
    /// </summary>
    event EventHandler<ItemActedEventArgs<TLogMessage>>? Logging;
}
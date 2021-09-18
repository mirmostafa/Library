using Library.EventsArgs;

namespace Library.Logging;

public interface IEventualLogger<TLogMessage> : ILogger<TLogMessage>
{
    /// <summary>
    /// Occurs when [logging].
    /// </summary>
    event EventHandler<ItemActedEventArgs<TLogMessage>>? Logging;
}

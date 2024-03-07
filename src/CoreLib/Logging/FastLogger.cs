using Library.EventsArgs;

namespace Library.Logging;

/// <summary>
/// Represents a class that provides logging functionality and raises an event when logging.
/// </summary>
public sealed class FastLogger : FastLoggerBase<object>, IEventualLogger
{
}
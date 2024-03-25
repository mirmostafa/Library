namespace Library.Logging;

/// <summary>
/// Interface for a logger that can log objects of any type. Inherits from IEventualLogger<object> and ILogger.
/// </summary>
public interface IEventualLogger : IEventualLogger<object>, ILogger
{
}
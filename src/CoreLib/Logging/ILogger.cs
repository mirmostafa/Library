namespace Library.Logging;

/// <summary>
/// Interface for a logger
/// </summary>
public interface ILogger : ILogger<object>
{
    /// <summary>
    /// Creates a new instance of the EmptyLogger class.
    /// </summary>
    public static new readonly ILogger Empty = new EmptyLogger();
}
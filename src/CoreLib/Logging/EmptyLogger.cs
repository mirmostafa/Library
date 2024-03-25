namespace Library.Logging;

/// <summary>
/// Represents an empty logger that does nothing.
/// </summary>
/// <returns>An instance of the EmptyLogger class.</returns>
public sealed class EmptyLogger : EmptyLogger<object>, ILogger
{
    /// <summary>
    /// Gets an empty logger instance.
    /// </summary>
    /// <returns>An empty logger instance.</returns>
    public static EmptyLogger Empty => new();
}
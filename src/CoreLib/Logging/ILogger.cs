using Library.Extensions.Options;

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

public interface IConfigurableLogger<TSelf, TOptions> : ILogger, IConfigurable<TSelf, TOptions>
    where TSelf : IConfigurableLogger<TSelf, TOptions>
    where TOptions : IOptions
{
}
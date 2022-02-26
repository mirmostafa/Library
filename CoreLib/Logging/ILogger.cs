using Library.Interfaces;

namespace Library.Logging;

public interface ILogger : ILogger<object>
{
    /// <summary>
    /// The empty Logger
    /// </summary>
    static new readonly ILogger Empty = new EmptyLogger();
}

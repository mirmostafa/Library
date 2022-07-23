namespace Library.Logging;

public interface ILogger : ILogger<object>
{
    /// <summary>
    /// The empty Logger
    /// </summary>
    public static new readonly ILogger Empty = new EmptyLogger();
}

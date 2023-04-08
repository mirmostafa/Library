namespace Library.Logging;

public sealed class EmptyLogger : EmptyLogger<object>, ILogger
{
    public static EmptyLogger Empty => new();
}

namespace Library.Logging;

internal class EmptyLogger : EmptyLogger<object>, ILogger
{
    public static EmptyLogger Empty = new();
}

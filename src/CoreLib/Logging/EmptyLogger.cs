namespace Library.Logging;

public class EmptyLogger : EmptyLogger<object>, ILogger
{
    public static EmptyLogger Empty => new();
}

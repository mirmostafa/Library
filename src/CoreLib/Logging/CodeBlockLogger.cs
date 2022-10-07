namespace Library.Logging;

public sealed class CodeBlockLogger : IDisposable
{
    private readonly string? _end;
    private readonly ILogger _logger;

    private CodeBlockLogger(ILogger logger, string start, string end)
    {
        this._logger = logger;
        this._end = end;
        this._logger.Debug(start);
    }

    public static IDisposable New(ILogger logger, string start, string end)
        => new CodeBlockLogger(logger, start, end);

    public void Dispose()
        => this._end.IsNullOrEmpty().IfFalse(() => this._logger.Info(this._end!));
}
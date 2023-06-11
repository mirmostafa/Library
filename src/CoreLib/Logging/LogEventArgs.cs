namespace Library.Logging;

public class LogEventArgs(object log, object? moreInfo = null, LogLevel level = LogLevel.Info) : EventArgs
{
    public LogLevel Level { get; set; } = level;

    public object Log { get; set; } = log;

    public string? MemberName { get; set; }

    public object? MoreInfo { get; set; } = moreInfo;

    public object? Sender { get; set; }

    public string? SourceFilePath { get; set; }

    public int? SourceLineNumber { get; set; }
}
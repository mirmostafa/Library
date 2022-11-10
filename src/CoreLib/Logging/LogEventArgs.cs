namespace Library.Logging;

public class LogEventArgs : EventArgs
{
    public LogEventArgs(object log, object? moreInfo = null, LogLevel level = LogLevel.Info)
    {
        this.Log = log;
        this.MoreInfo = moreInfo;
        this.Level = level;
    }

    public LogLevel Level { get; set; }

    public object Log { get; set; }

    public string? MemberName { get; set; }

    public object? MoreInfo { get; set; }

    public object? Sender { get; set; }

    public string? SourceFilePath { get; set; }

    public int? SourceLineNumber { get; set; }
}
namespace Library.Logging;

public readonly struct LogFormat
{
    public const string LONG_DATE = "longDate";
    public const string SHORT_TIME = "shortTime";
    public const string LEVEL = "level";
    public const string SENDER = "sender";
    public const string MESSAGE = "message";
    public const string STACK_TRACE = "stackTrace";
    public const string NEW_LINE = "newLine";
    public const string FORMAT_DEFAULT = $"{LONG_DATE}|{LEVEL}|{SENDER}|{MESSAGE}";
    public const string FORMAT_SHORT   = $"{SHORT_TIME}|{SENDER}|{MESSAGE}";
}

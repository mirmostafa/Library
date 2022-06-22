namespace Library.Logging;

public readonly struct LogFormat
{
    public const string DATE = "longDate";
    public const string LEVEL = "level";
    public const string SENDER = "sender";
    public const string MESSAGE = "message";
    public const string STACK_TRACE = "stackTrace";
    public const string NEW_LINE = "newLine";
    public const string DEFAULT_FORMAT = "longDate|level|sender|message";
}

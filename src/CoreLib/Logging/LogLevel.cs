namespace Library.Logging;

[Flags]
public enum LogLevel
{
    None = 0,
    Info = 15,
    Warning = 7,
    Error = 3,
    Fatal = 1,
    Debug = 31,
    Trace = 63,
    Verbose = Trace,
    ErrorsOnly = Error | Fatal,
    Normal = Info | Warning | ErrorsOnly,
}
using Library.Logging;
using System.Runtime.CompilerServices;

namespace ConAppTest;

public static class ConsoleAppStartupModule
{
    public static ILogger Logger { get; private set; } = null!;

    [ModuleInitializer]
    public static void Startup()
    {
        Loggers loggers = new();
        loggers.Add(new VsOutputLogger());
        loggers.Add(new TextWriterLogger(Console.Out));
        Logger = loggers;
    }
}
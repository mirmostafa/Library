using System.Runtime.CompilerServices;
using Library.Logging;

namespace ConAppTest;

public static class ConsoleAppStartupModule
{
    public static ILogger Logger { get; private set; } = null!;

    [ModuleInitializer]
    public static void Startup()
        => Logger = new Loggers()
        {
            new VsOutputLogger(),
            new TextWriterLogger(Console.Out)
        };
}
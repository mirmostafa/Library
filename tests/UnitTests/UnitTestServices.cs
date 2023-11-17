using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using Library.Logging;

namespace UnitTests;

[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
public static class UnitTestServices
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
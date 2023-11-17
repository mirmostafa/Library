﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

using Library.Logging;

namespace UnitTests;

[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
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
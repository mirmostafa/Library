using Library.Logging;

namespace ConAppTest;

public abstract class ProgramBase
{
    public static ILogger Logger { get; } = ConsoleAppStartupModule.Logger;
}

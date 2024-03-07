using System.Collections;

using Library.Logging;
using Library.Validations;

namespace Library.Helpers.ConsoleHelper;

public static class ConsoleHelper
{
    public static void Initialize(Action<ConsoleHelperOptions>? configurator = null)
    {
        LibLogger.Debug("Configuring...", nameof(ConsoleHelper));
        var options = new ConsoleHelperOptions();
        configurator?.Invoke(options);
        if (options.RedirectConsoleOut)
        {
            Console.SetOut(new ConsoleTextWriter(Console.Out));
        }
        if (options.RedirectLibLogger)
        {
            LibLogger.AddConsole(format:$"{LogFormat.MESSAGE}");
        }
        LibLogger.Debug("Configured.", nameof(ConsoleHelper));
    }

    public static T DumpListLine<T>(T t)
        where T : IEnumerable
    {
        foreach (var item in t)
        {
            Console.WriteLine(item);
        }
        return t;
    }

    public static T DumpLine<T>(T t)
    {
        if (t == null)
        {
            System.Console.WriteLine();
            return t;
        }
        if (t is IEnumerable items)
        {
            foreach (var item in items)
            {
                _ = DumpLine(item);
                System.Console.WriteLine();
            }
            return t;
        }

        var props = ObjectHelper.GetProperties(t);
        var maxLength = props.Select(x => x.Name.Length).Max() + 1;
        foreach (var (name, value) in props)
        {
            _ = WriteLine($"{StringHelper.FixSize(name, maxLength)}: {value}");
        }

        return t;
    }

    public static T WriteLine<T>(this T t)
    {
        if (t is IEnumerable items and not string)
        {
            foreach (var item in items)
            {
                _ = WriteLine(item);
            }
            return t;
        }

        Console.WriteLine(t);
        return t;
    }

    private sealed class ConsoleTextWriter([DisallowNull] TextWriter consoleOut) : TextWriter
    {
        public override Encoding Encoding { get; } = consoleOut.ArgumentNotNull().Encoding;

        public override void WriteLine()
            => this.InnerWrite();

        public override void WriteLine(string? text)
            => this.InnerWrite(text);

        private void InnerWrite(string? text = null)
        {
            if (text == null)
            {
                consoleOut.WriteLine();
            }
            else
            {
                //consoleOut.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {text}");
                consoleOut.WriteLine($"{text}");
            }
        }
    }
}

public sealed class ConsoleHelperOptions 
{
    internal ConsoleHelperOptions()
    {
    }

    internal bool RedirectConsoleOut { get; set; } = true;
    internal bool RedirectLibLogger { get; set; } = true;

    public ConsoleHelperOptions SetRedirectConsoleOut(bool redirectConsoleOut = true)
        => this.With(x => x.RedirectConsoleOut = redirectConsoleOut);

    public ConsoleHelperOptions SetRedirectLibLogger(bool redirectLibLogger = true)
        => this.With(x => x.RedirectLibLogger = redirectLibLogger);
}
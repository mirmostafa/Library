using System.Text;

namespace UnitTests;

[Trait("Category", nameof(Library.Helpers.ConsoleHelper))]
internal class ConsoleOutputRedirector : IDisposable
{
    private TextWriter? _lastWriter;
    private WriterCapturer? _writer;
    private WriterCapturerLazy? _writerLazy;

    public static ConsoleOutputRedirector CreateAndStart(Action<string?> callback)
    {
        var result = new ConsoleOutputRedirector();
        result.Start(callback);
        return result;
    }

    public void Dispose() => this.StopCapturing();

    public void Start(Action<string?> callback)
    {
        this.StopCapturing();
        this._writer = new(x => callback(x?.ToString()));
        this._lastWriter = Console.Out;
        Console.SetOut(this._writer);
    }

    public void StartLazy(Action<string?> callback)
    {
        this.StopCapturing();
        this._writerLazy = new(x => callback(x?.ToString()));
        this._lastWriter = Console.Out;
        Console.SetOut(this._writerLazy);
    }

    public void StopCapturing()
    {
        this._writer?.Flush();
        this._writer?.Dispose();
        this._writer = null;
        if (this._lastWriter != null)
        {
            Console.SetOut(this._lastWriter);
        }
    }

    public void StopLazyCapturing()
    {
        this._writerLazy?.Flush();
        this._writerLazy?.Dispose();
        this._writerLazy = null;
        if (this._lastWriter != null)
        {
            Console.SetOut(this._lastWriter);
        }
    }

    private class WriterCapturer(Action<object?> redirect) : TextWriter
    {
        private readonly Action<object?> _redirect = redirect;

        public override Encoding Encoding { get; } = Encoding.UTF8;

        public override void WriteLine(object? value)
            => this._redirect(value);

        public override void WriteLine(string? value)
            => this._redirect(value);
    }

    private class WriterCapturerLazy(Action<object?> redirect) : TextWriter
    {
        private readonly List<object?> _buffer = [];
        private readonly Action<object?> _redirect = redirect;

        public override Encoding Encoding { get; } = Encoding.UTF8;

        public override void Flush()
            => this._redirect(string.Concat(this._buffer));

        public override void WriteLine(object? value)
            => this._buffer.Add(value);

        public override void WriteLine(string? value)
            => this._buffer.Add(value);
    }
}
namespace Library.Coding;

public sealed class Progress<T> : IProgress<T>
{
    private readonly Action<T> _reporter;

    public Progress(Action<T> reporter)
        => this._reporter = reporter;

    public void Report(T value)
        => this._reporter(value);
}

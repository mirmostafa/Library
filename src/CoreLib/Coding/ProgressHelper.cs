using Library.Interfaces;

namespace Library.Coding;

public interface IProgressiveReport<in T>
{
    void Report(T value)
        => this.Report(value, -1, -1);

    void Report(T value, int current, int max);

    static IProgressiveReport<T> Empty { get; } = new ProgressiveReport<T>((_, _, _) => { });
}

public static class Progress
{
    public static IProgress<T> Create<T>(Action<T> reporter)
        => new Progress<T>(reporter);

    public static IProgressiveReport<T> CreateProgressive<T>(Action<T, int, int> reporter)
        => new ProgressiveReport<T>(reporter);
}

internal sealed class Progress<T> : IProgress<T>
{
    private readonly Action<T> _reporter;

    public Progress(Action<T> reporter)
        => this._reporter = reporter;

    public void Report(T value)
        => this._reporter(value);
}

internal sealed class ProgressiveReport<T> : IProgressiveReport<T>
{
    private readonly Action<T, int, int> _reporter;

    public ProgressiveReport(Action<T, int, int> reporter) 
        => this._reporter = reporter;

    public void Report(T value, int current, int max)
        => this._reporter(value, current, max);
}
using System.Diagnostics.Contracts;

using Library.DesignPatterns.Markers;

namespace Library.Coding;

[Immutable]
public sealed class Progress<T> : ProgressBase<T>
{
    public Progress(Action<T> reporter) : base(reporter)
    {
    }
}

[Immutable]
public abstract class ProgressBase<T> : IProgress<T>
{
    private readonly Action<T> _reporter;

    protected ProgressBase(Action<T> reporter)
        => this._reporter = reporter;

    public void Report(T value)
        => this._reporter(value);
}

[Pure]
[Immutable]
public sealed class ProgressDetailed<T> : ProgressBase<(T State, string Description)>
{
    public ProgressDetailed(Action<(T State, string Description)> reporter) : base(reporter)
    {
    }

    public void Report(T state, string description)
        => this.Report((state, description));
}

[Immutable]
public sealed class ProgressiveReport<T> : ProgressBase<(T State, int Current, int Maximum, string Description)>
{
    public static readonly ProgressiveReport<T> Empty = new(_ => { });

    public ProgressiveReport(Action<(T State, int Current, int Maximum, string Description)> reporter) : base(reporter)
    {
    }

    public void Report(T state, int current, int maximum, string description)
        => this.Report((state, current, maximum, description));
}

[Immutable]
public sealed class ProgressiveReport : ProgressBase<(int Current, int Maximum, string Description)>
{
    public static readonly ProgressiveReport Empty = new(_ => { });

    public ProgressiveReport(Action<(int Current, int Maximum, string Description)> reporter) : base(reporter)
    {
    }

    public void Report(int current, int maximum, string description)
        => base.Report((current, maximum, description));
}
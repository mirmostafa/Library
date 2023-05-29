using System.Diagnostics.Contracts;

using Library.DesignPatterns.Markers;

namespace Library.Coding;

/// <summary>
/// Represents an immutable progress object that reports progress updates.
/// </summary>
/// <param name="reporter">The action to be called when progress is reported.</param>
/// <returns>
/// An immutable progress object that reports progress updates.
/// </returns>
[Immutable]
public sealed class Progress<T> : ProgressBase<T>
{
    public Progress(Action<T> reporter) : base(reporter)
    {
    }
}

/// <summary>
/// Represents an abstract base class for progress reporting.
/// </summary>
/// <typeparam name="T">The type of the progress report value.</typeparam>
[Immutable]
public abstract class ProgressBase<T> : IProgress<T>
{
    private readonly Action<T> _reporter;

    protected ProgressBase(Action<T> reporter)
        => this._reporter = reporter;

    /// <summary>
    /// Reports the given value using the reporter.
    /// </summary>
    /// <param name="value">The value to report.</param>
    public void Report(T value)
        => this._reporter(value);
}

/// <summary>
/// Represents a progress object that reports a state and a description.
/// </summary>
/// <typeparam name="T">The type of the state.</typeparam>
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

/// <summary>
/// Represents a class that provides a progress report for a given state.
/// </summary>
/// <typeparam name="T">The type of the state.</typeparam>
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

/// <summary>
/// Represents a progress report that can be used to report progress of a task.
/// </summary>
/// <param name="reporter">The action to be called when progress is reported.</param>
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
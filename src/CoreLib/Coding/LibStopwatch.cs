using System.Diagnostics;

namespace Library.Coding;

/// <summary>
/// Represents an interface for a stopwatch timer.
/// </summary>
/// <returns>
/// A TimeSpan object representing the elapsed time and a TimeSpan object representing the stopped time.
/// </returns>
public interface IStopwatchTimer : IDisposable
{
    /// <summary>
    /// Gets the elapsed time since the start of the operation.
    /// </summary>
    TimeSpan Elapsed { get; }

    /// <summary>
    /// Stops the current TimeSpan instance.
    /// </summary>
    TimeSpan Stop();
}

/// <summary>
/// Represents a class that provides a stopwatch timer.
/// </summary>
/// <returns>
/// An instance of the <see cref="LibStopwatch"/> class.
/// </returns>
public sealed class LibStopwatch
{
    private readonly StopwatchTimer _stopwatchTimer;

    /// <summary>
    /// Initializes a new instance of the <see cref="LibStopwatch"/> class.
    /// </summary>
    public LibStopwatch()
        => this._stopwatchTimer = new StopwatchTimer();

    /// <summary>
    /// Gets the elapsed time since the StopwatchTimer was started.
    /// </summary>
    public TimeSpan Elapsed
        => this._stopwatchTimer.Elapsed;

    /// <summary>
    /// Starts a new instance of IStopwatchTimer.
    /// </summary>
    public static IStopwatchTimer StartNew()
        => new LibStopwatch().Start();

    /// <summary>
    /// Starts the StopwatchTimer.
    /// </summary>
    /// <returns>The StopwatchTimer.</returns>
    public IStopwatchTimer Start()
        => this._stopwatchTimer.Start();
}

internal class StopwatchTimer : IStopwatchTimer
{
    private readonly Stopwatch _stopwatch;

    internal StopwatchTimer()
        => this._stopwatch = new();

    public TimeSpan Elapsed
        => this._stopwatch.Elapsed;

    public void Dispose()
        => this.Stop();

    public StopwatchTimer Start()
    {
        this._stopwatch.Restart();
        return this;
    }

    public TimeSpan Stop()
    {
        this._stopwatch.Stop();
        return this.Elapsed;
    }
}
using System.Diagnostics;

namespace Library.Coding;

public interface IStopwatchTimer : IDisposable
{
    TimeSpan Elapsed { get; }

    TimeSpan Stop();
}

public sealed class LibStopwatch
{
    private readonly StopwatchTimer _stopwatchTimer;

    public LibStopwatch()
        => this._stopwatchTimer = new StopwatchTimer();

    public TimeSpan Elapsed
        => this._stopwatchTimer.Elapsed;

    public static IStopwatchTimer StartNew()
        => new LibStopwatch().Start();

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
using System.ComponentModel;
using System.Diagnostics;

namespace Library.Coding;

public class LibStopwatch
{
    private readonly StopwatchTimer _stopwatchTimer;

    public LibStopwatch()
        => this._stopwatchTimer = new StopwatchTimer();

    public TimeSpan Elapsed
        => this._stopwatchTimer.Elapsed;

    public StopwatchTimer Start() => this._stopwatchTimer.Start();
}

[EditorBrowsable(EditorBrowsableState.Advanced)]
public class StopwatchTimer : IDisposable
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
        this._stopwatch.Start();
        return this;
    }

    public TimeSpan Stop()
    {
        this._stopwatch.Stop();
        return this.Elapsed;
    }
}
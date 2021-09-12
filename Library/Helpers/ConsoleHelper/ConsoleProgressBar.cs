namespace Library.Helpers.ConsoleHelper;

/// <summary>
/// An ASCII progress bar
/// </summary>
public class ConsoleProgressBar : IDisposable, IProgress<double>
{
    private const int _blockCount = 10;
    private readonly TimeSpan _animationInterval = TimeSpan.FromSeconds(1.0 / 8);
    private const string _animation = @"|/-\";

    private readonly Timer _timer;

    private double _currentProgress = 0;
    private string _currentText = string.Empty;
    private int _animationIndex = 0;
    private bool _disposed;

    public ConsoleProgressBar()
    {
        this._timer = new Timer(this.TimerHandler);

        // A progress bar is only for temporary display in a console window.
        // If the console output is redirected to a file, draw nothing.
        // Otherwise, we'll end up with a lot of garbage in the target file.
        if (!Console.IsOutputRedirected)
        {
            this.ResetTimer();
        }
    }

    public void Report(double current, double max) => this.Report(current / max);

    public void Report(double value)
    {
        // Make sure value is in [0..1] range
        value = Math.Max(0, Math.Min(1, value));
        _ = Interlocked.Exchange(ref this._currentProgress, value);
    }

    private void TimerHandler(object? state)
    {
        lock (this._timer)
        {
            if (this._disposed)
            {
                return;
            }

            var progressBlockCount = (int)(this._currentProgress * _blockCount);
            var percent = (int)(this._currentProgress * 100);
            var text = string.Format("[{0}{1}] {2,3}% {3}",
                new string('#', progressBlockCount), new string('-', _blockCount - progressBlockCount),
                percent,
                _animation[this._animationIndex++ % _animation.Length]);
            this.UpdateText(text);

            this.ResetTimer();
        }
    }

    private void UpdateText(string text)
    {
        // Get length of common portion
        var commonPrefixLength = 0;
        var commonLength = Math.Min(this._currentText.Length, text.Length);
        while (commonPrefixLength < commonLength && text[commonPrefixLength] == this._currentText[commonPrefixLength])
        {
            commonPrefixLength++;
        }

        // Backtrack to the first differing character
        var outputBuilder = new StringBuilder();
        _ = outputBuilder.Append('\b', this._currentText.Length - commonPrefixLength);

        // Output new suffix
        _ = outputBuilder.Append(text.AsSpan(commonPrefixLength));

        // If the new text is shorter than the old one: delete overlapping characters
        var overlapCount = this._currentText.Length - text.Length;
        if (overlapCount > 0)
        {
            _ = outputBuilder.Append(' ', overlapCount);
            _ = outputBuilder.Append('\b', overlapCount);
        }

        Console.Write(outputBuilder);
        this._currentText = text;
    }

    private void ResetTimer() => this._timer.Change(this._animationInterval, TimeSpan.FromMilliseconds(-1));

    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                lock (this._timer)
                {
                    this._disposed = true;
                    this.UpdateText(string.Empty);
                }
            }
            this._disposed = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~ProgressBar()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
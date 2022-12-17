namespace Library.Helpers.Models;

/// <summary>
/// An ASCII progress bar
/// </summary>
public class ConsoleProgressBar : IDisposable, IProgress<double>
{
    private const string ANIMATION = @"|/-\";
    private readonly TimeSpan _animationInterval = TimeSpan.FromSeconds(1.0 / 8);
    private readonly int _blockCount;
    private readonly bool _showInTitle;
    private readonly Timer _timer;

    private int _animationIndex = 0;
    private double _currentProgress = 0;
    private string _currentText = string.Empty;
    private bool _disposed;
    private int? startIndex;

    public ConsoleProgressBar(in int blockCount = 10, bool showInTitle = false)
    {
        this._timer = new Timer(this.TimerHandler);

        // A progress bar is only for temporary display in a console window. If the console output
        // is redirected to a file, draw nothing. Otherwise, we'll end up with a lot of garbage in
        // the target file.
        if (!Console.IsOutputRedirected)
        {
            this.ResetTimer();
        }
        this._blockCount = blockCount;
        this._showInTitle = showInTitle;
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public void Report(double current, double max)
        => this.Report(current / max);

    public void Report(double value)
    {
        // Make sure value is in [0..1] range
        value = Math.Max(0, Math.Min(1, value));
        _ = Interlocked.Exchange(ref this._currentProgress, value);
    }

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

    private void ResetTimer()
        => this._timer.Change(this._animationInterval, TimeSpan.FromMilliseconds(-1));

    private void TimerHandler(object? state)
    {
        lock (this._timer)
        {
            if (this._disposed)
            {
                return;
            }

            var progressBlockCount = (int)(this._currentProgress * this._blockCount);
            var percent = (int)(this._currentProgress * 100);
            var text = string.Format("[{0}{1}] {2,3}% {3}",
                new string('#', progressBlockCount), new string('=', this._blockCount - progressBlockCount),
                percent,
                ANIMATION[this._animationIndex++ % ANIMATION.Length]);
            this.UpdateText(text);

            this.ResetTimer();
        }
    }

    private void UpdateText(string text)
    {
        //// Get length of common portion
        //var commonPrefixLength = 0;
        //var commonLength = Math.Min(this._currentText.Length, text.Length);
        //while (commonPrefixLength < commonLength && text[commonPrefixLength] == this._currentText[commonPrefixLength])
        //{
        //    commonPrefixLength++;
        //}

        //// Backtrack to the first differing character
        //var outputBuilder = new StringBuilder();
        //_ = outputBuilder.Append('\b', this._currentText.Length - commonPrefixLength);

        //// Output new suffix
        //_ = outputBuilder.Append(text.AsSpan(commonPrefixLength));

        //// If the new text is shorter than the old one: delete overlapping characters
        //var overlapCount = this._currentText.Length - text.Length;
        //if (overlapCount > 0)
        //{
        //    _ = outputBuilder.Append(' ', overlapCount);
        //    _ = outputBuilder.Append('\b', overlapCount);
        //}
        //Console.Write(outputBuilder);

        //x Get length of common portion
        Console.CursorLeft = this.startIndex ?? (this.startIndex = Console.CursorLeft).Value;
        if (this._showInTitle)
        {
            Console.Title = text;
        }
        else
        {
            Console.Write(text);
        }

        this._currentText = text;
    }
}
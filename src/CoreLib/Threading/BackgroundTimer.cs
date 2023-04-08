using Library.DesignPatterns.Markers;
using Library.Logging;

namespace Library.Threading;

[Immutable]
[Fluent]
public sealed class BackgroundTimer : ILoggerContainer
{
    private readonly CancellationTokenSource _cancellationSource;
    private readonly ILogger _Logger;
    private readonly bool _shouldDisposeCancellationTokenSource;
    private bool _isDone;
    private Action<Exception>? _onError;
    private Task? _timerTask;

    public BackgroundTimer([DisallowNull] in Action action, [DisallowNull] in TimeSpan interval, string? name = null, CancellationTokenSource? cancellationSource = null, ILogger? logger = null)
    {
        this._Logger = logger ?? EmptyLogger.Empty;
        this.Action = action;
        this.Interval = interval;
        this.Name = name;
        if (cancellationSource is not null)
        {
            this._cancellationSource = cancellationSource;
        }
        else
        {
            this._shouldDisposeCancellationTokenSource = true;
            this._cancellationSource = new();
        }
    }

    public Action Action { get; }
    public TimeSpan Interval { get; }
    ILogger ILoggerContainer.Logger => _Logger;
    public string? Name { get; }

    public static BackgroundTimer New([DisallowNull] in Action action, [DisallowNull] in TimeSpan interval, string? name = null, CancellationTokenSource? cancellationSource = null)
        => new(action, interval, name, cancellationSource);

    public static BackgroundTimer New([DisallowNull] in Action action, [DisallowNull] in long intervalInMilliseconds, string? name = null, CancellationTokenSource? cancellationSource = null)
        => new(action, TimeSpan.FromMilliseconds(intervalInMilliseconds), name, cancellationSource);

    public static BackgroundTimer Start([DisallowNull] in Action action, [DisallowNull] in TimeSpan interval, string? name = null, CancellationTokenSource? cancellationSource = null)
        => new BackgroundTimer(action, interval, name, cancellationSource).Start();

    public static BackgroundTimer Start([DisallowNull] in Action action, [DisallowNull] in long intervalInMilliseconds, string? name = null, CancellationTokenSource? cancellationSource = null)
        => new BackgroundTimer(action, TimeSpan.FromMilliseconds(intervalInMilliseconds), name, cancellationSource).Start();

    public BackgroundTimer OnError(Action<Exception> onError)
        => this.Fluent(this._onError = onError);

    public BackgroundTimer Start()
    {
        if (this._isDone)
        {
            throw new();
        }

        LibLogger.Info($"Starting…", sender: this);
        async Task DoStart()
        {
            try
            {
                using var timer = new PeriodicTimer(this.Interval);
                while (this._cancellationSource.IsCancellationRequested || await timer.WaitForNextTickAsync(this._cancellationSource.Token))
                {
                    if (this._cancellationSource.IsCancellationRequested)
                    {
                        break;
                    }

                    lock (this)
                    {
                        try
                        {
                            this.Debug($"Next interval started.", sender: this);
                            this.Action();
                            this.Debug($"Next interval ended.", sender: this);
                        }
                        catch (Exception ex)
                        {
                            LibLogger.Error(ex, sender: this);
                            this.Error($"Next interval error:'{ex.GetBaseException().Message}'", sender: this);
                            this.Debug($"Next interval error:'{ex.GetFullMessage()}'", sender: this);

                            this._onError?.Invoke(ex);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                this._isDone = true;
            }
        }
        this._timerTask = DoStart();
        LibLogger.Debug($"Started.", sender: this);
        return this;
    }

    public async Task<BackgroundTimer> StopAsync()
    {
        if (this._isDone)
        {
            return this;
        }

        LibLogger.Info($"Stopping…", sender: this);
        this._cancellationSource.Cancel();
        if (this._timerTask is not null)
        {
            await this._timerTask;
        }

        if (this._shouldDisposeCancellationTokenSource)
        {
            this._cancellationSource.Dispose();
        }
        LibLogger.Debug($"Stopped.", sender: this);
        this.Debug($"Timer stopped.", sender: this);
        return this;
    }

    public override string ToString()
        => $"{nameof(BackgroundTimer)}[{this.Name}]";
}

public static class BackgroundTimerExtensions
{
    public static BackgroundTimer Sleep(this BackgroundTimer instance, TimeSpan wait)
    {
        Thread.Sleep(wait);
        return instance;
    }

    public static async Task<BackgroundTimer> SleepAsync(this BackgroundTimer instance, TimeSpan wait)
    {
        await Task.Delay(wait);
        return instance;
    }
}
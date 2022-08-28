namespace Library.ProgressiveOperations;

public record struct StepInfo<TState>(in Func<(TState State, IMultistepProgress SubProgress), Task<TState>> AsyncAction, in string Description, in int ProgressCount)
{
    public static StepInfo<TState> New(in Func<(TState State, IMultistepProgress SubProgress), Task<TState>> AsyncAction, in string Description, in int ProgressCount)
        => new(AsyncAction, Description, ProgressCount);
    public static StepInfo<TState> New(Func<TState, TState> action, in string description, int progressCount = 1)
        => new(e => Task.FromResult(action(e.State)), description, progressCount);
    public static StepInfo<TState> New(Action<TState> action, in string description, int progressCount = 1)
        => new(e =>
        {
            action(e.State);
            return Task.FromResult(e.State);
        }, description, progressCount);
    public static StepInfo<TState> New(Func<TState, Task> action, in string description, int progressCount = 1)
        => new(async e =>
        {
            await action(e.State);
            return e.State;
        }, description, progressCount);
    public static StepInfo<TState> New(Func<TState, Task<TState>> action, in string description, int progressCount = 1)
        => new(e => action(e.State), description, progressCount);
    public static StepInfo<TState> New(Action action, in string description, int progressCount = 1)
        => new(e =>
        {
            action();
            return Task.FromResult(e.State);
        }, description, progressCount);
}

internal class MultistepProgress : IMultistepProgress
{
    private readonly Action<(string Description, int Max, int Current)>? _onReporting;

    public MultistepProgress(in Action<(string Description, int Max, int Current)>? onReporting)
        => this._onReporting = onReporting;

    public void Report(in string description, in int max = -1, in int current = -1)
        => this._onReporting?.Invoke((description, max, current));
}

public interface IMultistepProgress
{
    static IMultistepProgress New(Action<(string Description, int Max, int Current)>? onReporting = null) => new MultistepProgress(onReporting);

    void Report(in string description, in int max = -1, in int current = -1);
}

public class MultistepProgressManager<TState>
{
    private readonly IMultistepProgress _reporter;
    private readonly IEnumerable<StepInfo<TState>> _steps;
    private readonly IMultistepProgress _subReporter;
    private int _current;
    private int _max;
    private TState _state;

    public MultistepProgressManager(in TState state, in IEnumerable<StepInfo<TState>> steps, in IMultistepProgress reporter, IMultistepProgress? subReporter = null)
    {
        this._steps = steps;
        this._reporter = reporter;
        this._subReporter = subReporter ?? IMultistepProgress.New();
        this._state = state;
    }

    public MultistepProgressManager(in TState state, in IEnumerable<StepInfo<TState>> steps,
        in Action<(string Description, int Max, int Current)> reporter,
        Action<(string Description, int Max, int Current)>? subReporter = null)
        : this(state, steps, IMultistepProgress.New(reporter), subReporter is null ? null : IMultistepProgress.New(subReporter))
    {
    }

    public static Task<TState> StartAsync(in TState state, in IEnumerable<StepInfo<TState>> steps,
        in Action<(string Description, int Max, int Current)> reporter,
        Action<(string Description, int Max, int Current)>? subReporter = null)
    {
        var manager = new MultistepProgressManager<TState>(state, steps, reporter, subReporter);
        return manager.StartAsync();
    }

    public static Task<TState> StartAsync(in TState state, in IEnumerable<StepInfo<TState>> steps, in IMultistepProgress reporter, IMultistepProgress? subReporter = null)
    {
        var manager = new MultistepProgressManager<TState>(state, steps, reporter, subReporter);
        return manager.StartAsync();
    }

    public async Task<TState> StartAsync()
    {
        this._max = this._steps.Select(x => x.ProgressCount).Sum();
        this._current = 0;
        foreach (var step in this._steps)
        {
            this._reporter.Report(step.Description, this._max, this._current += step.ProgressCount);
            this._state = await step.AsyncAction((this._state, this._subReporter));
        }
        return this._state;
    }
}
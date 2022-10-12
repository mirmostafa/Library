using Library.Validations;

namespace Library.MultistepProgress;

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

public interface IMultistepProgress
{
    static IMultistepProgress New([DisallowNull] in Action<(string Description, int Max, int Current)> onReporting, in Action<string?>? onEnded = null)
        => new MultistepProgress(onReporting, onEnded);

    void Ended(in string? description = null);

    void Report(in string description, in int max = -1, in int current = -1);
}

internal class MultistepProgress : IMultistepProgress
{
    private readonly Action<string?>? _onEnded;
    private readonly Action<(string Description, int Max, int Current)> _onReporting;

    public MultistepProgress([DisallowNull] in Action<(string Description, int Max, int Current)> onReporting, in Action<string?>? onEnded)
    {
        this._onReporting = onReporting.ArgumentNotNull();
        this._onEnded = onEnded;
    }

    public void Ended(in string? description)
        => this._onEnded?.Invoke(description);

    public void Report(in string description, in int max = -1, in int current = -1)
        => this._onReporting((description, max, current));
}

public class MultistepProcessRunner<TState>
{
    private readonly IMultistepProgress _reporter;
    private readonly List<StepInfo<TState>> _steps = new();
    private readonly IMultistepProgress _subReporter;
    private int _current;
    private int _max;
    private TState _state;

    public MultistepProcessRunner(in TState state, in IMultistepProgress reporter, IMultistepProgress? subReporter = null)
    {
        this._reporter = reporter;
        this._subReporter = subReporter ?? IMultistepProgress.New(_ => { });
        this._state = state;
    }

    public MultistepProcessRunner(in TState state,
        in Action<(string Description, int Max, int Current)> reporter,
        Action<(string Description, int Max, int Current)>? subReporter = null)
        : this(state, IMultistepProgress.New(reporter), subReporter is null ? null : IMultistepProgress.New(subReporter))
    {
    }

    public IEnumerable<StepInfo<TState>> Steps => this._steps;

    public static MultistepProcessRunner<TState> New(in TState state, in IMultistepProgress reporter, IMultistepProgress? subReporter = null)
        => new(state, reporter, subReporter);

    public static MultistepProcessRunner<TState> New(in TState state,
        in Action<(string Description, int Max, int Current)> reporter,
        Action<(string Description, int Max, int Current)>? subReporter = null)
        => new(state, reporter, subReporter);

    public static Task<TState> RunAsync(
        in TState state, in IEnumerable<StepInfo<TState>> steps,
        in Action<(string Description, int Max, int Current)> reporter,
        in Action<(string Description, int Max, int Current)>? subReporter = null,
        in CancellationToken? token = default)
    {
        var manager = new MultistepProcessRunner<TState>(state, reporter, subReporter);
        _ = manager.AddStep(steps);
        return manager.RunAsync(default);
    }

    public static Task<TState> RunAsync(in TState state, in IEnumerable<StepInfo<TState>> steps, in IMultistepProgress reporter, IMultistepProgress? subReporter = null, in CancellationToken? token = default)
        => MultistepProcessRunner<TState>.New(state, reporter, subReporter).AddStep(steps).RunAsync(token);

    public MultistepProcessRunner<TState> AddStep(params StepInfo<TState>[] step)
    {
        this._steps.AddRange(step);
        return this;
    }

    public MultistepProcessRunner<TState> AddStep(IEnumerable<StepInfo<TState>> steps)
        => this.AddStep(steps.ToArray());

    public MultistepProcessRunner<TState> AddStep(Func<TState, Task> action, in string description, int progressCount = 1)
        => this.AddStep(StepInfo<TState>.New(action, description, progressCount));

    public MultistepProcessRunner<TState> AddStep(Action action, in string description, int progressCount = 1)
        => this.AddStep(StepInfo<TState>.New(action, description, progressCount));
    public MultistepProcessRunner<TState> AddStep(Action<TState> action, in string description, int progressCount = 1)
            => this.AddStep(StepInfo<TState>.New(action, description, progressCount));

    public async Task<TState> RunAsync(CancellationToken? cancellationToken = default)
    {
        var stepList = this.Steps.ToList();
        this._max = stepList.Select(x => x.ProgressCount).Sum();
        this._current = 0;
        var canBeCancelled = cancellationToken is not null and { CanBeCanceled: true };
        var token = cancellationToken is null ? default : cancellationToken.Value;
        var isCancellationRequested = () => canBeCancelled && token.IsCancellationRequested;
        foreach (var step in stepList)
        {
            if (isCancellationRequested())
            {
                break;
            }

            this._reporter.Report(step.Description, this._max, this._current += step.ProgressCount);
            this._state = await step.AsyncAction((this._state, this._subReporter));
        }
        this._reporter.Ended();
        return this._state;
    }
}
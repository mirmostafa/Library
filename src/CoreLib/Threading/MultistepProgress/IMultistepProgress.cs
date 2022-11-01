using Library;

using Library.Threading.MultistepProgress;
using Library.Validations;

namespace Library.Threading.MultistepProgress;

public interface IMultistepProcess
{
    static IMultistepProcess New([DisallowNull] in Action<(int Max, int Current, string? Description)> onReporting, in Action<string?>? onEnded = null)
        => new MultistepProcess(onReporting, onEnded);

    void Ended(in string? description = null);

    void Report(in int max = -1, in int current = -1, in string? description = null);
}

public class MultistepProcessRunner<TState>
{
    private readonly IMultistepProcess _reporter;
    private readonly List<StepInfo<TState>> _steps = new();
    private readonly IMultistepProcess _subReporter;
    private int _current;
    private int _max;
    private TState _state;

    public MultistepProcessRunner(in TState state, in IMultistepProcess reporter, IMultistepProcess? subReporter = null)
    {
        this._reporter = reporter;
        this._subReporter = subReporter ?? IMultistepProcess.New(_ => { });
        this._state = state;
    }

    public MultistepProcessRunner(in TState state,
        in Action<(int Max, int Current, string? Description)> reporter,
        Action<(int Max, int Current, string? Description)>? subReporter = null)
        : this(state, IMultistepProcess.New(reporter), subReporter is null ? null : IMultistepProcess.New(subReporter))
    {
    }

    public IEnumerable<StepInfo<TState>> Steps => this._steps;

    public static MultistepProcessRunner<TState> New(in TState state, in IMultistepProcess reporter, IMultistepProcess? subReporter = null)
        => new(state, reporter, subReporter);

    public static MultistepProcessRunner<TState> New(in TState state,
        in Action<(int Max, int Current, string? Description)> reporter,
        Action<(int Max, int Current, string? Description)>? subReporter = null)
        => new(state, reporter, subReporter);

    public static Task<TState> RunAsync(
        in TState state, in IEnumerable<StepInfo<TState>> steps,
        in Action<(int Max, int Current, string? Description)> reporter,
        in Action<(int Max, int Current, string? Description)>? subReporter = null,
        in CancellationToken? token = default)
    {
        var manager = new MultistepProcessRunner<TState>(state, reporter, subReporter);
        _ = manager.AddStep(steps);
        return manager.RunAsync(default);
    }

    public static Task<TState> RunAsync(in TState state, in IEnumerable<StepInfo<TState>> steps, in IMultistepProcess reporter, IMultistepProcess? subReporter = null, in CancellationToken? token = default)
        => MultistepProcessRunner<TState>.New(state, reporter, subReporter).AddStep(steps).RunAsync(token);

    public MultistepProcessRunner<TState> AddStep(StepInfo<TState> step)
        => this.AddStep(EnumerableHelper.ToArray(step));

    public MultistepProcessRunner<TState> AddStep(params StepInfo<TState>[] step)
    {
        this._steps.AddRange(step);
        return this;
    }

    public MultistepProcessRunner<TState> AddStep(IEnumerable<StepInfo<TState>> steps)
        => this.AddStep(steps.ToArray());

    public MultistepProcessRunner<TState> AddStep(Func<(TState State, IMultistepProcess SubProgress), TState> action, string? description, int progressCount)
        => this.AddStep(new StepInfo<TState>(e => Task.FromResult(action(e)), description, progressCount));

    public MultistepProcessRunner<TState> AddStep(in Func<(TState State, IMultistepProcess SubProgress), Task<TState>> asyncAction, in string? description, in int progressCount)
        => this.AddStep(new StepInfo<TState>(asyncAction, description, progressCount));

    public MultistepProcessRunner<TState> AddStep(Func<TState, Task> action, in string? description, int progressCount = 1)
        => this.AddStep(new StepInfo<TState>(async e =>
        {
            await action(e.State);
            return e.State;
        }, description, progressCount));

    public MultistepProcessRunner<TState> AddStep(Action action, in string? description, int progressCount = 1)
        => this.AddStep(new StepInfo<TState>(e =>
        {
            action();
            return Task.FromResult(e.State);
        }, description, progressCount));

    public MultistepProcessRunner<TState> AddStep(Action<TState> action, in string? description, int progressCount = 1)
            => this.AddStep(new StepInfo<TState>(e =>
            {
                action(e.State);
                return Task.FromResult(e.State);
            }, description, progressCount));

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

            this._reporter.Report(this._max, this._current += step.ProgressCount, step.Description);
            this._state = await step.AsyncAction((this._state, this._subReporter));
        }
        this._reporter.Ended();
        return this._state;
    }
}

internal class MultistepProcess : IMultistepProcess
{
    private readonly Action<string?>? _onEnded;
    private readonly Action<(int Max, int Current, string? Description)> _onReporting;

    public MultistepProcess([DisallowNull] in Action<(int Max, int Current, string? Description)> onReporting, in Action<string?>? onEnded)
    {
        this._onReporting = onReporting.ArgumentNotNull();
        this._onEnded = onEnded;
    }

    public void Ended(in string? description)
        => this._onEnded?.Invoke(description);

    public void Report(in int max = -1, in int current = -1, in string? description = null)
        => this._onReporting((max, current, description));
}

public record struct StepInfo<TState>(in Func<(TState State, IMultistepProcess SubProgress), Task<TState>> AsyncAction, in string? Description, in int ProgressCount);
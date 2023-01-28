using Library.EventsArgs;

namespace Library.Threading.MultistepProgress;

public interface IMultistepProcess
{
    event EventHandler<ItemActedEventArgs<string?>>? Ended;

    event EventHandler<ItemActedEventArgs<(int Max, int Current, string? Description)>>? Reported;

    static IMultistepProcess New()
        => new MultistepProcess();

    void End(in string? description = null);

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
        this._subReporter = subReporter ?? IMultistepProcess.New();
        this._state = state;
    }

    public MultistepProcessRunner(in TState state,
        Action<(int Max, int Current, string? Description)> reporter,
        Action<(int Max, int Current, string? Description)>? subReporter = null)
        : this(state,
              IMultistepProcess.New().With(x => x.Reported += (_, e) => reporter?.Invoke(e.Item)),
              subReporter is null ? null :
                IMultistepProcess.New().With(x => x.Reported += (_, e) => subReporter(e.Item)))
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
        var token = canBeCancelled ? cancellationToken!.Value : default;
        Func<bool> isCancellationRequested = canBeCancelled ? () => token.IsCancellationRequested : () => false;
        foreach (var step in stepList)
        {
            if (isCancellationRequested())
            {
                break;
            }

            this._reporter.Report(this._max, this._current += step.ProgressCount, step.Description);
            this._state = await step.AsyncAction((this._state, this._subReporter));
        }
        this._reporter.End();
        return this._state;
    }
}

internal class MultistepProcess : IMultistepProcess
{
    public event EventHandler<ItemActedEventArgs<string?>>? Ended;

    public event EventHandler<ItemActedEventArgs<(int Max, int Current, string? Description)>>? Reported;

    public MultistepProcess()
    {
    }

    public void End(in string? description)
        => this.Ended?.Invoke(this, new(description));

    public void Report(in int max = -1, in int current = -1, in string? description = null)
        => this.Reported?.Invoke(this, new((max, current, description)));
}

public record struct StepInfo<TState>(in Func<(TState State, IMultistepProcess SubProgress), Task<TState>> AsyncAction, in string? Description, in int ProgressCount);
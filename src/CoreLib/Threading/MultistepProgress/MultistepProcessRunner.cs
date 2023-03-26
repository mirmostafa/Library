namespace Library.Threading.MultistepProgress;

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

    private MultistepProcessRunner(in TState state,
        Action<(int Max, int Current, string? Description)> reporter,
        Action<(int Max, int Current, string? Description)>? subReporter = null)
        : this(state,
              IMultistepProcess.New().With(x => x.Reported += (_, e) => reporter?.Invoke(e.Item)),
              subReporter is null ? null :
                IMultistepProcess.New().With(x => x.Reported += (_, e) => subReporter(e.Item)))
    {
    }

    public IEnumerable<StepInfo<TState>> Steps => this._steps;

    public static MultistepProcessRunner<TState> New(in TState state, in IMultistepProcess reporter, in IMultistepProcess? subReporter = null)
        => new(state, reporter, subReporter);

    public MultistepProcessRunner<TState> AddStep(in StepInfo<TState> step)
        => this.AddStep(EnumerableHelper.ToArray(step));

    public MultistepProcessRunner<TState> AddStep(params StepInfo<TState>[] step)
    {
        this._steps.AddRange(step);
        return this;
    }

    public MultistepProcessRunner<TState> AddStep(in IEnumerable<StepInfo<TState>> steps)
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
        var canBeCancelled = cancellationToken is not null and { CanBeCanceled: true };
        var token = canBeCancelled ? cancellationToken!.Value : default;
        Func<bool> isCancellationRequested = canBeCancelled ? () => token.IsCancellationRequested : () => false;
        var stepList = this.Steps.ToList();
        this._max = stepList.Select(x => x.ProgressCount).Sum();
        this._current = 0;
        foreach (var step in stepList)
        {
            if (isCancellationRequested())
            {
                break;
            }

            this._reporter.Report(step.Description, this._max, this._current += step.ProgressCount);
            this._state = await step.AsyncAction((this._state, this._subReporter));
        }
        this._reporter.End();
        return this._state;
    }
}

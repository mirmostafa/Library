using System.Diagnostics;

using Library.DesignPatterns.Markers;

namespace Library.Threading.MultistepProgress;

[Fluent]
[DebuggerStepThrough]
[StackTraceHidden]
public sealed class MultistepProcessRunner<TState>(in TState state, in IMultistepProcess reporter, IMultistepProcess? subReporter = null, object? owner = null)
{
    private readonly object? _owner = owner;
    private readonly IMultistepProcess _reporter = reporter;
    private readonly List<StepInfo<TState>> _steps = new();
    private readonly IMultistepProcess _subReporter = subReporter ?? IMultistepProcess.New();
    private int _current;
    private int _max;
    private TState _state = state;

    public IEnumerable<StepInfo<TState>> Steps => this._steps;

    public static MultistepProcessRunner<TState> New(in TState state, in IMultistepProcess reporter, in IMultistepProcess? subReporter = null, object? owner = null)
        => new(state, reporter, subReporter, owner);

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
        => this.AddStep(new StepInfo<TState>([DebuggerStepThrough] async (e) =>
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

    public async Task<TState> RunAsync(CancellationToken cancellationToken = default)
    {
        var stepList = this.Steps.ToList();
        this._max = stepList.Select(x => x.ProgressCount).Sum();
        this._current = 0;
        LibLogger.DebugStartingAction();
        foreach (var step in stepList)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            this._reporter.Report(this._max, this._current += step.ProgressCount, step.Description, this._owner);
            this._state = await step.AsyncAction((this._state, this._subReporter));
            LibLogger.Debug($"{nameof(MultistepProcessRunner<TState>)} - [{this._current} of {this._max}]");
        }
        this._reporter.End();
        LibLogger.DebugEndedAction();
        return this._state;
    }
}
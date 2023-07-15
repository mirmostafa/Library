using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;

using Library.DesignPatterns.Markers;

namespace Library.Threading.MultistepProgress;

[Fluent]
public sealed class MultistepProcessRunner<TState>(in TState state, in IMultistepProcess reporter, IMultistepProcess? subReporter = null, object? owner = null)
{
    private readonly object? _owner = owner;
    private readonly IMultistepProcess _reporter = reporter;
    private readonly List<StepInfo<TState>> _stepsList = new();
    private readonly IMultistepProcess _subReporter = subReporter ?? IMultistepProcess.New();
    private int _current;
    private int _max;
    private TState _state = state;

    public IEnumerable<StepInfo<TState>> Steps => this._stepsList;

    public static MultistepProcessRunner<TState> New(in TState state, in IMultistepProcess reporter, in IMultistepProcess? subReporter = null, object? owner = null)
        => new(state, reporter, subReporter, owner);

    public MultistepProcessRunner<TState> AddStep(in StepInfo<TState> step)
        => this.AddStep(EnumerableHelper.ToArray(step));

    public MultistepProcessRunner<TState> AddStep(params StepInfo<TState>[] step)
    {
        this._stepsList.AddRange(step);
        return this;
    }

    public MultistepProcessRunner<TState> AddStep(in IEnumerable<StepInfo<TState>> steps)
        => this.AddStep(steps.ToArray());

    public MultistepProcessRunner<TState> AddStep(Func<(TState State, IMultistepProcess SubProgress, CancellationToken cancellationToken), TState> action, string? description, int progressCount)
        => this.AddStep(new StepInfo<TState>(e => Task.FromResult(action(e)), description, progressCount));

    public MultistepProcessRunner<TState> AddStep(in Func<(TState State, IMultistepProcess SubProgress, CancellationToken cancellationToken), Task<TState>> asyncAction, in string? description, in int progressCount)
        => this.AddStep(new StepInfo<TState>(asyncAction, description, progressCount));

    public MultistepProcessRunner<TState> AddStep(Func<TState, CancellationToken, Task> action, in string? description, int progressCount = 1)
        => this.AddStep(new StepInfo<TState>([DebuggerStepThrough] async (e) =>
        {
            await action(e.State, e.cancellationToken);
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
        var steps = this._stepsList.ToImmutableArray();
        this._max = steps.Select(x => x.ProgressCount).Sum();
        this._current = 0;

        LibLogger.DebugStartingAction();

        var enumerator = steps.GetEnumerator();
        while (!cancellationToken.IsCancellationRequested && enumerator.MoveNext())
        {
            var step = enumerator.Current;

            this._reporter.Report(this._max, this._current += step.ProgressCount, step.Description, this._owner);
            LibLogger.Debug($"{nameof(MultistepProcessRunner<TState>)} - [{this._current} of {this._max}]");

            var arg = (this._state, this._subReporter, cancellationToken);
            this._state = await step.AsyncAction(arg);
        }

        this._reporter.End();
        LibLogger.DebugEndedAction();

        return this._state;
    }
}
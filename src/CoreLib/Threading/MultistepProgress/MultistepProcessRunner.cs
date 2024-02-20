using System.Diagnostics;

using Library.DesignPatterns.Markers;
using Library.Results;
using Library.Validations;

namespace Library.Threading.MultistepProgress;

[Fluent]
public sealed class MultistepProcessRunner<TState>(in TState state, in IProgressReport reporter, IProgressReport? subReporter = null, object? owner = null)
{
    private readonly object? _owner = owner;
    private readonly IProgressReport _reporter = reporter;
    private readonly List<StepInfo<TState>> _stepsList = [];
    private readonly IProgressReport _subReporter = subReporter ?? IProgressReport.New();
    private int _current;
    private bool _isRunning;
    private int _max;
    private TState _state = state;
    public IEnumerable<StepInfo<TState>> Steps => this._stepsList;

    public static MultistepProcessRunner<TState> New(in TState state, in IProgressReport reporter, in IProgressReport? subReporter = null, object? owner = null) =>
        new(state, reporter, subReporter, owner);

    public MultistepProcessRunner<TState> AddStep(in StepInfo<TState> step) =>
        this.AddStep(EnumerableHelper.ToArray(step));

    public MultistepProcessRunner<TState> AddStep(params StepInfo<TState>[] step)
    {
        this.CheckOperationRunning();
        this._stepsList.AddRange(step);
        return this;
    }

    public MultistepProcessRunner<TState> AddStep(in IEnumerable<StepInfo<TState>> steps) =>
        this.AddStep(steps.ToArray());

    public MultistepProcessRunner<TState> AddStep(Func<(TState State, IProgressReport SubProgress, CancellationToken cancellationToken), TState> action, string? description, int progressCount) =>
        this.AddStep(new StepInfo<TState>(e => Task.FromResult(action(e)), description, progressCount));

    public MultistepProcessRunner<TState> AddStep(in Func<(TState State, IProgressReport SubProgress, CancellationToken cancellationToken), Task<TState>> asyncAction, in string? description, in int progressCount) =>
        this.AddStep(new StepInfo<TState>(asyncAction, description, progressCount));

    public MultistepProcessRunner<TState> AddStep(Func<TState, CancellationToken, Task> action, in string? description, int progressCount = 1) =>
        this.AddStep(new StepInfo<TState>([DebuggerStepThrough] async (e) =>
        {
            await action(e.State, e.cancellationToken);
            return e.State;
        }, description, progressCount));

    public MultistepProcessRunner<TState> AddStep(Action action, in string? description, int progressCount = 1) =>
        this.AddStep(new StepInfo<TState>(e =>
        {
            action();
            return Task.FromResult(e.State);
        }, description, progressCount));

    public MultistepProcessRunner<TState> AddStep(Action<TState> action, in string? description, int progressCount = 1) =>
        this.AddStep(new StepInfo<TState>(e =>
        {
            action(e.State);
            return Task.FromResult(e.State);
        }, description, progressCount));

    [return: NotNull]
    public async Task<Result<TState>> RunAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Result.Fail<TState>(this._state)!;
        }
        this.CheckOperationRunning();

        Result<TState> result = default!;
        (this._max, this._current) = (this._stepsList.Select(x => x.ProgressCount).Sum(), 0);
        (TState _state, IProgressReport _subReporter, CancellationToken cancellationToken) arg;
        Func<Result<TState>, TState, Result<TState>> add = default!;
        add = (result, state) =>
        {
            add = (r, s) => r.WithValue(s);
            return new(state);
        };

        try
        {
            LibLogger.DebugStartingAction();
            this._isRunning = true;
            foreach (var step in this._stepsList)
            {
                this._reporter.Report(this._max, this._current += step.ProgressCount, step.Description, this._owner);
                LibLogger.Debug($"{nameof(MultistepProcessRunner<TState>)} - [{this._current} of {this._max}]");

                arg = (this._state, this._subReporter, cancellationToken);
                this._state = await step.AsyncAction(arg);
                result = add(result, this._state);
                if (cancellationToken.IsCancellationRequested)
                {
                    return result + Result.Fail(new OperationCanceledException(cancellationToken));
                }
            }
            return result;
        }
        finally
        {
            this._isRunning = false;
            this._reporter.End();
            LibLogger.DebugEndedAction();
        }
    }

    private void CheckOperationRunning()
        => Check.MustBe(!this._isRunning, () => new Exceptions.InvalidOperationException("Process is running"));
}
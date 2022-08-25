namespace Library.ProgressiveOperations;

public struct OperationReport
{
    public OperationReport(int max, int current, int? last = null)
        => (this.Last, this.Max, this.Current) = (last, max, current);

    public int Current { get; set; }
    public int? Last { get; }
    public int Max { get; }
}

public class MultistepProgressManager<TState, TValue>
{
    private readonly IMultistepProgress<TValue> _reporter;
    private readonly IEnumerable<StepInfo<TState, TValue>> _steps;
    private TState _state;

    public MultistepProgressManager(TState state, IEnumerable<StepInfo<TState, TValue>> steps, IMultistepProgress<TValue> reporter)
    {
        this._steps = steps;
        this._reporter = reporter;
        this._state = state;
    }

    public async Task<TState> StartAsync()
    {
        var max = this._steps.Select(x => x.ProgressCount).Sum();
        var current = 0;
        foreach (var step in this._steps)
        {
            this._reporter.Report(step.Value, max, current += step.ProgressCount);
            this._state = await step.AsyncAction(this._state);
        }
        return this._state;
    }
}

public class MultistepProgressManager<TState> : MultistepProgressManager<TState, string>
{
    public MultistepProgressManager(TState state, IEnumerable<StepInfo<TState, string>> steps, IMultistepProgress reporter)
        : base(state, steps, reporter)
    {
    }
}

internal class MultistepProgress<T> : IMultistepProgress<T>
{
    private readonly Action<(T Value, OperationReport Operation, OperationReport? SubOperation)> _onReporting;

    public MultistepProgress(Action<(T Value, OperationReport Operation, OperationReport? SubOperation)> onReporting)
        => this._onReporting = onReporting;

    public void Report(T value, OperationReport operation = new(), OperationReport? subOperation = null)
        => this._onReporting?.Invoke((value, operation, subOperation));
}

internal class MultistepProgress : MultistepProgress<string>, IMultistepProgress
{
    public MultistepProgress(Action<(string Value, OperationReport Operation, OperationReport? SubOperation)> onReporting) : base(onReporting)
    {
    }
}

public interface IMultistepProgress<T>
{
    static IMultistepProgress<T> GetNew(Action<(T Value, OperationReport Operation, OperationReport? SubOperation)> onReporting)
        => new MultistepProgress<T>(onReporting);

    void Report(T value, int max, int current)
        => this.Report(value, new(max, current));

    void Report(T value, int max, int current, int? last)
        => this.Report(value, new(max, current, last));

    void Report(T value, (int Max, int Current) main, (int Max, int Current) sub)
        => this.Report(value, new OperationReport(main.Max, main.Current), new OperationReport(sub.Max, sub.Current));

    void Report(T value, OperationReport operation = new(), OperationReport? subOperation = null);
}

public interface IMultistepProgress : IMultistepProgress<string>
{
    static new IMultistepProgress GetNew(Action<(string Value, OperationReport Operation, OperationReport? SubOperation)> onReporting)
        => new MultistepProgress(onReporting);

    static IMultistepProgress GetNew(Action<string, OperationReport?, OperationReport?> onReporting)
        => new MultistepProgress(e => onReporting(e.Value, e.Operation, e.SubOperation));
}

public record struct StepInfo<TState, TValue>(Func<TState, Task<TState>> AsyncAction, TValue Value, int ProgressCount);
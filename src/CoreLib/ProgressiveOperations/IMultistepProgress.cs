namespace Library.ProgressiveOperations;

//public struct OperationReport
//{
//    public OperationReport(int max, int current, int? last = null)
//        => (this.Last, this.Max, this.Current) = (last, max, current);

//    public int Current { get; set; }
//    public int? Last { get; }
//    public int Max { get; }
//}

//public class MultistepProgressManager<TState, TValue>
//{
//    private readonly IMultistepProgress<TValue> _reporter;
//    private readonly IEnumerable<StepInfo<TState, TValue>> _steps;
//    private readonly IMultistepProgress<TValue> _subReporter;
//    private TState _state;
//    private int _max;
//    private int _current;

// public MultistepProgressManager(TState state, IEnumerable<StepInfo<TState, TValue>> steps,
// IMultistepProgress<TValue> reporter, IMultistepProgress<TValue>? subReporter = null) {
// this._steps = steps; this._reporter = reporter; this._subReporter =
// subReporter??IMultistepProgress<TValue>.GetNew(OnSubReporting); this._state = state; }

// private void OnSubReporting((TValue Value, OperationReport Operation, OperationReport?
// SubOperation) report)
// => _reporter.Report()

//    public async Task<TState> StartAsync()
//    {
//        _max = this._steps.Select(x => x.ProgressCount).Sum();
//        _current = 0;
//        foreach (var step in this._steps)
//        {
//            this._reporter.Report(step.Value, _max, _current += step.ProgressCount);
//            this._state = await step.AsyncAction(this._state, this._subReporter);
//        }
//        return this._state;
//    }
//}

//public class MultistepProgressManager<TState> : MultistepProgressManager<TState, string>
//{
//    public MultistepProgressManager(TState state, IEnumerable<StepInfo<TState, string>> steps, IMultistepProgress reporter)
//        : base(state, steps, reporter)
//    {
//    }
//}

//internal class MultistepProgress<T> : IMultistepProgress<T>
//{
//    private readonly Action<(T Value, OperationReport Operation, OperationReport? SubOperation)> _onReporting;

// public MultistepProgress(Action<(T Value, OperationReport Operation, OperationReport?
// SubOperation)> onReporting)
// => this._onReporting = onReporting;

//    public void Report(T value, OperationReport operation = new(), OperationReport? subOperation = null)
//        => this._onReporting?.Invoke((value, operation, subOperation));
//}

//internal class MultistepProgress : MultistepProgress<string>, IMultistepProgress
//{
//    public MultistepProgress(Action<(string Value, OperationReport Operation, OperationReport? SubOperation)> onReporting) : base(onReporting)
//    {
//    }
//}

//public interface IMultistepProgress<T>
//{
//    static IMultistepProgress<T> GetNew(Action<(T Value, OperationReport Operation, OperationReport? SubOperation)> onReporting)
//        => new MultistepProgress<T>(onReporting);

// void Report(T value, int max, int current)
// => this.Report(value, new(max, current));

// void Report(T value, int max, int current, int? last)
// => this.Report(value, new(max, current, last));

// void Report(T value, (int Max, int Current) main, (int Max, int Current) sub)
// => this.Report(value, new OperationReport(main.Max, main.Current), new OperationReport(sub.Max, sub.Current));

//    void Report(T value, OperationReport operation = new(), OperationReport? subOperation = null);
//}

//public interface IMultistepProgress : IMultistepProgress<string>
//{
//    static new IMultistepProgress GetNew(Action<(string Value, OperationReport Operation, OperationReport? SubOperation)> onReporting)
//        => new MultistepProgress(onReporting);

//    static IMultistepProgress GetNew(Action<string, OperationReport?, OperationReport?> onReporting)
//        => new MultistepProgress(e => onReporting(e.Value, e.Operation, e.SubOperation));
//}

//public record struct StepInfo<TState, TValue>(Func<TState, IMultistepProgress<TValue>, Task<TState>> AsyncAction, TValue Value, int ProgressCount);

public record struct StepInfo<TState>(Func<(TState State, IMultistepProgress SubProgress), Task<TState>> AsyncAction, string Description, int ProgressCount);

internal class MultistepProgress : IMultistepProgress
{
    private readonly Action<(string Description, int Max, int Current)>? _onReporting;

    public MultistepProgress(Action<(string Description, int Max, int Current)>? onReporting)
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

    public MultistepProgressManager(TState state, IEnumerable<StepInfo<TState>> steps, IMultistepProgress reporter, IMultistepProgress? subReporter = null)
    {
        this._steps = steps;
        this._reporter = reporter;
        this._subReporter = subReporter ?? IMultistepProgress.New();
        this._state = state;
    }

    public MultistepProgressManager(TState state, IEnumerable<StepInfo<TState>> steps,
        Action<(string Description, int Max, int Current)> reporter,
        Action<(string Description, int Max, int Current)>? subReporter = null)
        : this(state, steps, IMultistepProgress.New(reporter), subReporter is null ? null : IMultistepProgress.New(subReporter))
    {
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
namespace Library.ProgressiveOperations;

public struct OperationReport
{
    public OperationReport(int? max, int? last, int? current = null)
        => (this.Last, this.Max, this.Current) = (last, max, current);
    
    public int? Current { get; set; }
    public int? Last { get; }
    public int? Max { get; }
}

internal class MultistepProgress<T> : IMultistepProgress<T>
{
    private readonly Action<(T value, OperationReport? operation, OperationReport? subOperation)> _onReporting;

    public MultistepProgress(Action<(T value, OperationReport? operation, OperationReport? subOperation)> onReporting) => this._onReporting = onReporting;

    public void Report(T value, OperationReport? operation = null, OperationReport? subOperation = null)
        => this._onReporting?.Invoke((value, operation, subOperation));
}

public interface IMultistepProgress<T>
{
    static IMultistepProgress<T> GetNew(Action<(T value, OperationReport? operation, OperationReport? subOperation)> onReporting)
        => new MultistepProgress<T>(onReporting);

    void Report(T value, OperationReport? operation = null, OperationReport? subOperation = null);
}

public interface IMultistepProgress : IMultistepProgress<string>
{
    static new IMultistepProgress GetNew(Action<(string value, OperationReport? operation, OperationReport? subOperation)> onReporting)
        => new MultistepProgress(onReporting);
}

internal class MultistepProgress : MultistepProgress<string>, IMultistepProgress
{
    public MultistepProgress(Action<(string value, OperationReport? operation, OperationReport? subOperation)> onReporting) : base(onReporting)
    {
    }
}
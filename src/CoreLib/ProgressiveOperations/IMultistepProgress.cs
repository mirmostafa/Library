namespace Library.ProgressiveOperations;

public interface IMultistepProgress<T>
{
    static IMultistepProgress<T> GetNew(Action<(T value, OperationReport? operation, OperationReport? subOperation)> onReporting)
        => new MultistepProgress<T>(onReporting);

    void Report(T value, OperationReport? operation = null, OperationReport? subOperation = null);
}

public record OperationReport(int? Step, int? Max)
{
    public static OperationReport New(int? step, int? max)
        => new(step, max);
    public OperationReport With(int? step)
        => New(step, this.Max);
    public void Deconstruct(out int? step, out int? max)
        => (step, max) = (this.Step, this.Max);

    public static implicit operator OperationReport((int? Step, int? Max) data)
        => New(data.Step, data.Max);
}

internal class MultistepProgress<T> : IMultistepProgress<T>
{
    private readonly Action<(T value, OperationReport? operation, OperationReport? subOperation)> _onReporting;

    public MultistepProgress(Action<(T value, OperationReport? operation, OperationReport? subOperation)> onReporting) => this._onReporting = onReporting;

    public void Report(T value, OperationReport? operation = null, OperationReport? subOperation = null) 
        => this._onReporting?.Invoke((value, operation, subOperation));
}
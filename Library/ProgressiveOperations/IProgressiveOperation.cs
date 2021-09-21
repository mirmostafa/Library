namespace Library.Coding.ProgressiveOperations;

public interface IProgressiveOperation<T>
{
    event EventHandler<ProgressiveOperationEventArgs<T>> ProgressChanged;
}

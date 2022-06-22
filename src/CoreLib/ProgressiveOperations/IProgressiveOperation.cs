namespace Library.ProgressiveOperations;

public interface IProgressiveOperation<T>
{
    event EventHandler<ProgressiveOperationEventArgs<T>> ProgressChanged;
}

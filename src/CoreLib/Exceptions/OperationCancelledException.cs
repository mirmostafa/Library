namespace Library.Exceptions;

[Serializable]
public sealed class OperationCancelledException : LibraryExceptionBase
{
    public OperationCancelledException()
    {
    }

    public OperationCancelledException(string message) : base(message)
    {
    }

    public OperationCancelledException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public OperationCancelledException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null) : base(message, instruction, title, details, inner, owner)
    {
    }
}

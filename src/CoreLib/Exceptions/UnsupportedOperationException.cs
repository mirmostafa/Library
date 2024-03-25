namespace Library.Exceptions;

[Serializable]
public sealed class UnsupportedOperationException : LibraryExceptionBase
{
    public UnsupportedOperationException()
    {
    }

    public UnsupportedOperationException(string message) : base(message)
    {
    }

    public UnsupportedOperationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public UnsupportedOperationException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null) : base(message, instruction, title, details, inner, owner)
    {
    }
}
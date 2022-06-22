namespace Library.Exceptions;

[Serializable]
public sealed class InvalidOperationException : LibraryExceptionBase
{
    public InvalidOperationException()
    {
    }

    public InvalidOperationException(string message) : base(message)
    {
    }

    public InvalidOperationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public InvalidOperationException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null) : base(message, instruction, title, details, inner, owner)
    {
    }
}
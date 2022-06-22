namespace Library.Exceptions;

[Serializable]
public sealed class InvalidArgumentException : LibraryExceptionBase
{
    public InvalidArgumentException()
    {
    }

    public InvalidArgumentException(string message) : base(message)
    {
    }

    public InvalidArgumentException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public InvalidArgumentException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null) : base(message, instruction, title, details, inner, owner)
    {
    }
}

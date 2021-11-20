namespace Library.Exceptions;

[Serializable]
public class DuplicateObjectFoundException : LibraryExceptionBase
{
    public DuplicateObjectFoundException()
        : this("Object not found")
    {
    }

    public DuplicateObjectFoundException(string message) : base(message)
    {
    }

    public DuplicateObjectFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public DuplicateObjectFoundException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null) : base(message, instruction, title, details, inner, owner)
    {
    }
}


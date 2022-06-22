namespace Library.Exceptions;

[Serializable]
public class ObjectNotFoundException : LibraryExceptionBase
{
    public ObjectNotFoundException()
        : this("Object not found.")
    {
    }

    public ObjectNotFoundException(string message) : base(message)
    {
    }

    public ObjectNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ObjectNotFoundException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null) : base(message, instruction, title, details, inner, owner)
    {
    }
}

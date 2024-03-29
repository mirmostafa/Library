namespace Library.Exceptions;

[Serializable]
public sealed class ObjectNotFoundException : LibraryExceptionBase
{
    public ObjectNotFoundException()
        : this("Object")
    {
    }

    public ObjectNotFoundException(string arg)
        : base($"{arg} not found.")
    {
    }

    public ObjectNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ObjectNotFoundException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null) : base(message, instruction, title, details, inner, owner)
    {
    }
}
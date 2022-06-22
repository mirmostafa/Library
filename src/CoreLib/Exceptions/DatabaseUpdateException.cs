namespace Library.Exceptions;

[Serializable]
public abstract class DatabaseUpdateExceptionBase : ExceptionBase
{
    protected DatabaseUpdateExceptionBase()
    {
    }

    protected DatabaseUpdateExceptionBase(string message)
        : base(message)
    {
    }

    protected DatabaseUpdateExceptionBase(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected DatabaseUpdateExceptionBase(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null)
        : base(message, instruction, title, details, inner, owner)
    {
    }

    protected DatabaseUpdateExceptionBase(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {

    }
}

[Serializable]
public sealed class DatabaseUpdateException : DatabaseUpdateExceptionBase, IException
{
    public DatabaseUpdateException()
    {
    }

    public DatabaseUpdateException(string message) : base(message)
    {
    }

    public DatabaseUpdateException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public DatabaseUpdateException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null) : base(message, instruction, title, details, inner, owner)
    {
    }

    private DatabaseUpdateException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {

    }
}

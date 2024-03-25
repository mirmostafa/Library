namespace Library.Exceptions;

[Serializable]
public abstract class DatabaseExceptionBase : ExceptionBase
{
    protected DatabaseExceptionBase()
    {
    }

    protected DatabaseExceptionBase(string message)
        : base(message)
    {
    }

    protected DatabaseExceptionBase(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected DatabaseExceptionBase(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null)
        : base(message, instruction, title, details, inner, owner)
    {
    }

    protected DatabaseExceptionBase(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {

    }
}

[Serializable]
public sealed class DatabaseUpdateException : DatabaseExceptionBase, IException
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

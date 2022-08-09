using System.Runtime.Serialization;

namespace Library.Exceptions.Validations;

[Serializable]
public sealed class NoItemValidationException : ValidationExceptionBase
{
    public NoItemValidationException()
    {
    }

    public NoItemValidationException(string message) : base(message)
    {
    }

    public NoItemValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public NoItemValidationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }

    public NoItemValidationException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null) : base(message, instruction, title, details, inner, owner)
    {
    }

    [DoesNotReturn]
    public static void Throw(string message)
        => throw new NoItemValidationException(message);
}
using System.Runtime.Serialization;

namespace Library.Exceptions.Validations;

[Serializable]
public sealed class InvalidOperationValidationException : ValidationExceptionBase
{
    public InvalidOperationValidationException()
    {
    }

    public InvalidOperationValidationException(string message) : base(message)
    {
    }

    public InvalidOperationValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public InvalidOperationValidationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }

    public InvalidOperationValidationException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null) : base(message, instruction, title, details, inner, owner)
    {
    }
}

namespace Library.Exceptions.Validations;

[Serializable]
public sealed class ValidationException : ValidationExceptionBase
{
    public ValidationException()
    {
    }

    public ValidationException(string message)
        : base(message)
    {
    }

    public ValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public ValidationException(
        string message,
        string? instruction = null,
        string? title = null,
        string? details = null,
        Exception? inner = null,
        object? owner = null)
        : base(message, instruction, title, details, inner, owner)
    {
    }

    private ValidationException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
    {

    }
}

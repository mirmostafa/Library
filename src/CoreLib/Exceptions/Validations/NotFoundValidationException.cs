using System.Net;

namespace Library.Exceptions.Validations;

[System.Serializable]
public sealed class NotFoundValidationException : ValidationExceptionBase
{
    public static new int ErrorCode { get; } = HttpStatusCode.NotFound.ToInt() * -1;

    public NotFoundValidationException(string message)
        : base(message)
    {
    }

    public NotFoundValidationException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }

    public NotFoundValidationException()
    {
    }

    private NotFoundValidationException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
    {

    }

    public NotFoundValidationException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null) : base(message, instruction, title, details, inner, owner)
    {
    }
}

using System.Net;

namespace Library.Exceptions.Validations;

[Serializable]
public abstract class ValidationExceptionBase : LibraryExceptionBase, IValidationException
{
    public static int ErrorCode { get; } = HttpStatusCode.BadRequest.ToInt() * -1;

    protected ValidationExceptionBase()
    {
    }

    protected ValidationExceptionBase(string message)
        : base(message)
    {
    }

    protected ValidationExceptionBase(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected ValidationExceptionBase(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null)
        : base(message, instruction, title, details, inner, owner)
    {
    }

    protected ValidationExceptionBase(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
    {

    }
}

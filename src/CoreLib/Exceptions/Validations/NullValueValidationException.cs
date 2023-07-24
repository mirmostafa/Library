using System.Runtime.Serialization;

namespace Library.Exceptions.Validations;

[Serializable]
public sealed class NullValueValidationException : ValidationExceptionBase
{
    public NullValueValidationException(string valueName)
        : base($"{valueName} cannot be null", $"{valueName} is null.")
    {
    }

    public NullValueValidationException()
    {
    }

    public NullValueValidationException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public NullValueValidationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }

    public static new int ErrorCode { get; } = 620 * -1;
}
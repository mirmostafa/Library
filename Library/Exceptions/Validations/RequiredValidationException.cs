using System.Runtime.Serialization;

namespace Library.Exceptions.Validations;

[Serializable]
public sealed class RequiredValidationException : ValidationExceptionBase
{
    private const string _format = "{0} is required.";
    public RequiredValidationException()
    {
    }

    public RequiredValidationException(string fieldName)
        : base(string.Format(_format, fieldName))
    {
    }

    public RequiredValidationException(string fieldName, Exception innerException)
        : base(string.Format(_format, fieldName), innerException)
    {
    }

    public RequiredValidationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }

    public RequiredValidationException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null)
        : base(message, instruction, title, details, inner, owner)
    {
    }
}

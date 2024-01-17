namespace Library.Web.Exceptions;

[Serializable]
public sealed class ApiCommonException : ApiExceptionBase
{
    public ApiCommonException(int? statusCode = null, string? message = null, object? owner = null)
        : base(statusCode, message, owner)
    {
    }

    public ApiCommonException()
    {
    }

    public ApiCommonException(string message) : base(message)
    {
    }

    public ApiCommonException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
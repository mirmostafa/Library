using System.Net;

namespace Library.Web.Exceptions;

[Serializable]
public sealed class CannotCreateException : ApiExceptionBase
{
    public CannotCreateException(string? message = null, object? owner = null)
        : base(HttpStatusCode.BadRequest.Cast().ToInt(), message ?? "Cannot create.", owner)
    {
    }

    public CannotCreateException()
    {
    }

    public CannotCreateException(string message) : base(HttpStatusCode.BadRequest.Cast().ToInt(), message)
    {
    }

    public CannotCreateException(string message, Exception innerException) : base(HttpStatusCode.BadRequest.Cast().ToInt(), message, innerException)
    {
    }
}
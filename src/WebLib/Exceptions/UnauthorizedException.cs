using System.Net;

namespace Library.Web.Exceptions;

[Serializable]
public sealed class UnauthorizedException : ApiExceptionBase
{
    public UnauthorizedException(string? message = null, object? owner = null)
        : base(HttpStatusCode.Unauthorized.Cast().ToInt(), message ?? "شما مجاز به استفاده از این قسمت نمی‌باشید.", owner)
    {
    }

    public UnauthorizedException()
    {
    }

    public UnauthorizedException(string message) : base(HttpStatusCode.Unauthorized.Cast().ToInt(), message)
    {
    }

    public UnauthorizedException(string message, Exception innerException) : base(HttpStatusCode.Unauthorized.Cast().ToInt(), message, innerException)
    {
    }
}
using System.Net;

namespace Library.Web.Exceptions
{
    public sealed class UnauthorizedException : ApiExceptionBase
    {
        public UnauthorizedException(string? message = null, object? owner = null)
            : base(HttpStatusCode.Unauthorized.ToInt(), message ?? "شما مجاز به استفاده از این قسمت نمی‌باشید.", owner)
        {
        }

        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message) : base(HttpStatusCode.Unauthorized.ToInt(), message)
        {
        }

        public UnauthorizedException(string message, Exception innerException) : base(HttpStatusCode.Unauthorized.ToInt(), message, innerException)
        {
        }
    }
}
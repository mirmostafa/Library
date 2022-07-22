using System.Net;

namespace Library.Web.Exceptions
{
    public sealed class CannotCreareException : ApiExceptionBase
    {
        public CannotCreareException(string? message = null, object? owner = null)
            : base(HttpStatusCode.BadRequest.ToInt(), message ?? "Cannot create.", owner)
        {
        }

        public CannotCreareException()
        {
        }

        public CannotCreareException(string message) : base(HttpStatusCode.BadRequest.ToInt(), message)
        {
        }

        public CannotCreareException(string message, Exception innerException) : base(HttpStatusCode.BadRequest.ToInt(), message, innerException)
        {
        }
    }
}
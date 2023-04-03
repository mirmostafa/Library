using System.Net;

namespace Library.Web.Exceptions
{
    public sealed class ObjectNotFoundApiException : ApiExceptionBase
    {
        public ObjectNotFoundApiException(string? message = null, object? owner = null)
            : base(HttpStatusCode.NotFound.Cast().ToInt(), message ?? "No specific object found.", owner)
        {
        }

        public ObjectNotFoundApiException()
        {
        }

        public ObjectNotFoundApiException(string message) : base(HttpStatusCode.NotFound.Cast().ToInt(), message)
        {
        }

        public ObjectNotFoundApiException(string message, Exception innerException) : base(HttpStatusCode.NotFound.Cast().ToInt(), message, innerException)
        {
        }
    }
}
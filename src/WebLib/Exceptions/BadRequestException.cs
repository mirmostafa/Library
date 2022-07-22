using System.Net;

namespace Library.Web.Exceptions
{
    public sealed class BadRequestException : ApiExceptionBase
    {
        public BadRequestException(string? message = null, object? owner = null)
            : base(HttpStatusCode.BadRequest.ToInt(), message ?? "The request cannot be procceed.", owner)
        {
        }

        public BadRequestException()
        {
        }

        public BadRequestException(string message) : base(HttpStatusCode.NotFound.ToInt(), message)
        {
        }

        public BadRequestException(string message, Exception innerException) : base(HttpStatusCode.NotFound.ToInt(), message, innerException)
        {
        }
    }
}
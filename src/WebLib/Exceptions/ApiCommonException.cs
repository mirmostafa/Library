namespace Library.Web.Exceptions
{
    public sealed class ApiCommonException : ApiExceptionBase
    {
        public ApiCommonException(int? statusCode = null, string? message = null, object? owner = null)
            : base(statusCode, message, owner)
        {
        }
    }
}
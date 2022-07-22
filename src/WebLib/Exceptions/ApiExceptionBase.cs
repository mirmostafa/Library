using Library.Exceptions;

using System.Net;
using System.Web.Http;

namespace Library.Web.Exceptions;

public abstract class ApiExceptionBase : HttpResponseException, IApiException
{
    protected ApiExceptionBase(int? statusCode = null, string? message = null, object? owner = null)
        : base(new((HttpStatusCode?)statusCode ?? HttpStatusCode.BadRequest))
    {
        this.Message = message ?? string.Empty;
        this.StatusCode = statusCode;
        this.Owner = owner;
    }
    /// <inheritdoc/>
    public override string Message { get; }
    public int? StatusCode { get; }
    string? IException.Instruction { get; }
    public object? Owner { get; set; }
    public string? Title { get; }
    public string? Details { get; set; }
}

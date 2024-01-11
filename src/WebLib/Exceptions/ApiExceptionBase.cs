using System.Net;
using System.Web.Http;

using Library.Exceptions;

namespace Library.Web.Exceptions;

public abstract class ApiExceptionBase : HttpResponseException, IApiException
{
    protected ApiExceptionBase(HttpResponseMessage message) : base(message) => 
        this.Message = message?.ToString() ?? string.Empty;

    protected ApiExceptionBase(int? statusCode = null, string? message = null, object? owner = null)
        : base((HttpStatusCode?)statusCode ?? HttpStatusCode.BadRequest)
    {
        this.Message = message ?? string.Empty;
        this.StatusCode = statusCode;
        this.Owner = owner;
    }

    public string? Details { get; set; }

    string? IException.Instruction { get; }

    /// <inheritdoc/>
    public override string Message { get; }

    public object? Owner { get; set; }
    public int? StatusCode { get; }
    public string? Title { get; }
}
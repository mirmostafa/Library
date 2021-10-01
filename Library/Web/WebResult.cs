using System.Net;
using Library.Validations;

namespace Library.Web;

public class WebResult
{
    public HttpStatusCode? StatusCode { get; }
    public string? Message { get; private set; }
    public bool Success => this.StatusCode?.ToInt() is null or (>= 200 and < 300);
    public bool Failure => !this.Success;

    protected WebResult(HttpStatusCode? statusCode = null, string? message = null)
        => (this.StatusCode, this.Message) = (statusCode, message);

    public void Deconstruct(out HttpStatusCode? statusCode, out string? message)
        => (statusCode, message) = (this.StatusCode, this.Message);

    public static WebResult Ok() => new(HttpStatusCode.OK);
    public static WebResult Ok(string? message) => new(HttpStatusCode.OK, message);

    public static WebResult Fail(HttpStatusCode statusCode)
        => new(statusCode);
    public static WebResult Fail(HttpStatusCode statusCode, string? message)
        => new(statusCode, message);

    public static WebResult BadRequest() => Fail(HttpStatusCode.BadRequest);
    public static WebResult BadRequest(string? message) => Fail(HttpStatusCode.BadRequest, message);

    public static WebResult Unauthorized() => Fail(HttpStatusCode.Unauthorized);
    public static WebResult Unauthorized(string? message) => Fail(HttpStatusCode.Unauthorized, message);

    public static WebResult NotFound() => Fail(HttpStatusCode.NotFound);
    public static WebResult NotFound(string? message) => Fail(HttpStatusCode.NotFound, message);

    public static WebResult NoContent() => Fail(HttpStatusCode.NoContent);
    public static WebResult NoContent(string? message) => Fail(HttpStatusCode.NoContent, message);
}

public class WebResult<T> : WebResult
{
    protected WebResult(HttpStatusCode? httpStatusCode = null, string? message = null, T? value = default)
        : base(httpStatusCode, message) => this.Value = value;

    public void Deconstruct(out HttpStatusCode? statusCode, out string? message, out T? value)
        => (statusCode, message, value) = (this.StatusCode, this.Message, this.Value);

    private T? _value;
    public T? Value
    {
        get
        {
            Check.Require(this.Success);

            return this._value;
        }
        private set => this._value = value;
    }

    public static WebResult<T> Ok(T value) => new(HttpStatusCode.OK) { Value = value };
    public static WebResult<T> Ok(T value, string? message) => new(HttpStatusCode.OK, message) { Value = value };

    public static explicit operator T?(WebResult<T> result) => result.Value;
}

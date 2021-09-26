using System.Net;

namespace Library.Web;

public class Result
{
    public HttpStatusCode? StatusCode { get; }
    public string? Message { get; private set; }
    public bool Success => this.StatusCode?.ToInt() is null or (>= 200 and < 300);
    public bool Failure => !this.Success;

    protected Result(HttpStatusCode? statusCode = null, string? message = null)
        => (this.StatusCode, this.Message) = (statusCode, message);

    public void Deconstruct(out HttpStatusCode? statusCode, out string? message)
        => (statusCode, message) = (this.StatusCode, this.Message);

    public static Result Ok() => new(HttpStatusCode.OK);
    public static Result Ok(string? message) => new(HttpStatusCode.OK, message);

    public static Result Fail(HttpStatusCode statusCode) => new(statusCode);
    public static Result Fail(HttpStatusCode statusCode, string message) => new(statusCode, message);

    public static Result BadRequest() => new(HttpStatusCode.BadRequest);
    public static Result BadRequest(string? message) => new(HttpStatusCode.BadRequest, message);

    public static Result Unauthorized() => new(HttpStatusCode.Unauthorized);
    public static Result Unauthorized(string? message) => new(HttpStatusCode.Unauthorized, message);

    public static Result NotFound() => new(HttpStatusCode.NotFound);
    public static Result NotFound(string? message) => new(HttpStatusCode.NotFound, message);

    public static Result NoContent() => new(HttpStatusCode.NoContent);
    public static Result NoContent(string? message) => new(HttpStatusCode.NoContent, message);
}

public class Result<T> : Result
{
    protected Result(HttpStatusCode? httpStatusCode = null, string? message = null, T? value = default)
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

    public static Result<T> Ok(T value) => new(HttpStatusCode.OK) { Value = value };
    public static Result<T> Ok(T value, string? message) => new(HttpStatusCode.OK, message);

    public static explicit operator T?(Result<T> result) => result.Value;
}

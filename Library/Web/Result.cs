using System.Net;

namespace Library.Web;

public record Result
{
    public string? Error { get; private set; }
    public bool Success => this.StatusCode.ToInt() is >= 200 and < 300;
    public bool Failure => !this.Success;
    public HttpStatusCode StatusCode { get; set; }

    protected Result(string error) => this.Error = error;

    public static Result Fail(string message) => new(message);

    public static Result<T> Fail<T>(string message) => new(default, message);

    public static Result Ok() => new(string.Empty);

    public static Result<T> Ok<T>(T value) => new(value, string.Empty);

    public static Result Combine(params Result[] results)
    {
        foreach (var result in results)
        {
            if (result.Failure)
            {
                return result;
            }
        }

        return Ok();
    }
}


public record Result<T> : Result
{
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

    protected internal Result(T? value, string error)
        : base(error) => this.Value = value;

    public static implicit operator T?(in Result<T> result) => result.Value;
    public static implicit operator Result<T>(in T value) => Ok(value);
}

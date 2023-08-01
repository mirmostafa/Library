using System.Net;

using Library.Validations;

namespace Library.Web.Results;

public class ApiResult<T> : ApiResult, IApiResult<T>
{
    public ApiResult(int? httpStatusCode = null, string? message = null, T? value = default)
        : base(httpStatusCode, message) => this.Value = value;

    public ApiResult(HttpStatusCode? httpStatusCode = null, string? message = null, T? value = default)
        : base(httpStatusCode, message) => this.Value = value;

    public void Deconstruct(out object? statusCode, out string? message, out T? value)
        => (statusCode, message, value) = (this.Status, this.Message, this.Value);

    private T? _value;
    public T? Value
    {
        get
        {
            Check.MustBe(this.IsSucceed);

            return this._value;
        }
        private set => this._value = value;
    }

    public static ApiResult<T> Ok(T value, string? message = null)
        => new(System.Net.HttpStatusCode.OK.Cast().ToInt(), message) { Value = value };

    public static ApiResult<T?> New(int? statusCode = null, string? message = null, T? value = default)
        => new(statusCode, message, value);
    public static ApiResult<T?> New(HttpStatusCode? statusCode = null, string? message = null, T? value = default)
        => new(statusCode?.Cast().ToInt(), message, value);

    public static explicit operator T?(ApiResult<T> result) => result.Value;
}

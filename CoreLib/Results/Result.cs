using Library.Interfaces;

namespace Library.Results;

public class Result : ResultBase
{
    public Result(int? statusCode = null, string? message = null)
        : base(statusCode, message) { }

    public static Result Success => CreateSuccess();
    public static Result Fail => CreateFail();

    public static Result CreateFail(string? message = null, int? erroCode = -1) =>
        new(erroCode, message) { IsSucceed = false };
    public static Result CreateSuccess(string? message = null, int? statusCode = 0) =>
        new(statusCode, message) { IsSucceed = true };
}

public class Result<TValue> : ResultBase, IConvertible<Result<TValue>, Result>
{
    public TValue Value { get; }

    public Result(in TValue value, in int? statusCode = null, in string? message = null)
        : base(statusCode, message) =>
        this.Value = value;

    public static Result<TValue?> Fail =>
        CreateFail(errorCode: -1);

    [return: NotNull]
    public static Result<TValue> CreateFail(in string? message = null, in TValue value = default, in int? errorCode = -1) =>
        new(value, errorCode, message);

    [return: NotNull]
    public static Result<TValue> CreateSuccess(in TValue value, in string? message = null, in int? statusCode = 0) =>
        new(value, statusCode, message);

    public bool Equals(Result<TValue?> other) =>
        other is not null &&
        (other.StatusCode, other.IsSucceed) == (this.StatusCode, this.IsSucceed) &&
        (other.Value?.Equals(this.Value) ?? this.Value is null);

    public void Deconstruct(out int? StatusCode, out string? Message, out TValue Value) =>
        (StatusCode, Message, Value) = (this.StatusCode, this.Message, this.Value);
    public void Deconstruct(out bool isSucceed, out TValue Value) =>
        (isSucceed, Value) = (this.IsSucceed, this.Value);
    public Result Convert() =>
        this.IsSucceed ? Result.CreateSuccess(this.Message, this.StatusCode) : Result.CreateFail(this.Message, this.StatusCode);
    public static Result<TValue> Convert([DisallowNull] Result other) =>
        other.IsSucceed ? CreateSuccess(default, other.Message, other.StatusCode) : CreateFail(other.Message, errorCode: other.StatusCode);
    public Result<TValue1> With<TValue1>(TValue1 value1)
    {
        var result = new Result<TValue1>(value1, this.StatusCode, this.Message)
        {
            IsSucceed = this.IsSucceed
        };
        if (this.Errors.Any())
        {
            result.Errors.AddRange(this.Errors);
        }
        if (this.Extra.Any())
        {
            result.Extra.AddRange(this.Extra);
        }

        return result;
    }

    public static implicit operator TValue(in Result<TValue> result) =>
        result.Value;
    public static implicit operator bool(in Result<TValue> result) =>
        result.IsSucceed;
}

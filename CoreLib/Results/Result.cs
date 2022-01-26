using Library.Exceptions.Validations;
using Library.Interfaces;

namespace Library.Results;

public class Result : ResultBase
{
    public Result(object? statusCode = null, string? message = null)
        : base(statusCode, message) { }
    public static Result New() =>
        new();
    public static Result Success => CreateSuccess();
    public static Result Fail => CreateFail();

    public static Result CreateFail(string? message = null, object? erroCode = null) =>
        new(erroCode ?? -1, message) { IsSucceed = false };
    public static Result CreateSuccess(string? message = null, object? statusCode = null) =>
        new(statusCode, message) { IsSucceed = true };
    public Result With(ResultBase other)
    {
        if (other == null)
        {
            return this;
        }
        if (!other.Message.IsNullOrEmpty())
        {
            this.Message = other.Message;
        }
        if (other.StatusCode is not null and not 0)
        {
            this.StatusCode = other.StatusCode;
        }
        this.Errors.AddRange(other.Errors);

        return this;
    }
}

public class Result<TValue> : ResultBase, IConvertible<Result<TValue>, Result>
{
    public TValue Value { get; }

    public Result(in TValue value, in object? statusCode = null, in string? message = null)
        : base(statusCode, message) =>
        this.Value = value;

    public static Result<TValue?> Fail =>
        CreateFail(errorCode: -1);

    [return: NotNull]
    public static Result<TValue> CreateFail(in string? message = null, in TValue value = default, in object? errorCode = null) =>
        new(value, errorCode ?? -1, message);

    [return: NotNull]
    public static Result<TValue> CreateSuccess(in TValue value, in string? message = null, in object? statusCode = null) =>
        new(value, statusCode, message);

    public bool Equals(Result<TValue?> other) =>
        other is not null &&
        (other.StatusCode, other.IsSucceed) == (this.StatusCode, this.IsSucceed) &&
        (other.Value?.Equals(this.Value) ?? this.Value is null);

    public void Deconstruct(out object? StatusCode, out string? Message, out TValue Value) =>
        (StatusCode, Message, Value) = (this.StatusCode, this.Message, this.Value);
    public void Deconstruct(out bool isSucceed, out TValue Value) =>
        (isSucceed, Value) = (this.IsSucceed, this.Value);
    public Result Convert() =>
        this.IsSucceed ? Result.CreateSuccess(this.Message, this.StatusCode) : Result.CreateFail(this.Message, this.StatusCode);
    public static Result<TValue> Convert([DisallowNull] Result other) =>
        Convert(other, default);

    public static Result<TValue> Convert([DisallowNull] Result other, TValue value) =>
        other.IsSucceed ? CreateSuccess(value, other.Message, other.StatusCode) : CreateFail(other.Message, errorCode: other.StatusCode);
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
    public static Result<TValue> New(TValue item) =>
        new(item);
    public Task<Result<TValue>> ToTask() =>
        Task.FromResult(this);
}

public static class ResultHelper
{
    public static TResult MustBe<TResult>(this TResult result, bool condition, in object errorMessage, object? errorId = null)
        where TResult : ResultBase
    {
        if (!condition)
        {
            result.Errors.Add((errorId, errorMessage));
        }

        return result;
    }
    public static TResult HaveValue<TResult>(this TResult result, object? obj, in object errorMessage, object? errorId = null)
        where TResult : ResultBase =>
        MustBe(result, obj is not null, errorMessage, errorId ?? NullValueValidationException.ErrorCode);

    public static TResult HaveValue<TResult>(this TResult result, string? obj, in object errorMessage, object? errorId = null)
        where TResult : ResultBase =>
        MustBe(result, !obj.IsNullOrEmpty(), errorMessage, errorId ?? NullValueValidationException.ErrorCode);
}

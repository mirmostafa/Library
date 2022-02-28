using Library.Exceptions.Validations;
using Library.Interfaces;
using System.Runtime.CompilerServices;

namespace Library.Results;

public class Result : ResultBase, IEmpty<Result>
{
    public Result(in object? statusCode = null, in string? message = null)
        : base(statusCode, message) { }
    public static Result New() =>
        new();
    public static Result Success => CreateSuccess();
    public static Result Fail => CreateFail();

    private static Result _empty;
    public static Result Empty { get; } = _empty ??= NewEmpty();

    public static Result CreateFail(in string? message = null, in object? erroCode = null) =>
        new(erroCode ?? -1, message) { IsSucceed = false };
    public static Result CreateSuccess(in string? message = null, in object? statusCode = null) =>
        new(statusCode, message) { IsSucceed = true };
    public Result With(in ResultBase other)
    {
        if (other == null)
        {
            return this;
        }
        if (!other.Message.IsNullOrEmpty())
        {
            if (other.IsSucceed && this.Message.IsNullOrEmpty())
            {
                this.Message = other.Message;
            }
            else
            {
                this.Errors.Add((-1, other.Message));
            }
        }
        if (this.StatusCode != other.StatusCode)
        {
            this.StatusCode = other.StatusCode;
        }
        this.Errors.AddRange(other.Errors);

        return this;
    }

    public static Result NewEmpty() => New();
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
    public Result ConvertTo() =>
        this.IsSucceed ? Result.CreateSuccess(this.Message, this.StatusCode) : Result.CreateFail(this.Message, this.StatusCode);
    public static Result<TValue> ConvertFrom([DisallowNull] Result other) =>
        ConvertFrom(other, default);

    public static Result<TValue> ConvertFrom([DisallowNull] in Result other, in TValue value)
    {
        var result = new Result<TValue>(value)
        {
            StatusCode = other.StatusCode,
            Message = other.Message,
        };
        result.Errors.AddRange(other.Errors);
        result.Extra.AddRange(other.Extra);
        return result;
    }
    public static Result<TValue1> ConvertFrom<TValue1>([DisallowNull] in ResultBase other, in TValue1 value)
    {
        var result = new Result<TValue1>(value)
        {
            StatusCode = other.StatusCode,
            Message = other.Message,
        };
        result.Errors.AddRange(other.Errors);
        result.Extra.AddRange(other.Extra);
        return result;
    }

    public Result<TValue1> With<TValue1>(in TValue1 value1) =>
        ConvertFrom(this, value1);

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
    public static TResult MustBe<TResult>(this TResult result, bool condition, in object? errorMessage, object? errorId = null)
        where TResult : ResultBase
    {
        if (!condition)
        {
            result.Errors.Add((errorId, errorMessage ?? string.Empty));
        }

        return result;
    }
    public static TResult MustBe<TResult>(this TResult result, in (bool condition, object? errorMessage, object? errorId) situation)
        where TResult : ResultBase => MustBe(result, situation.condition, situation.errorMessage, situation.errorId);
    public static TResult MustBe<TResult>(this TResult result, in (bool condition, object? errorMessage) situation)
        where TResult : ResultBase => MustBe(result, situation.condition, situation.errorMessage, null);

    public static TResult HasValue<TResult>(this TResult result, object? obj, in object? errorMessage, object? errorId = null)
    where TResult : ResultBase =>
        MustBe(result, obj is not null, errorMessage, errorId ?? NullValueValidationException.ErrorCode);

    public static TResult HasValue<TResult>(this TResult result, string? obj, in object errorMessage, object? errorId = null)
        where TResult : ResultBase =>
        MustBe(result, !obj.IsNullOrEmpty(), errorMessage, errorId ?? NullValueValidationException.ErrorCode);

    public static TResult MustHaveValue<TResult>(this TResult result, string? obj, [CallerArgumentExpression("obj")] in string? argName = null)
        where TResult : ResultBase =>
        MustBe(result, !obj.IsNullOrEmpty(), $"{argName} cannot be empty.", NullValueValidationException.ErrorCode);
}

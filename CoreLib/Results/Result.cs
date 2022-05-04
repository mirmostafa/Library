using System.Runtime.CompilerServices;
using Library.Dynamic;
using Library.Exceptions;
using Library.Exceptions.Validations;
using Library.Interfaces;

namespace Library.Results;

public class Result : ResultBase, IEmpty<Result>
{
    private static readonly dynamic _staticFields = new Expando();

    public Result(in object? statusCode = null, in FullMessage? fullMessage = null)
        : base(statusCode, fullMessage) { }

    public static Result Empty { get; } = _staticFields.Empty ??= NewEmpty();
    public static Result Fail => _staticFields.Fail ??= CreateFail();
    public static Result Success => _staticFields.Success ??= CreateSuccess();

    public static Result CreateFail(in string? message = null, in object? erroCode = null)
        => new(erroCode ?? -1, message) { IsSucceed = false };

    public static Result CreateFail(in FullMessage? message, in object? erroCode = null)
        => new(erroCode ?? -1, message) { IsSucceed = false };

    public static Result CreateFail(string message, string instruction, string tiltle, string details, in object? statusCode = null) =>
        new(statusCode, new FullMessage(message, instruction, tiltle, details));

    public static Result CreateSuccess(in FullMessage? fullMessage = null, in object? statusCode = null)
            => new(statusCode, fullMessage) { IsSucceed = true };

    public static implicit operator bool(Result result!!) => result.IsSucceed;

    public static explicit operator Result(bool b) => b ? Success : Fail;

    public static Result New()
            => new();

    public static Result NewEmpty()
        => New();
}

public class Result<TValue> : ResultBase, IConvertible<Result<TValue?>, Result>
{
    public Result(in TValue value, in object? statusCode = null, in string? message = null)
        : base(statusCode, message)
        => this.Value = value;

    public Result(in TValue value, object? statusCode, FullMessage? fullMessage)
        : base(statusCode, fullMessage)
        => this.Value = value;

    public Result(in TValue value, object? statusCode, [DisallowNull] IException exception)
        : base(statusCode, exception)
        => this.Value = value;

    public static Result<TValue?> Fail
        => CreateFail(errorCode: -1);

    public TValue Value { get; }

    public static Result<TValue?> ConvertFrom([DisallowNull] Result other)
        => ConvertFrom(other, default);

    public static Result<TValue?> ConvertFrom([DisallowNull] in Result other, in TValue? value)
    {
        var result = new Result<TValue?>(value)
        {
            StatusCode = other.StatusCode,
            FullMessage = other.FullMessage,
        };
        result.Errors.AddRange(other.Errors);
        _ = result.Extra.AddRange(other.Extra);
        return result;
    }

    public static Result<TValue1> ConvertFrom<TValue1>([DisallowNull] in ResultBase other, in TValue1 value)
    {
        var result = new Result<TValue1>(value)
        {
            StatusCode = other.StatusCode,
            FullMessage = other.FullMessage,
        };
        result.Errors.AddRange(other.Errors);
        _ = result.Extra.AddRange(other.Extra);
        return result;
    }

    [return: NotNull]
    public static Result<TValue?> CreateFail(in string? message = null, in TValue? value = default, in object? errorCode = null)
        => new(value, errorCode ?? -1, message);

    [return: NotNull]
    public static Result<TValue?> CreateSuccess(in TValue value, in string? message = null, in object? statusCode = null)
        => new(value, statusCode, message);

    public static implicit operator bool(in Result<TValue?> result)
        => result.IsSucceed;

    public static implicit operator TValue(in Result<TValue> result)
        => result.Value;

    public static Result<TValue> New(TValue item)
        => new(item);

    public Result ConvertTo()
        => this.IsSucceed ? Result.CreateSuccess(this.FullMessage, this.StatusCode) : Result.CreateFail(this.FullMessage, this.StatusCode);

    public void Deconstruct(out object? StatusCode, out FullMessage? Message, out TValue Value)
        => (StatusCode, Message, Value) = (this.StatusCode, this.FullMessage, this.Value);

    public void Deconstruct(out bool isSucceed, out TValue Value)
        => (isSucceed, Value) = (this.IsSucceed, this.Value);

    public bool Equals(Result<TValue?> other)
        => other is not null && (other.StatusCode, other.IsSucceed) == (this.StatusCode, this.IsSucceed) && (other.Value?.Equals(this.Value) ?? this.Value is null);

    public Task<Result<TValue>> ToTask()
        => Task.FromResult(this);

    public Result<TValue1> With<TValue1>(in TValue1 value1)
        => ConvertFrom(this, value1);
}

public static class ResultHelper
{
    public static TResult HasValue<TResult>(this TResult result, object? obj, in object? errorMessage, object? errorId = null)
        where TResult : ResultBase
        => MustBe(result, obj is not null, errorMessage, errorId ?? NullValueValidationException.ErrorCode);

    public static TResult HasValue<TResult>(this TResult result, string? obj, in object errorMessage, object? errorId = null)
        where TResult : ResultBase
        => MustBe(result, !obj.IsNullOrEmpty(), errorMessage, errorId ?? NullValueValidationException.ErrorCode);

    public static bool IsValid<TValue>([NotNullWhen(true)] this Result<TValue> result)
        => result is not null and { IsSucceed: true } and { Value: not null };

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
        where TResult : ResultBase
        => MustBe(result, situation.condition, situation.errorMessage, situation.errorId);

    public static TResult MustBe<TResult>(this TResult result, in (bool condition, object? errorMessage) situation)
        where TResult : ResultBase
        => MustBe(result, situation.condition, situation.errorMessage, null);

    public static TResult MustHaveValue<TResult>(this TResult result, string? obj, [CallerArgumentExpression("obj")] in string? argName = null)
        where TResult : ResultBase
        => MustBe(result, !obj.IsNullOrEmpty(), $"{argName} cannot be empty.", NullValueValidationException.ErrorCode);

    public static ivalidationResult Validate<TValue>(this Result<TValue> result)
        => IsValid(result) ? valid.Result : invalid.Result;
}
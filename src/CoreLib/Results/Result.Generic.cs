using Library.Exceptions;
using Library.Interfaces;
using Library.Windows;

namespace Library.Results;

public class Result<TValue> : ResultBase, IConvertible<Result<TValue?>, Result>
{
    public Result(in TValue value, in object? statusCode = null, in string? message = null)
        : base(statusCode, message)
        => this.Value = value;

    public Result(in TValue value, object? statusCode, NotificationMessage? fullMessage)
        : base(statusCode, fullMessage)
        => this.Value = value;

    public Result(in TValue value, object? statusCode, [DisallowNull] IException exception)
        : base(statusCode, exception)
        => this.Value = value;

    public static Result<TValue?> Fail
        => CreateFail(errorCode: -1);

    public TValue Value { get; }

    [return: NotNull]
    public static Result<TValue?> CreateFail(in string? message = null, in TValue? value = default, in object? errorCode = null)
        => new(value, errorCode ?? -1, message);

    [return: NotNull]
    public static Result<TValue> CreateSuccess(in TValue value, in string? message = null, in object? statusCode = null)
        => new(value, statusCode, message);

    //public static Result<TValue1> From<TValue1>([DisallowNull] in ResultBase other, in TValue1 value)
    //{
    //    var result = new Result<TValue1>(value)
    //    {
    //        StatusCode = other.StatusCode,
    //        FullMessage = other.FullMessage,
    //    };
    //    result.Errors.AddRange(other.Errors);
    //    _ = result.Extra.AddRange(other.Extra);
    //    return result;
    //}

    public static Result<TValue> From<TValue1>([DisallowNull] in Result<TValue1> other, TValue value)
    {
        var result = new Result<TValue>(value)
        {
            StatusCode = other.StatusCode,
            FullMessage = other.FullMessage,
        };
        result.Errors.AddRange(other.Errors);
        _ = result.Extra.AddRange(other.Extra);
        return result;
    }

    public static Result<TValue> From([DisallowNull] in Result other, in TValue value)
    {
        var result = new Result<TValue>(value)
        {
            StatusCode = other.StatusCode,
            FullMessage = other.FullMessage,
        };
        result.Errors.AddRange(other.Errors);
        _ = result.Extra.AddRange(other.Extra);
        return result;
    }

    public static implicit operator bool(in Result<TValue?> result)
        => result.IsSucceed;

    public static implicit operator Result(in Result<TValue> result)
        => result.ConvertTo();

    public static implicit operator TValue(in Result<TValue> result)
        => result.Value;

    public static Result<TValue> New(TValue item)
        => new(item);

    public Result ConvertTo()
        => this.IsSucceed ? Result.CreateSuccess(this.FullMessage, this.StatusCode) : Result.CreateFail(this.FullMessage, this.StatusCode);

    public void Deconstruct(out object? StatusCode, out NotificationMessage? Message, out TValue Value)
        => (StatusCode, Message, Value) = (this.StatusCode, this.FullMessage, this.Value);

    public void Deconstruct(out bool isSucceed, out TValue Value)
        => (isSucceed, Value) = (this.IsSucceed, this.Value);

    public bool Equals(Result<TValue?> other)
        => other is not null && (other.StatusCode, other.IsSucceed) == (this.StatusCode, this.IsSucceed) && (other.Value?.Equals(this.Value) ?? this.Value is null);

    public Task<Result<TValue>> ToTask()
        => Task.FromResult(this);

    public Result<TValue1> With<TValue1>(in TValue1 value1)
        => Result<TValue1>.From(this, value1);

    static Result<TValue> IConvertible<Result<TValue>, Result>.From(Result other)
        => From(other, default!);
}
using Library.Exceptions;
using Library.Interfaces;
using Library.Windows;

namespace Library.Results;

public class Result<TValue> : ResultBase, IConvertible<Result<TValue?>, Result>
{
    public Result(in TValue value, in object? status = null, in string? message = null) : base(status, message)
        => this.Value = value;

    public Result(in TValue value, object? status, NotificationMessage? fullMessage) : base(status, fullMessage)
        => this.Value = value;

    public Result(in TValue value, object? status, [DisallowNull] IException exception) : base(status, exception)
        => this.Value = value;

    public static Result<TValue?> Fail
        => CreateFail(status: -1);

    public TValue Value { get; }

    [return: NotNull]
    public static Result<TValue?> CreateFail(in string? message = null, in TValue? value = default, in object? status = null)
        => new(value, status ?? -1, message);

    [return: NotNull]
    public static Result<TValue> CreateSuccess(in TValue value, in string? message = null, in object? status = null)
        => new(value, status, message);

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
            Status = other.Status,
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
            Status = other.Status,
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
        => this.IsSucceed ? Result.CreateSuccess(this.FullMessage, this.Status) : Result.CreateFail(this.FullMessage, this.Status);

    public void Deconstruct(out object? Status, out NotificationMessage? Message, out TValue Value)
        => (Status, Message, Value) = (this.Status, this.FullMessage, this.Value);

    public void Deconstruct(out bool isSucceed, out TValue Value)
        => (isSucceed, Value) = (this.IsSucceed, this.Value);

    public bool Equals(Result<TValue?> other)
        => other is not null && (other.Status, other.IsSucceed) == (this.Status, this.IsSucceed) && (other.Value?.Equals(this.Value) ?? this.Value is null);

    public Task<Result<TValue>> ToTask()
        => Task.FromResult(this);

    public Result<TValue1> With<TValue1>(in TValue1 value1)
        => Result<TValue1>.From(this, value1);

    static Result<TValue> IConvertible<Result<TValue>, Result>.From(Result other)
        => From(other, default!);
}
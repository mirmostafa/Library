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

    public static Result<TValue> From<TValue1>([DisallowNull] in Result<TValue1> other, TValue value)
        => ResultBase.From(other, new Result<TValue>(value));

    public static Result<TValue> From([DisallowNull] in Result other, in TValue value)
        => ResultBase.From(other, new Result<TValue>(value));

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

    public Result<TValue1> ToResult<TValue1>(TValue1 value)
        => From(this, new Result<TValue1>(value));

    public Result<TValue1> ToResult<TValue1>(Func<Result<TValue>, TValue1> action)
        => From(this, new Result<TValue1>(action(this)));    
}
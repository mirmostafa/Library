using System.Diagnostics;
using System.Numerics;

using Library.DesignPatterns.Markers;
using Library.Interfaces;
using Library.Validations;

namespace Library.Results;

/// <summary>
/// Represents a result that encapsulates a value along with success status, status, message,
/// errors, and extra data.
/// </summary>
/// <typeparam name="TValue">The type of the encapsulated value.</typeparam>
[DebuggerStepThrough, StackTraceHidden]
[Immutable]
[Fluent]
public class Result<TValue> : ResultBase
    , IAdditionOperators<Result<TValue>, Result, Result<TValue>>
    , IAdditionOperators<Result<TValue>, Result<TValue>, Result<TValue>>
    , IEquatable<Result<TValue>>
    , ICombinable<Result<TValue>>
    , INew<Result<TValue>, Result<TValue>>
{
    public Result(
        TValue value,
        in bool? succeed = null,
        in string? message = null,
        in IEnumerable<Exception>? errors = null,
        in IEnumerable<object>? extraData = null,
        in ResultBase? innerResult = null)
        : base(succeed, message, errors, extraData, innerResult) =>
        this.Value = value;

    public Result(ResultBase original, TValue value)
        : base(original)
        => this.Value = value;

    //! Incomplete Abstraction 👃
    //x public static Result<TValue?> Failure => _failure ??= Fail();

    public TValue Value
    {
        [StackTraceHidden]
        [DebuggerStepThrough]
        get;
        [StackTraceHidden]
        [DebuggerStepThrough]
        init;
    }

    public static implicit operator Result(Result<TValue> result)
    {
        Check.MustBeArgumentNotNull(result);
        return new(result.IsSucceed, result.Message, result.Errors, result.ExtraData);
    }

    [StackTraceHidden]
    [DebuggerStepThrough]
    public static implicit operator Result<TValue>(TValue value) =>
        new(value);

    [StackTraceHidden]
    [DebuggerStepThrough]
    public static implicit operator TValue(Result<TValue> result) =>
        result.ArgumentNotNull().Value;

    public static Result<TValue> operator +(Result<TValue> left, Result right)
    {
        Check.MustBeArgumentNotNull(left);
        return new Result<TValue>(left, left.Value) { InnerResult = right };
    }

    public static Result<TValue> operator +(Result<TValue> left, Result<TValue> right)
    {
        Check.MustBeArgumentNotNull(left);
        return new Result<TValue>(left, left.Value) { InnerResult = right };
    }

    public Result<TValue> Combine(Result obj) =>
        this + obj;

    public Result<TValue> Combine(Result<TValue> obj) =>
        this + obj;

    public bool Equals(Result<TValue>? other) =>
            other is not null && this.GetHashCode() == other.GetHashCode();

    public override bool Equals(object? obj) =>
        this.Equals(obj as Result<TValue>);

    public override int GetHashCode() =>
        this.Value?.GetHashCode() ?? base.GetHashCode();

    public Result<TValue> SetMessage(string? message) =>
        new(this) { Message = message };

    /// <summary>
    /// Converts the current Result object to an asynchronous Task.
    /// </summary>
    public Task<Result<TValue>> ToAsync() =>
        Task.FromResult(this);

    public override string ToString() =>
        this.IsFailure ? base.ToString() : this.Value?.ToString() ?? base.ToString();
    public static Result<TValue> New(Result<TValue> arg)
        => new(arg, arg.Value);
}
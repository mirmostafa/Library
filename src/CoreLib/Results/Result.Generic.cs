using System.Diagnostics;

using Library.DesignPatterns.Markers;
using Library.Interfaces;
using Library.Validations;

namespace Library.Results;

/// <summary>
/// Represents a result that encapsulates a value along with success status, status, message,
/// errors, and extra data.
/// </summary>
/// <typeparam name="TValue">The type of the encapsulated value.</typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
[Immutable]
[Fluent]
public class Result<TValue> : ResultBase
    , IAdditionOperators<Result<TValue>, Result, Result<TValue>>
    , IAdditionOperators<Result<TValue>, Result<TValue>, Result<TValue>>
    , IEquatable<Result<TValue>>
    , ICombinable<Result<TValue>>
    , ICombinable<Result, Result<TValue>>
    , INew<Result<TValue>, Result<TValue>>
{
    public Result(
        TValue value,
        in bool? succeed = null,
        in object? status = null,
        in string? message = null,
        in IEnumerable<(object Id, object Error)>? errors = null,
        in IEnumerable<object>? extraData = null,
        in ResultBase? innerResult = null)
        : base(succeed, status, message, errors, extraData, innerResult) =>
        this.Value = value;

    public Result(ResultBase original, TValue value)
        : base(original) =>
        this.Value = value;

    //! Incomplete Abstraction 👃
    //x public static Result<TValue?> Failure => _failure ??= CreateFailure();

    public TValue Value
    {
        [StackTraceHidden]
        [DebuggerStepThrough]
        get;
        [StackTraceHidden]
        [DebuggerStepThrough]
        init;
    }

    public static Result<TValue> Combine(IEnumerable<Result<TValue>> results) =>
            results.Combine();

    /// <summary>
    /// Creates a new Result with the given parameters and a success value of false.
    /// </summary>
    /// <param name="value">The value of the Result.</param>
    /// <param name="status">The status of the Result.</param>
    /// <param name="message">The message of the Result.</param>
    /// <param name="errors">The errors of the Result.</param>
    /// <param name="extraData">The extra data of the Result.</param>
    /// <returns>A new Result with the given parameters and a success value of false.</returns>
    public static Result<TValue?> CreateFailure(in TValue? value = default,
        in object? status = null,
        in string? message = null,
        in IEnumerable<(object Id, object Error)>? errors = null,
        in IEnumerable<object>? extraData = null) =>
        new(value, false, status, message, errors, extraData);

    /// <summary>
    /// Creates a new Result with the given value, status, message, and error.
    /// </summary>
    /// <param name="value">The value of the Result.</param>
    /// <param name="status">The status of the Result.</param>
    /// <param name="message">The message of the Result.</param>
    /// <param name="error">The error of the Result.</param>
    /// <returns>A new Result with the given value, status, message, and error.</returns>
    public static Result<TValue?> CreateFailure(in TValue? value,
        in object? status,
        in string? message,
        in (object Id, object Error) error) =>
        new(value, false, status, message, EnumerableHelper.AsEnumerable(error), null);

    /// <summary>
    /// Creates a failure result with the specified message, exception and value.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="ex">The exception.</param>
    /// <param name="value">The value.</param>
    /// <returns>A failure result.</returns>
    public static Result<TValue?> CreateFailure(in string message, in Exception ex, in TValue? value) =>
        CreateFailure(value, ex, message);

    /// <summary>
    /// Creates a Result with a failure status and an Exception.
    /// </summary>
    /// <param name="error">The Exception to be stored in the Result.</param>
    /// <param name="value">The value to be stored in the Result.</param>
    /// <returns>A Result with a failure status and an Exception.</returns>
    public static Result<TValue?> CreateFailure(in Exception error) =>
        CreateFailure(default, error, null);

    /// <summary>
    /// Creates a Result with a failure status and an Exception.
    /// </summary>
    /// <param name="error">The Exception to be stored in the Result.</param>
    /// <param name="value">The value to be stored in the Result.</param>
    /// <returns>A Result with a failure status and an Exception.</returns>
    public static Result<TValue> CreateFailure(in Exception error, in TValue value) =>
        CreateFailure(value, null, null, error: (-1, error))!;

    /// <summary>
    /// Creates a Result with a failure status and an Exception.
    /// </summary>
    /// <param name="message">The message to be stored in the Result.</param>
    /// <param name="value">The value to be stored in the Result.</param>
    /// <returns>A Result with a failure status and an Exception.</returns>
    public static Result<TValue?> CreateFailure(in string message, in TValue? value) =>
        CreateFailure(value, null, message);

    /// <summary>
    /// Creates a new successful Result with the given value, status, message, errors and extra data.
    /// </summary>
    /// <param name="value">The value of the Result.</param>
    /// <param name="status">The status of the Result.</param>
    /// <param name="message">The message of the Result.</param>
    /// <param name="errors">The errors of the Result.</param>
    /// <param name="extraData">The extra data of the Result.</param>
    /// <returns>A new successful Result.</returns>
    [return: NotNull]
    public static Result<TValue> CreateSuccess(in TValue value,
        in object? status = null,
        in string? message = null,
        in IEnumerable<(object Id, object Error)>? errors = null,
        in IEnumerable<object>? extraData = null) =>
        new(value, true, status, message, errors, extraData);

    public static Result<TValue> From(in Result result, in TValue value)
    {
        Check.MustBeArgumentNotNull(result);
        return new(value, result.Succeed, result.Status, result.Message, result.Errors, result.ExtraData);
    }

    public static implicit operator Result(Result<TValue> result)
    {
        Check.MustBeArgumentNotNull(result);
        return new(result.Succeed, result.Status, result.Message, result.Errors, result.ExtraData);
    }

    [StackTraceHidden]
    [DebuggerStepThrough]
    public static implicit operator Result<TValue>(TValue value) =>
        new(value);

    [StackTraceHidden]
    [DebuggerStepThrough]
    public static implicit operator TValue(Result<TValue> result) =>
        result.ArgumentNotNull().Value;

    /// <summary>
    /// Creates a new Result object with the given value, succeed, status, message, errors, and extraData.
    /// </summary>
    public static Result<TValue> New(in TValue value,
        in bool? succeed = null,
        in object? status = null,
        in string? message = null,
        in IEnumerable<(object Id, object Error)>? errors = null,
        in IEnumerable<object>? extraData = null,
        in ResultBase? innerResult = null) =>
        new(value, succeed, status, message, errors, extraData, innerResult);

    public static Result<TValue> New(Result<TValue> arg) =>
        new(arg, arg.ArgumentNotNull().Value);

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
}
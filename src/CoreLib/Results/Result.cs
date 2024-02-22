using System.Diagnostics;
using System.Numerics;

using Library.DesignPatterns.Markers;
using Library.Interfaces;
using Library.Validations;

namespace Library.Results;

[DebuggerStepThrough]
[StackTraceHidden]
[Immutable]
[Fluent]
public sealed class Result : ResultBase
    , IEmpty<Result>
    , IAdditionOperators<Result, Result, Result>
    , IAdditionOperators<Result, ResultBase, Result>
    , IEquatable<Result>
    , ICombinable<Result>
    , ICombinable<ResultBase, Result>
{
    private static Result? _empty;
    private static Result? _failed;
    private static Result? _succeed;

    internal Result(
        in bool? succeed = null,
        in string? message = null,
        in IEnumerable<Exception>? errors = null,
        in IEnumerable<object>? extraData = null,
        in ResultBase? innerResult = null) : base(succeed, message, errors, extraData, innerResult)
    {
    }

    internal Result(ResultBase origin)
        : base(origin)
    {
    }

    /// <summary>
    /// Gets an empty result.
    /// </summary>
    /// <returns>An empty result.</returns>
    public static Result Empty => _empty ??= NewEmpty();

    /// <summary>
    /// Gets a Result object representing a failed operation.
    /// </summary>
    /// <returns>A Result object representing a failed operation.</returns>
    public static Result Failed => _failed ??= Fail();

    /// <summary>
    /// Get a new instance of the Result class representing a successful operation.
    /// </summary>
    /// <returns>A new instance of the Result class representing a successful operation.</returns>
    public static Result Succeed => _succeed ??= Success();

    public static explicit operator Result(bool b) =>
        b ? Succeed : Failed;

    public static Result operator +(Result left, Result right) =>
        new(left) { InnerResult = right };

    public static Result operator +(Result left, ResultBase right) =>
        new(left) { InnerResult = right };

    public static Result Add(in Result left, in Result right)
        => left + right;

    /// <summary>
    /// Creates a new Result object with a failure status.
    /// </summary>
    /// <param name="status">Optional status object.</param>
    /// <param name="message">Optional message string.</param>
    /// <param name="errors">Optional enumerable of (Id, Error) tuples.</param>
    /// <param name="extraData">Optional extra data dictionary.</param>
    /// <returns>A new Result object with a failure status.</returns>
    public static Result Fail(in object? status = null, in string? message = null, in IEnumerable<Exception>? errors = null, in IEnumerable<object>? extraData = null) =>
        new(false, message, errors: errors, extraData: extraData);

    public static Result Fail<TException>()
        where TException : Exception, new()
        => Fail(new TException());

    /// <summary>
    /// Creates a new Result object with a failure status and the specified message and errors.
    /// </summary>
    /// <param name="message">The message associated with the failure.</param>
    /// <param name="errors">The errors associated with the failure.</param>
    /// <returns>A new Result object with a failure status and the specified message and errors.</returns>
    public static Result Fail(in string? message, in IEnumerable<Exception>? errors) =>
        new(false, message, errors: errors);

    /// <summary>
    /// Creates a new Result object with a failure status and the specified message and error.
    /// </summary>
    /// <param name="message">The message to be included in the Result object.</param>
    /// <param name="error">The Exception to be included in the Result object.</param>
    /// <returns>A new Result object with a failure status and the specified message and error.</returns>
    public static Result Fail(in string? message, in Exception error) =>
        Fail(message: message, errors: [error]);

    /// <summary>
    /// Creates a failure result with the given exception and optional message.
    /// </summary>
    /// <param name="error">The exception to use for the failure result.</param>
    /// <returns>A failure result with the given exception.</returns>
    public static Result Fail(Exception error) =>
        Fail(message: null, error);

    /// <summary>
    /// Creates a new Result object with a success status.
    /// </summary>
    /// <param name="status">Optional status object.</param>
    /// <param name="message">Optional message string.</param>
    /// <returns>A new Result object with a success status.</returns>
    public static Result Success(in object? status = null, in string? message = null) =>
        new(true, message);

    public static Result<TValue> From<TValue>(Result result, TValue value) =>
        From(result, value);

    // <summary>
    /// Creates a new empty Result object.
    /// </summary>
    public static Result NewEmpty() =>
        new();

    public Result Combine(Result obj) =>
        this + obj;

    public Result Combine(ResultBase obj) =>
        this + obj;

    public bool Equals(Result? other) =>
                other is not null && other == this;

    public override bool Equals(object? obj) =>
        this.Equals(obj as Result);

    public override int GetHashCode() =>
        base.GetHashCode();

    /// <summary>
    /// Creates a new Result with the given parameters and a success value of false.
    /// </summary>
    /// <param name="value">The value of the Result.</param>
    /// <param name="message">The message of the Result.</param>
    /// <param name="errors">The errors of the Result.</param>
    /// <param name="extraData">The extra data of the Result.</param>
    /// 
    /// <returns>A new Result with the given parameters and a success value of false.</returns>
    public static Result<TValue?> Fail<TValue>(in TValue? value = default,
        in string? message = null,
        in IEnumerable<Exception>? errors = null,
        in IEnumerable<object>? extraData = null) =>
        new(value, false, message, errors, extraData);

    /// <summary>
    /// Creates a new Result with the given value, status, message, and error.
    /// </summary>
    /// <param name="value">The value of the Result.</param>
    /// <param name="message">The message of the Result.</param>
    /// <param name="error">The error of the Result.</param>
    /// 
    /// <returns>A new Result with the given value, status, message, and error.</returns>
    public static Result<TValue?> Fail<TValue>(in TValue? value,
        in string? message,
        in Exception error) =>
        new(value, false, message, EnumerableHelper.AsEnumerable(error), null);

    /// <summary>
    /// Creates a Result with a failure status and an Exception.
    /// </summary>
    /// <param name="error">The Exception to be stored in the Result.</param>
    /// <param name="value">The value to be stored in the Result.</param>
    /// <returns>A Result with a failure status and an Exception.</returns>
    public static Result<TValue?> Fail<TValue>(in Exception error) => Fail<TValue>(default, null, error: error);

    public static Result<TValue?> Fail<TValue, TException>()
        where TException : Exception, new() => Fail<TValue>(new TException());

    /// <summary>
    /// Creates a Result with a failure status and an Exception.
    /// </summary>
    /// <param name="value">The value to be stored in the Result.</param>
    /// <param name="error">The Exception to be stored in the Result.</param>
    /// <returns>A Result with a failure status and an Exception.</returns>
    public static Result<TValue> Fail<TValue>(in TValue value, in Exception error)
        => Fail<TValue>(value, null, error: error)!;

    /// <summary>
    /// Creates a Result with a failure status and an Exception.
    /// </summary>
    /// <param name="message">The message to be stored in the Result.</param>
    /// <param name="value">The value to be stored in the Result.</param>
    /// <returns>A Result with a failure status and an Exception.</returns>
    public static Result<TValue?> Fail<TValue>(in string message, in TValue? value)
        => Fail<TValue>(value, message);

    /// <summary>
    /// Creates a new successful Result with the given value, status, message, errors and extra data.
    /// </summary>
    /// <param name="value">The value of the Result.</param>
    /// <param name="message">The message of the Result.</param>
    /// <param name="errors">The errors of the Result.</param>
    /// <param name="extraData">The extra data of the Result.</param>
    /// 
    /// <returns>A new successful Result.</returns>
    [return: NotNull]
    public static Result<TValue> Success<TValue>(in TValue value,
        in string? message = null,
        in IEnumerable<Exception>? errors = null,
        in IEnumerable<object>? extraData = null) =>
        new(value, true, message, errors, extraData);

    public static Result<TValue> From<TValue>(in Result result, in TValue value)
    {
        Check.MustBeArgumentNotNull(result);
        return new(value, result.IsSucceed, result.Message, result.Errors, result.ExtraData);
    }

    public static Result<TValue> New<TValue>(Result<TValue> arg) =>
        new(arg, arg.ArgumentNotNull().Value);
}
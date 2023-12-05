using Library.DesignPatterns.Markers;
using Library.Interfaces;

namespace Library.Results;

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
    private static Result? _fail;
    private static Result? _success;

    public Result(
        in bool? succeed = null,
        in object? status = null,
        in string? message = null,
        in IEnumerable<(object Id, object Error)>? errors = null,
        in IEnumerable<object>? extraData = null,
        in ResultBase? innerResult = null) : base(succeed, status, message, errors, extraData, innerResult)
    {
    }

    public Result(ResultBase origin)
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
    public static Result Failure => _fail ??= CreateFailure();

    /// <summary>
    /// Get a new instance of the Result class representing a successful operation.
    /// </summary>
    /// <returns>A new instance of the Result class representing a successful operation.</returns>
    public static Result Success => _success ??= CreateSuccess();

    public static Result Add(Result left, Result right) =>
        left + right;

    /// <summary>
    /// Creates a new Result object with a failure status.
    /// </summary>
    /// <param name="status">Optional status object.</param>
    /// <param name="message">Optional message string.</param>
    /// <param name="errors">Optional enumerable of (Id, Error) tuples.</param>
    /// <param name="extraData">Optional extra data dictionary.</param>
    /// <returns>A new Result object with a failure status.</returns>
    public static Result CreateFailure(in object? status = null, in string? message = null, in IEnumerable<(object Id, object Error)>? errors = null, in IEnumerable<object>? extraData = null) =>
        new(false, status, message, errors: errors, extraData);

    /// <summary>
    /// Creates a new Result object with a failure status and the specified message and errors.
    /// </summary>
    /// <param name="message">The message associated with the failure.</param>
    /// <param name="errors">The errors associated with the failure.</param>
    /// <returns>A new Result object with a failure status and the specified message and errors.</returns>
    public static Result CreateFailure(in string? message, in IEnumerable<(object Id, object Error)>? errors) =>
        new(false, null, message, errors: errors);

    /// <summary>
    /// Creates a new Result object with a failure status and the specified message and error.
    /// </summary>
    /// <param name="message">The message to be included in the Result object.</param>
    /// <param name="error">The Exception to be included in the Result object.</param>
    /// <returns>A new Result object with a failure status and the specified message and error.</returns>
    public static Result CreateFailure(in string? message, in Exception error) =>
        CreateFailure(message: message, errors: [(-1, error)]);

    /// <summary>
    /// Creates a failure result with the given exception and optional message.
    /// </summary>
    /// <param name="error">The exception to use for the failure result.</param>
    /// <returns>A failure result with the given exception.</returns>
    public static Result CreateFailure(Exception error) =>
        CreateFailure(null, error);

    /// <summary>
    /// Creates a new Result object with a success status.
    /// </summary>
    /// <param name="status">Optional status object.</param>
    /// <param name="message">Optional message string.</param>
    /// <returns>A new Result object with a success status.</returns>
    public static Result CreateSuccess(in object? status = null, in string? message = null) =>
        new(true, status, message);

    public static explicit operator Result(bool b) =>
        b ? Success : Failure;

    public static Result<TValue> From<TValue>(Result result, TValue value) =>
        Result<TValue>.From(result, value);

    // <summary>
    /// Creates a new empty Result object. </summary>
    public static Result NewEmpty() =>
        new();

    public static Result operator +(Result left, Result right) =>
        new(left) { InnerResult = right };

    public static Result operator +(Result left, ResultBase right) =>
        new(left) { InnerResult = right };

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
}
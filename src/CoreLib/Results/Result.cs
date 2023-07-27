using System.Collections.Immutable;

using Library.Interfaces;

namespace Library.Results;

public record Result(in bool? Succeed = null,
                     in object? Status = null,
                     in string? Message = null,
                     in IEnumerable<(object Id, object Error)>? Errors = null,
                     in ImmutableDictionary<string, object>? ExtraData = null)
    : ResultBase(Succeed, Status, Message, Errors, ExtraData)
    , IEmpty<Result>
    , IAdditionOperators<Result, Result, Result>
    , IEquatable<Result>
{
    private static Result? _empty;
    private static Result? _fail;
    private static Result? _success;

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

    /// <summary>
    /// Creates a new Result object with a failure status.
    /// </summary>
    /// <param name="status">Optional status object.</param>
    /// <param name="message">Optional message string.</param>
    /// <param name="errors">Optional enumerable of (Id, Error) tuples.</param>
    /// <param name="extraData">Optional extra data dictionary.</param>
    /// <returns>A new Result object with a failure status.</returns>
    public static Result CreateFailure(in object? status = null, in string? message = null, in IEnumerable<(object Id, object Error)>? errors = null, in ImmutableDictionary<string, object>? extraData = null) =>
        new(false, status, message, Errors: errors, extraData);

    /// <summary>
    /// Creates a new Result object with a failure status and the specified message and errors.
    /// </summary>
    /// <param name="message">The message associated with the failure.</param>
    /// <param name="errors">The errors associated with the failure.</param>
    /// <returns>A new Result object with a failure status and the specified message and errors.</returns>
    public static Result CreateFailure(in string? message, in IEnumerable<(object Id, object Error)>? errors) =>
        new(false, null, message, Errors: errors);

    /// <summary>
    /// Creates a new Result object with a success status.
    /// </summary>
    /// <param name="status">Optional status object.</param>
    /// <param name="message">Optional message string.</param>
    /// <returns>A new Result object with a success status.</returns>
    public static Result CreateSuccess(in object? status = null, in string? message = null) =>
        new(true, status, message);

    /// <summary>
    /// Creates a new empty Result object.
    /// </summary>
    public static Result NewEmpty() =>
        new();

    public static explicit operator Result(bool b) =>
        b ? Success : Failure;

    public static Result operator +(Result left, Result right)
    {
        var total = Combine(left, right);
        var result = new Result(total.Succeed, total.Status, total.Message, total.Errors, total.ExtraData);
        return result;
    }

    /// <summary>
    /// Creates a new Result object with a failure status and the specified message and error.
    /// </summary>
    /// <param name="message">The message to be included in the Result object.</param>
    /// <param name="error">The Exception to be included in the Result object.</param>
    /// <returns>A new Result object with a failure status and the specified message and error.</returns>
    public static Result CreateFailure(in string message, in Exception error) =>
        CreateFailure(error, message);

    /// <summary>
    /// Creates a failure result with the given exception and optional message.
    /// </summary>
    /// <param name="error">The exception to use for the failure result.</param>
    /// <returns>A failure result with the given exception.</returns>
    public static Result CreateFailure(Exception error) =>
        CreateFailure(error, null);

    /// <summary>
    /// Combines multiple Result objects into a single Result object.
    /// </summary>
    /// <param name="results">The Result objects to combine.</param>
    /// <returns>A single Result object containing the combined data.</returns>
    public static Result Combine(params Result[] results)
    {
        var data = ResultBase.Combine(results);
        var result = new Result(data.Succeed, data.Status, data.Message, data.Errors, data.ExtraData);
        return result;
    }
}
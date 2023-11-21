using System.Diagnostics;

using Library.Interfaces;
using Library.Validations;

namespace Library.Results;

/// <summary>
/// Represents a result that encapsulates a value along with success status, status, message,
/// errors, and extra data.
/// </summary>
/// <typeparam name="TValue">The type of the encapsulated value.</typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
public class Result<TValue>(
    TValue value,
    in bool? succeed = null,
    in object? status = null,
    in string? message = null,
    in IEnumerable<(object Id, object Error)>? errors = null,
    in IEnumerable<(string Id, object Data)>? extraData = null)
    : ResultBase(succeed, status, message, errors, extraData)
    , IAdditionOperators<Result<TValue>, Result, Result<TValue>>
    , IEquatable<Result<TValue>>
{
    private static Result<TValue?>? _failure;

    public Result(ResultBase original, TValue value)
        : this(value, original.ArgumentNotNull().Succeed, original.Status, original.Message, original.Errors, original.ExtraData)
    {
    }

    /// <summary>
    /// Gets a Result object representing a failed operation.
    /// </summary>
    /// <returns>A Result object representing a failed operation.</returns>
    public static Result<TValue?> Failure => _failure ??= CreateFailure();

    public TValue Value
    {
        [StackTraceHidden]
        [DebuggerStepThrough]
        get;
        [StackTraceHidden]
        [DebuggerStepThrough]
        init;
    } = value;

    /// <summary>
    /// Combines multiple <see cref="Result{TValue}"/> objects by applying a specified addition
    /// operation to their values.
    /// </summary>
    /// <param name="results">An array of <see cref="Result{TValue}"/> objects to be combined.</param>
    /// <param name="add">The addition operation to be applied to the values.</param>
    /// <returns>
    /// A combined <see cref="Result{TValue}"/> object with the sum of the values and other combined properties.
    /// </returns>
    public static Result<TValue> Combine(IEnumerable<Result<TValue>> results, Func<TValue, TValue, TValue> add) =>
        Combine(add, results.ToArray());

    /// <summary>
    /// Combines multiple Result objects by applying a specified addition operation to their values.
    /// </summary>
    /// <param name="add">The addition operation to be applied to the values.</param>
    /// <param name="resultArray">An array of Result objects to be combined.</param>
    /// <returns>A combined Result object with the sum of the values and other combined properties.</returns>
    public static Result<TValue> Combine(Func<TValue, TValue, TValue> add, params Result<TValue>[] resultArray)
    {
        Check.MustBeArgumentNotNull(add);
        Checker.MustBe(resultArray is not null and { Length: > 0 }, () => $"{nameof(resultArray)} cannot be empty.");

        // Combine the Result<TValue> objects to get a combined Tuple object.
        var combine = Combine(resultArray);

        // Extract the values from the Result<TValue> objects.
        var valueArray = resultArray.Select(x => x.Value).ToArray();
        var value = valueArray[0]; // Initialize the combined value with the first value.

        // Apply the addition operation to combine the values, using auxiliary functions, given by
        // the caller.
        foreach (var v in valueArray.Skip(1))
        {
            value = add(value, v); // Apply the addition operation to the current value and the next value.
        }

        // Create and return a new Result<TValue> object with the combined value and other combined properties.
        return new Result<TValue>(value, combine.Succeed, combine.Status, combine.Message, combine.Errors, combine.ExtraData);
    }

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
        in IEnumerable<(string Id, object Data)>? extraData = null) =>
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
        new(value, false, status, message, EnumerableHelper.Iterate(error), null);

    /// <summary>
    /// Creates a failure result with the specified message, exception and value.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="ex">The exception.</param>
    /// <param name="value">The value.</param>
    /// <returns>A failure result.</returns>
    public static Result<TValue?> CreateFailure(in string message, in Exception ex, in TValue? value)
        => CreateFailure(value, ex, message);

    /// <summary>
    /// Creates a Result with a failure status and an Exception.
    /// </summary>
    /// <param name="error">The Exception to be stored in the Result.</param>
    /// <param name="value">The value to be stored in the Result.</param>
    /// <returns>A Result with a failure status and an Exception.</returns>
    public static Result<TValue?> CreateFailure(in Exception error)
        => CreateFailure(default, error, null);

    /// <summary>
    /// Creates a Result with a failure status and an Exception.
    /// </summary>
    /// <param name="error">The Exception to be stored in the Result.</param>
    /// <param name="value">The value to be stored in the Result.</param>
    /// <returns>A Result with a failure status and an Exception.</returns>
    public static Result<TValue> CreateFailure(in Exception error, in TValue value) =>
        CreateFailure(value, error, null)!;

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
        in IEnumerable<(string Id, object Data)>? extraData = null) =>
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
        in IEnumerable<(string Id, object Data)>? extraData = null) =>
        new(value, succeed, status, message, errors, extraData);

    public static Result<TValue> operator +(Result<TValue> left, Result right)
    {
        Check.MustBeArgumentNotNull(left);
        var total = Combine(left, right);
        return new Result<TValue>(left.Value, total.Succeed, total.Status, total.Message, total.Errors, total.ExtraData);
    }

    public Result<TValue> Add(Result<TValue> item, Func<TValue, TValue, TValue> add) =>
            Result<TValue>.Combine(add, item);

    public bool Equals(Result<TValue>? other) => throw new NotImplementedException();

    public override bool Equals(object? obj) =>
        this.Equals(obj as Result<TValue>);

    public override int GetHashCode() =>
        this.Value?.GetHashCode() ?? -1;

    /// <summary>
    /// Converts the current Result object to an asynchronous Task.
    /// </summary>
    public Task<Result<TValue>> ToAsync() =>
        Task.FromResult(this);

    public override string ToString() =>
        this.IsFailure ? base.ToString() : this.Value?.ToString() ?? base.ToString();
}
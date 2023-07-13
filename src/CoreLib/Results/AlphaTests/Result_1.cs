using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using Library.Interfaces;
using Library.Validations;

namespace Library.AlphaTests;

public class Result(in bool? succeed = null,
                     in object? status = null,
                     in string? message = null,
                     in IEnumerable<(object Id, object Error)>? errors = null,
                     in ImmutableDictionary<string, object>? extraData = null)
    : ResultBase(succeed, status, message, errors, extraData),
    IEmpty<Result>,
    IAdditionOperators<Result, Result, Result>,
    IEquatable<Result>
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

    /// <summary>
    /// Creates a new Result object with a failure status.
    /// </summary>
    /// <param name="status">Optional status object.</param>
    /// <param name="message">Optional message string.</param>
    /// <param name="errors">Optional enumerable of (Id, Error) tuples.</param>
    /// <param name="extraData">Optional extra data dictionary.</param>
    /// <returns>A new Result object with a failure status.</returns>
    public static Result CreateFailure(in object? status = null, in string? message = null, in IEnumerable<(object Id, object Error)>? errors = null, in ImmutableDictionary<string, object>? extraData = null)
        => new(false, status, message, errors: errors, extraData);

    /// <summary>
    /// Creates a new Result object with a failure status and the specified message and errors.
    /// </summary>
    /// <param name="message">The message associated with the failure.</param>
    /// <param name="errors">The errors associated with the failure.</param>
    /// <returns>A new Result object with a failure status and the specified message and errors.</returns>
    public static Result CreateFailure(in string? message, in IEnumerable<(object Id, object Error)>? errors)
        => new(false, null, message, errors: errors);

    /// <summary>
    /// Creates a new Result object with a failure status and the specified message and error.
    /// </summary>
    /// <param name="message">The message to be included in the Result object.</param>
    /// <param name="error">The Exception to be included in the Result object.</param>
    /// <returns>A new Result object with a failure status and the specified message and error.</returns>
    public static Result CreateFailure(in string message, in Exception error)
        => CreateFailure(error, message);

    /// <summary>
    /// Creates a failure result with the given exception and optional message.
    /// </summary>
    /// <param name="error">The exception to use for the failure result.</param>
    /// <returns>A failure result with the given exception.</returns>
    public static Result CreateFailure(Exception error)
        => CreateFailure(error, null);

    /// <summary>
    /// Creates a new Result object with a success status.
    /// </summary>
    /// <param name="status">Optional status object.</param>
    /// <param name="message">Optional message string.</param>
    /// <returns>A new Result object with a success status.</returns>
    public static Result CreateSuccess(in object? status = null, in string? message = null)
        => new(true, status, message);

    public static explicit operator Result(bool b)
        => b ? Success : Failure;

    /// <summary>
    /// Creates a new empty Result object.
    /// </summary>
    public static Result NewEmpty()
        => new();

    public static Result operator +(Result left, Result right)
    {
        var total = Combine(left, right);
        var result = new Result(total.Succeed, total.Status, total.Message, total.Errors, total.ExtraData);
        return result;
    }

    public bool Equals(Result? other)
        => throw new NotImplementedException();

    public override bool Equals(object? obj)
        => this.Equals(obj as Result);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>The hash code for the current instance.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public override int GetHashCode()
        => throw new NotImplementedException();
}

public class Result<TValue>(in TValue value,
    in bool? succeed = null,
    in object? status = null,
    in string? message = null,
    in IEnumerable<(object Id, object Error)>? errors = null,
    in ImmutableDictionary<string, object>? extraData = null)
    : ResultBase(succeed, status, message, errors, extraData),
    IAdditionOperators<Result<TValue>, ResultBase, Result<TValue>>,
    IEquatable<Result<TValue>>
{
    private static Result<TValue?>? _failure;

    /// <summary>
    /// Gets a Result object representing a failed operation.
    /// </summary>
    /// <returns>A Result object representing a failed operation.</returns>
    public static Result<TValue?> Failure => _failure ??= CreateFailure();

    public TValue Value { get; } = value;

    /// <summary> Combines multiple Result<TValue> objects into a single Result<TValue> object.
    /// </summary> <param name="results">The Result<TValue> objects to combine.</param> <returns>A
    /// single Result<TValue> object containing the combined results.</returns>
    public static Result<TValue> Combine(params Result<TValue>[] results)
    {
        var data = ResultBase.Combine(results);
        var result = new Result<TValue>(results.Last().Value, data.Succeed, data.Status, data.Message, data.Errors, data.ExtraData);
        return result;
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
        in ImmutableDictionary<string, object>? extraData = null)
        => new(value, false, status, message, errors, extraData);

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
        in (object Id, object Error) error)
        => new(value, false, status, message, EnumerableHelper.ToEnumerable(error), null);

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
    public static Result<TValue?> CreateFailure(in Exception error, in TValue? value = default)
        => CreateFailure(value, error, null);

    public static Result<TValue?> CreateFailure(in string message, in TValue value)
        => CreateFailure(value, null, message);

    /// <summary>
    /// Creates a new successful Result with the given value, status, message, errors and extra data.
    /// </summary>
    /// <param name="value">The value of the Result.</param>
    /// <param name="status">The status of the Result.</param>
    /// <param name="message">The message of the Result.</param>
    /// <param name="errors">The errors of the Result.</param>
    /// <param name="extraData">The extra data of the Result.</param>
    /// <returns>A new successful Result.</returns>
    public static Result<TValue> CreateSuccess(in TValue value,
        in object? status = null,
        in string? message = null,
        in IEnumerable<(object Id, object Error)>? errors = null,
        in ImmutableDictionary<string, object>? extraData = null)
        => new(value, true, status, message, errors, extraData);

    public static Result<TValue> From(in Result result, in TValue value)
        => new(value, result.Succeed, result.Status, result.Message, result.Errors, result.ExtraData);

    public static implicit operator Result(Result<TValue> result)
        => new(result.Succeed, result.Status, result.Message, result.Errors, result.ExtraData);

    public static implicit operator TValue(Result<TValue> result)
        => result.Value;

    /// <summary>
    /// Creates a new Result object with the given value, succeed, status, message, errors, and extraData.
    /// </summary>
    public static Result<TValue> New(in TValue value,
        in bool? succeed = null,
        in object? status = null,
        in string? message = null,
        in IEnumerable<(object Id, object Error)>? errors = null,
        in ImmutableDictionary<string, object>? extraData = null)
        => new(value, succeed, status, message, errors, extraData);

    public static Result<TValue> operator +(Result<TValue> left, ResultBase right)
    {
        var total = Combine(left, right);
        var result = new Result<TValue>(left.Value, total.Succeed, total.Status, total.Message, total.Errors, total.ExtraData);
        return result;
    }

    public void Deconstruct(out bool isSucceed, out TValue Value)
        => (isSucceed, Value) = (this.IsSucceed, this.Value);

    public bool Equals(Result<TValue>? other)
        => throw new NotImplementedException();

    public override bool Equals(object? obj)
        => this.Equals(obj as Result<TValue>);

    public override int GetHashCode()
        => throw new NotImplementedException();

    /// <summary>
    /// Gets the value of the current instance.
    /// </summary>
    /// <returns>The value of the current instance.</returns>
    public TValue GetValue()
        => this.Value;

    /// <summary>
    /// Converts the current Result object to an asynchronous Task.
    /// </summary>
    public Task<Result<TValue>> ToAsync()
        => Task.FromResult(this);

    /// <summary> Converts a Result<TValue> to a Result. </summary>
    public Result ToResult(in Result<TValue> result)
        => result;

    /// <summary>
    /// Creates a new instance of the Result class with the specified errors.
    /// </summary>
    /// <param name="errors">The errors to add to the Result.</param>
    /// <returns>A new instance of the Result class with the specified errors.</returns>
    public Result<TValue> WithError(params (object Id, object Error)[] errors)
        => this.With(() => this.Value, getErrors: () => errors);

    /// <summary>
    /// Creates a new instance of the <see cref="Result{TValue}"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value to set.</param>
    /// <returns>
    /// A new instance of the <see cref="Result{TValue}"/> class with the specified value.
    /// </returns>
    public Result<TValue> WithValue(in TValue value)
        => this.With(() => this.Value);
}

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract class ResultBase(in bool? succeed = null,
    in object? status = null,
    in string? message = null,
    in IEnumerable<(object Id, object Error)>? errors = null,
    in ImmutableDictionary<string, object>? extraData = null)
    : ICloneable
{
    protected ResultBase(ResultBase original)
        : this(original.Succeed, original.Status, original.Message, original.Errors, original.ExtraData)
    {
    }

    public IEnumerable<(object Id, object Error)>? Errors { get; init; } = errors;
    public ImmutableDictionary<string, object>? ExtraData { get; init; } = extraData;

    /// <summary>
    /// Gets a value indicating whether the operation has failed.
    /// </summary>
    public virtual bool IsFailure => !this.IsSucceed;

    /// <summary>
    /// Checks if the operation was successful by checking the Succeed flag, Status and Errors.
    /// </summary>
    public virtual bool IsSucceed => this.Succeed ?? this.Status is null or 0 or 200 && (!this.Errors?.Any() ?? true);

    public string? Message { get; init; } = message;

    public object? Status { get; } = status;

    public bool? Succeed { get; init; } = succeed;

    protected virtual Type EqualityContract => typeof(ResultBase);

    public static implicit operator bool(ResultBase result)
        => result.NotNull().IsSucceed;

    public virtual object Clone() => throw new NotImplementedException();

    public void Deconstruct(out bool isSucceed, out string message)
        => (isSucceed, message) = (this.IsSucceed, this.Message?.ToString() ?? string.Empty);

    public void Deconstruct(out bool? Succeed, out object? Status, out string? Message, out IEnumerable<(object Id, object Error)>? Errors, out ImmutableDictionary<string, object>? ExtraData)
    {
        Succeed = this.Succeed;
        Status = this.Status;
        Message = this.Message;
        Errors = this.Errors;
        ExtraData = this.ExtraData;
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared.</returns>
    public virtual bool Equals(ResultBase? other)
        => other is not null && this.Status == other.Status;

    public override bool Equals(object? obj)
        => this.Equals(obj as ResultBase);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>The hash code for the current instance.</returns>
    public override int GetHashCode()
        => HashCode.Combine(this.Status, this.Message);

    /// <summary>
    /// Generates a string representation of the result object.
    /// </summary>
    /// <returns>A string representation of the result object.</returns>
    public override string ToString()
    {
        var result = new StringBuilder().AppendLine($"IsSucceed: {this.IsSucceed}");
        if (!this.Message.IsNullOrEmpty())
        {
            _ = result.AppendLine(this.Message);
        }
        else if (this.Errors?.Count() == 1)
        {
            _ = result.AppendLine(this.Errors!.First().Error?.ToString() ?? "An error occurred.");
        }
        else if (this.Errors?.Any() ?? false)
        {
            foreach (var errorMessage in this.Errors.Select(x => x.Error?.ToString()).Compact())
            {
                _ = result.AppendLine($"- {errorMessage}");
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Combines multiple ResultBase objects into a single ResultBase object.
    /// </summary>
    /// <param name="results">The ResultBase objects to combine.</param>
    /// <returns>A ResultBase object containing the combined results.</returns>
    protected static (bool? Succeed, object? Status, string? Message, IEnumerable<(object Id, object Error)>? Errors, ImmutableDictionary<string, object>? ExtraData) Combine(params ResultBase[] results)
    {
        bool? isSucceed = results.All(x => x.Succeed == null) ? null : results.All(x => x.IsSucceed);
        var status = results.LastOrDefault(x => x.Status is not null)?.Status;
        var message = results.LastOrDefault(x => !x.Message.IsNullOrEmpty())?.Message;
        var errors = results.SelectMany(x => EnumerableHelper.DefaultIfEmpty(x?.Errors));
        var extraData = results.SelectMany(x => EnumerableHelper.DefaultIfEmpty(x.ExtraData)).ToImmutableDictionary();

        var statusBuffer = results.Where(x => x.Status is not null).Select(x => x.Status).ToList();
        if (statusBuffer.Count > 1)
        {
            errors = errors.AddRangeImmuted(statusBuffer.Select(x => ((object)null!, x!)));
            status = null;
        }

        return (isSucceed, status, message, errors, extraData);
    }

    protected virtual bool PrintMembers(StringBuilder builder)
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        var props = ObjectHelper.GetProperties(this);
        var isFirst = true;
        foreach (var (name, value) in props)
        {
            if (isFirst)
            {
                isFirst = false;
            }
            else
            {
                _ = builder.Append(", ");
            }

            _ = builder.Append($"{name} = ${value}");
        }
        return true;
    }

    private string GetDebuggerDisplay()
        => this.ToString();
}

public static class ResultExtensions
{
    public static Result With(this ResultBase source
        , Func<bool>? getSucceed = null
        , Func<object?>? getStatus = null
        , Func<string?>? getMessage = null
        , Func<IEnumerable<(object Id, object Error)>?>? getErrors = null
        , Func<ImmutableDictionary<string, object>?>? getExtraData = null
        ) => new(
           getSucceed != null ? getSucceed() : source.Succeed,
           getStatus != null ? getStatus() : source.Status,
           getMessage != null ? getMessage() : source.Message,
           getErrors != null ? getErrors() : source.Errors,
           getExtraData != null ? getExtraData() : source.ExtraData);

    public static Result<TValue> With<TValue>(this Result<TValue> source
        , Func<TValue>? getValue = null
        , Func<bool>? getSucceed = null
        , Func<object?>? getStatus = null
        , Func<string?>? getMessage = null
        , Func<IEnumerable<(object Id, object Error)>?>? getErrors = null
        , Func<ImmutableDictionary<string, object>?>? getExtraData = null
        ) => new(
           getValue != null ? getValue() : source.Value,
           getSucceed != null ? getSucceed() : source.Succeed,
           getStatus != null ? getStatus() : source.Status,
           getMessage != null ? getMessage() : source.Message,
           getErrors != null ? getErrors() : source.Errors,
           getExtraData != null ? getExtraData() : source.ExtraData);
}
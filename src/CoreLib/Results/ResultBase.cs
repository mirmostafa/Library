using System.Collections.Immutable;
using System.Diagnostics;

using Library.Interfaces;
using Library.Validations;

namespace Library.Results;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract record ResultBase(in bool? Succeed = null,
                                  in object? Status = null,
                                  in string? Message = null,
                                  in IEnumerable<(object Id, object Error)>? Errors = null,
                                  in ImmutableDictionary<string, object>? ExtraData = null)
{
    public virtual bool IsSucceed => this.Succeed ?? ((this.Status is null or 0 or 200) && (!this.Errors?.Any() ?? true));
    public virtual bool IsFailure => !this.IsSucceed;
    public void Deconstruct(out bool isSucceed, out string message)
        => (isSucceed, message) = (this.IsSucceed, this.Message?.ToString() ?? string.Empty);

    public virtual bool Equals(ResultBase? other)
        => other is not null && this.Status == other.Status;

    public override int GetHashCode()
        => HashCode.Combine(this.Status, this.Message);

    public static implicit operator bool(ResultBase result)
        => result.NotNull().IsSucceed;

    public override string ToString()
    {
        var result = new StringBuilder();
        _ = result.AppendLine($"IsSucceed: {this.IsSucceed}");
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

    private string GetDebuggerDisplay()
        => this.ToString();

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
}

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
    public static Result Empty => _empty ??= NewEmpty();

    public static Result Failure => _fail ??= CreateFailure();

    public static Result Success => _success ??= CreateSuccess();

    /// <summary>
    /// Creates a new Result object with a failure status.
    /// </summary>
    /// <param name="status">Optional status object.</param>
    /// <param name="message">Optional message string.</param>
    /// <param name="errors">Optional enumerable of (Id, Error) tuples.</param>
    /// <param name="extraData">Optional extra data dictionary.</param>
    /// <returns>A new Result object with a failure status.</returns>
    public static Result CreateFailure(in object? status = null, in string? message = null, in IEnumerable<(object Id, object Error)>? errors = null, in ImmutableDictionary<string, object>? extraData = null)
        => new(false, status, message, Errors: errors, extraData);
    public static Result CreateFailure(in string? message, in IEnumerable<(object Id, object Error)>? errors)
        => new(false, null, message, Errors: errors);

    public static Result CreateSuccess(in object? status = null, in string? message = null)
        => new(true, status, message);

    public static Result NewEmpty()
        => new();

    public static explicit operator Result(bool b)
        => b ? Success : Failure;

    public static Result operator +(Result left, Result right)
    {
        var total = Combine(left, right);
        var result = new Result(total.Succeed, total.Status, total.Message, total.Errors, total.ExtraData);
        return result;
    }

    public static Result CreateFailure(in string message, in Exception error)
        => CreateFailure(error, message);

    public static Result CreateFailure(Exception error)
        => CreateFailure(error, null);

    public static Result Combine(params Result[] results)
    {
        var data = ResultBase.Combine(results);
        var result = new Result(data.Succeed, data.Status, data.Message, data.Errors, data.ExtraData);
        return result;
    }
}

public record Result<TValue>(in TValue Value,
    in bool? Succeed = null,
    in object? Status = null,
    in string? Message = null,
    in IEnumerable<(object Id, object Error)>? Errors = null,
    in ImmutableDictionary<string, object>? ExtraData = null)
    : ResultBase(Succeed, Status, Message, Errors, ExtraData)
    , IAdditionOperators<Result<TValue>, ResultBase, Result<TValue>>
    , IEquatable<Result<TValue>>
{
    private static Result<TValue?>? _failure;
    public static Result<TValue?> Failure => _failure ??= CreateFailure();

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

    public static Result<TValue?> CreateFailure(in TValue? value,
        in object? status,
        in string? message,
        in (object Id, object Error) error)
        => new(value, false, status, message, EnumerableHelper.ToEnumerable(error), null);

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

    public static implicit operator Result(Result<TValue> result)
        => new(result.Succeed, result.Status, result.Message, result.Errors, result.ExtraData);

    public static Result<TValue> operator +(Result<TValue> left, ResultBase right)
    {
        var total = Combine(left, right);
        var result = new Result<TValue>(left.Value, total.Succeed, total.Status, total.Message, total.Errors, total.ExtraData);
        return result;
    }

    public static Result<TValue> Combine(params Result<TValue>[] results)
    {
        var data = ResultBase.Combine(results);
        var result = new Result<TValue>(results.Last().Value, data.Succeed, data.Status, data.Message, data.Errors, data.ExtraData);
        return result;
    }

    public static implicit operator TValue(Result<TValue> result)
        => result.Value;
    public void Deconstruct(out bool isSucceed, out TValue Value)
        => (isSucceed, Value) = (this.IsSucceed, this.Value);
    public Result ToResult(in Result<TValue> result)
        => result;
    public static Result<TValue> From(in Result result, in TValue value)
        => new(value, result.Succeed, result.Status, result.Message, result.Errors, result.ExtraData);

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
    /// Creates a new instance of the <see cref="Result{TValue}"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value to set.</param>
    /// <returns>A new instance of the <see cref="Result{TValue}"/> class with the specified value.</returns>
    public Result<TValue> WithValue(in TValue value)
        => this with { Value = value };

    /// <summary>
    /// Creates a new instance of the Result class with the specified errors.
    /// </summary>
    /// <param name="errors">The errors to add to the Result.</param>
    /// <returns>A new instance of the Result class with the specified errors.</returns>
    public Result<TValue> WithError(params (object Id, object Error)[] errors)
        => this with { Errors = errors };

    public Task<Result<TValue>> ToAsync()
        => Task.FromResult(this);
    public TValue GetValue()
        => this.Value;
}
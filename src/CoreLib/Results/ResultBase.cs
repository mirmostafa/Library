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
        var result = (this.IsSucceed ? new StringBuilder($"IsSucceed: {this.IsSucceed}") : new StringBuilder()).AppendLine();
        if (!this.Message.IsNullOrEmpty())
        {
            _ = result.AppendLine(this.Message);
        }
        if (this.Message.IsNullOrEmpty() && this.Errors?.Count() == 1)
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
        => (results.All(x => x.Succeed == null) ? null : results.All(x => x.IsSucceed),
            results.LastOrDefault(x => x.Status is not null)?.Status,
            results.LastOrDefault(x => !x.Message.IsNullOrEmpty())?.Message,
            results.SelectMany(x => EnumerableHelper.DefaultIfEmpty(x?.Errors)),
            results.SelectMany(x => EnumerableHelper.DefaultIfEmpty(x.ExtraData)).ToImmutableDictionary());
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

    public static Result Fail => _fail ??= CreateFail();

    public static Result Success => _success ??= CreateSuccess();

    public static Result CreateFail(in object? status = null, in string? message = null, in IEnumerable<(object Id, object Error)>? errors = null, in ImmutableDictionary<string, object>? extraData = null)
        => new(false, status, message, Errors: errors, extraData);
    public static Result CreateFail(in string? message, in IEnumerable<(object Id, object Error)>? errors)
        => new(false, null, message, Errors: errors);

    public static Result CreateSuccess(in object? status = null, in string? message = null)
        => new(true, status, message);

    public static Result NewEmpty()
        => new();

    public static explicit operator Result(bool b)
        => b ? Success : Fail;

    public static Result operator +(Result left, Result right)
    {
        var total = Combine(left, right);
        var result = new Result(total.Succeed, total.Status, total.Message, total.Errors, total.ExtraData);
        return result;
    }

    public static Result CreateFail(in string message, in Exception error)
        => CreateFail(null, message, EnumerableHelper.ToEnumerable(((object)0, (object)error)));

    public static Result CreateFail(Exception error)
        => CreateFail(null, null, EnumerableHelper.ToEnumerable(((object)0, (object)error)));

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
    private static Result<TValue?>? _fail;
    public static Result<TValue?> Fail => _fail ??= CreateFail();

    public static Result<TValue> New(in TValue value,
        in bool? succeed = null,
        in object? status = null,
        in string? message = null,
        in IEnumerable<(object Id, object Error)>? errors = null,
        in ImmutableDictionary<string, object>? extraData = null)
        => new(value, succeed, status, message, errors, extraData);
    public static Result<TValue?> CreateFail(in TValue? value = default,
        in object? status = null,
        in string? message = null,
        in IEnumerable<(object Id, object Error)>? errors = null,
        in ImmutableDictionary<string, object>? extraData = null)
        => new(value, false, status, message, errors, extraData);

    public static Result<TValue?> CreateFail(in TValue? value,
        in object? status,
        in string? message,
        in (object Id, object Error) error)
        => new(value, false, status, message, EnumerableHelper.ToEnumerable(error), null);

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

    public static Result<TValue?> CreateFail(in string message, in Exception ex, in TValue? value)
        => CreateFail(value, null, message, EnumerableHelper.ToEnumerable(((object)0, (object)ex)));

    public static Result<TValue?> CreateFail(in Exception error, in TValue? value = default)
        => CreateFail(value, null, null, EnumerableHelper.ToEnumerable(((object)0, (object)error)));

    public static Result<TValue?> CreateFail(in string message, in TValue value)
        => CreateFail(value, null, message);

    public Result<TValue> WithValue(in TValue value)
        => this with { Value = value };

    public Result<TValue> WithError(params (object Id, object Error)[] errors)
        => this with { Errors = errors };

    public Task<Result<TValue>> ToAsync()
        => Task.FromResult(this);
}
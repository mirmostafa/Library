//using System.Diagnostics;

//using Library.Exceptions;
//using Library.Validations;
//using Library.Windows;

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

    protected static void Concat(in ResultBase item1,
                                 in ResultBase items2,
                                 out bool success,
                                 out object? status,
                                 out string? message,
                                 out IEnumerable<(object Id, object Error)> errors,
                                 out ImmutableDictionary<string, object> extra)
    {
        success = item1.IsSucceed && items2.IsSucceed;
        status = item1.Status ?? items2.Status;
        message = item1.Message ?? items2.Message;
        errors = item1.Errors.AddRangeImmuted(items2.Errors);
        extra = item1.ExtraData.AddRangeImmuted(items2.ExtraData).ToImmutableDictionary();
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
        Concat(left, right, out var success, out var status, out var message, out var errors, out var extra);
        var result = new Result(success, status, message, errors, extra);
        return result;
    }

    public static Result CreateFail(in string message, in Exception error)
        => CreateFail(null, message, EnumerableHelper.ToEnumerable(((object)0, (object)error)));

    public static Result CreateFail(Exception error)
        => CreateFail(null, null, EnumerableHelper.ToEnumerable(((object)0, (object)error)));
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
        Concat(left, right, out var success, out var status, out var message, out var errors, out var extra);
        var result = new Result<TValue>(left.Value, success, status, message, errors, extra);
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
}

//[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
//public abstract record ResultBase : IEquatable<ResultBase?>
//{
//    private NotificationMessage? _fullMessage;
//    private bool? _isSucceed;

//    protected ResultBase(object? status = null, string? message = null)
//        => (this.Status, this.FullMessage) = (status, message);

//    protected ResultBase(object? status, NotificationMessage? fullMessage)
//        => (this.Status, this.FullMessage) = (status, fullMessage);

//    protected ResultBase(object? status, [DisallowNull] IException exception)
//        => (this.Status, this.FullMessage) = (status, exception.NotNull().ToFullMessage());

//    public List<(object? Id, object Data)> Errors { get; } = new();

//    public Dictionary<string, object> Extra { get; } = new();

//    public NotificationMessage? FullMessage { get => this._fullMessage; init => this._fullMessage = value; }

//    public bool IsFailure => !this.IsSucceed;

//    public virtual bool IsSucceed
//    {
//        get => this._isSucceed ?? ((this.Status is null or 0 or 200) && (!this.Errors.Any()));
//        init => this._isSucceed = value;
//    }

//    public string? Message => this.FullMessage?.Text;

//    public object? Status
//    {
//        get;
//        protected set;
//    }

//    //public static bool operator !=(ResultBase? left, ResultBase? right)
//    //    => !(left == right);

//    //public static bool operator ==(ResultBase? left, ResultBase? right)
//    //    => EqualityComparer<ResultBase>.Default.Equals(left, right);

//    public void Deconstruct(out bool isSucceed, out string? message)
//        => (isSucceed, message) = (this.IsSucceed, this.Message);

//    //public override bool Equals(object? obj)
//    //    => this.Equals(obj as ResultBase);

//    public virtual bool Equals(ResultBase? other)
//        => other is not null && this.Status == other.Status;

//    public override int GetHashCode()
//        => HashCode.Combine(this.Status, this.Message, this.Errors);

//    public override string ToString()
//    {
//        var result = (this.IsSucceed ? new StringBuilder($"IsSucceed: {this.IsSucceed}") : new StringBuilder()).AppendLine();
//        if (!this.Message.IsNullOrEmpty())
//        {
//            _ = result.AppendLine(this.Message);
//        }
//        if (this.Message.IsNullOrEmpty() && this.Errors.Count == 1)
//        {
//            _ = result.AppendLine(this.Errors[0].Data?.ToString() ?? "An error occurred.");
//        }
//        else
//        {
//            foreach (var errorMessage in this.Errors.Select(x => x.Data?.ToString()).Compact())
//            {
//                _ = result.AppendLine($"- {errorMessage}");
//            }
//        }

//        return result.ToString();
//    }

//    internal static TResult Add<TResult>(in ResultBase item1, in ResultBase item2, TResult result)
//        where TResult : ResultBase, new()
//    {
//        if (!item1.IsSucceed || !item2.IsSucceed)
//        {
//            result.SetIsSucceed(false);
//        }

//        result.Errors.AddRange(item2.Errors.AddRangeImmuted(item1.Errors));
//        _ = item2.Extra.AddRange(item1.Extra);
//        return result;
//    }

//    internal void SetIsSucceed(bool? isSucceed)
//        => this._isSucceed = isSucceed;

//    private string GetDebuggerDisplay()
//        => this.ToString();
//}
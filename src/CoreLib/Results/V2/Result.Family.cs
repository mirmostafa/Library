using System.Diagnostics;

using Library.Validations;

namespace Library.Results.V2;

public interface IResult
{
    IEnumerable<Exception> Exceptions { get; }
    bool IsFailure { get; }
    bool IsSucceed { get; }
    string? Message { get; }
}
public interface IResult<TValue> : IResult
{
    TValue Value { get; }
}

[DebuggerStepThrough]
[StackTraceHidden]
public sealed class Result<TValue> : ResultBase, IResult<TValue>
{
    private static Result<TValue?>? _failed;
    private static Result<TValue?>? _succeed;

    public Result(TValue value, bool isSucceed, string? message = null, params Exception[] exceptions) : base(isSucceed, message, exceptions)
        => this.Value = value;

    public Result(TValue value, bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null) : base(isSucceed, message, exceptions)
        => this.Value = value;

    public static Result<TValue?> Failed => _failed ??= new(default, false);
    public static Result<TValue?> Succeed => _succeed ??= new(default, true);
    public TValue Value { get; }

    public static implicit operator (Result Result, TValue? Value)(Result<TValue?> result)
        => result == null ? (default!, default) : (result, result.Value);

    public static implicit operator Result(Result<TValue?> result)
        => result == null ? null! : new(result.IsSucceed, result.Message, result.Exceptions);

    public static implicit operator TValue?(Result<TValue?> result)
        => result == null ? default : result.Value;
}

[DebuggerStepThrough]
[StackTraceHidden]
public sealed class Result : ResultBase, IResult
{
    private static Result? _failed;

    private static Result? _succeed;

    internal Result(bool isSucceed, string? message = null, params Exception[] exceptions) : base(isSucceed, message, exceptions)
    {
    }

    internal Result(bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null) : base(isSucceed, message, exceptions)
    {
    }

    [NotNull]
    public static Result Failed => _failed ??= new(false);

    [NotNull]
    public static Result Succeed => _succeed ??= new(true);

    [return: NotNull]
    public static Result Create(bool isSucceed)
        => new(isSucceed);

    [return: NotNull]
    public static Result Create(bool isSucceed, string? message)
        => new(isSucceed, message);

    [return: NotNull]
    public static Result Create(Exception exception)
        => new(false, exceptions: exception);

    [return: NotNull]
    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed)
        => new(value, isSucceed);

    [return: NotNull]
    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed, string? message)
        => new(value, isSucceed, message);

    [return: NotNull]
    public static Result<TValue> Fail<TValue>(TValue value)
        => new(value, false);

    [return: NotNull]
    public static Result Fail(Exception exception)
        => new(false, exceptions: exception);

    [return: NotNull]
    public static Result<TValue> Fail<TValue>(TValue value, string message)
        => new(value, false, message);

    [return: NotNull]
    public static Result<TValue> Success<TValue>(TValue value)
        => new(value, true);
}

[DebuggerStepThrough]
[StackTraceHidden]
public abstract class ResultBase(bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null)
{
    public IEnumerable<Exception> Exceptions { get; } = exceptions?.Count() > 0 ? exceptions : [];

    public bool IsFailure => !this.IsSucceed;

    public bool IsSucceed { get; } = isSucceed;

    public string? Message { get; } = message;
}

[DebuggerStepThrough]
[StackTraceHidden]
public static class ResultHelper
{
    public static void Deconstruct<TValue>(this IResult<TValue?> r, out IResult result, out TValue? value)
        => (result, value) = r == null ? (default!, default) : (r, r.Value);

    public static async Task<(IResult Result, TValue? Value)> Deconstruct<TValue>(this Task<Result<TValue?>> r)
    {
        var result = await r;
        return (result, result.Value);
    }

    public static async Task<(IResult Result, TValue? Value)> Deconstruct<TValue>(this Task<IResult<TValue?>> r)
    {
        Check.MustBeArgumentNotNull(r);
        var result = await r;
        return (result, result.Value);
    }

    public static async Task<TValue> GetValueAsync<TValue>(this Task<IResult<TValue>> result)
        => (await result.ArgumentNotNull()).Value;

    [return: NotNullIfNotNull(nameof(result))]
    public static TResult? OnFailure<TResult>(this TResult? result, Action<TResult> action)
        where TResult : IResult
    {
        if (result?.IsSucceed != true)
        {
            action.ArgumentNotNull()(result!);
        }

        return result!;
    }

    public static async Task<TFuncResult?> OnFailure<TResult, TFuncResult>(this Task<TResult?> resultAsync, Func<TResult, TFuncResult> action, TFuncResult defaultFuncResult = default!)
        where TResult : IResult
    {
        Check.MustBeArgumentNotNull(resultAsync);

        var result = await resultAsync;
        return result?.IsSucceed != true
            ? action.ArgumentNotNull()(result!)
            : defaultFuncResult;
    }

    [return: NotNullIfNotNull(nameof(result))]
    public static TResult OnSucceed<TResult>(this TResult result, Action<TResult> action)
        where TResult : IResult
    {
        if (result?.IsSucceed == true)
        {
            action.ArgumentNotNull()(result);
        }

        return result!;
    }

    public static async Task<TResult> OnSucceed<TResult>(this Task<TResult> resultAsync, Action<TResult> action)
        where TResult : IResult
    {
        Check.MustBeArgumentNotNull(resultAsync);

        var result = await resultAsync;
        if (result?.IsSucceed == true)
        {
            action.ArgumentNotNull()(result);
        }

        return result!;
    }

    public static TFuncResult OnSucceed<TResult, TFuncResult>(this TResult result, Func<TResult, TFuncResult> action, TFuncResult defaultFuncResult = default!)
        where TResult : IResult => result?.IsSucceed == true
            ? action.ArgumentNotNull()(result)
            : defaultFuncResult;

    public static async Task<TFuncResult> OnSucceedAsync<TResult, TFuncResult>(this Task<TResult> resultAsync, Func<TResult, TFuncResult> action, TFuncResult defaultFuncResult = default!)
        where TResult : IResult
    {
        Check.MustBeArgumentNotNull(resultAsync);

        var result = await resultAsync;
        return result?.IsSucceed == true
            ? action.ArgumentNotNull()(result)
            : defaultFuncResult;
    }

    public static async Task<TFuncResult> Process<TResult, TFuncResult>(this Task<TResult?> resultAsync, Func<TResult, TFuncResult> onSucceed, Func<TResult?, TFuncResult> onFailure)
        where TResult : IResult
    {
        var result = await resultAsync;
        return result?.IsSucceed == true
            ? onSucceed.ArgumentNotNull()(result)
            : onFailure.ArgumentNotNull()(result);
    }

    public static TFuncResult Process<TResult, TFuncResult>(this TResult? result, Func<TResult, TFuncResult> onSucceed, Func<TResult?, TFuncResult> onFailure)
        where TResult : IResult
        => result?.IsSucceed == true
            ? onSucceed.ArgumentNotNull()(result)
            : onFailure.ArgumentNotNull()(result);

    public static Task<IResult> ToAsync(this IResult result)
        => Task.FromResult(result);

    public static Task<IResult<TValue>> ToAsync<TValue>(this IResult<TValue> result)
        => Task.FromResult(result);

    public static IResult<TValue> WithValue<TValue>(this IResult result, TValue value)
        => result == null ? null! : new Result<TValue>(value, result.IsSucceed, result.Message, result.Exceptions)!;
}
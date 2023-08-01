using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

using Library.Exceptions;
using Library.Interfaces;
using Library.Logging;
using Library.Results;

namespace Library.Helpers;

[DebuggerStepThrough]
[StackTraceHidden]
public static class ResultHelper
{
    /// <summary>
    /// Asynchronously awaits a task of type TResult and returns the result, breaking on failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="task">The task to await.</param>
    /// <returns>The result of the task, breaking on failure.</returns>
    public static async Task<TResult> BreakOnFail<TResult>(this Task<TResult> task)
        where TResult : ResultBase
    {
        var result = await task;
        return result.BreakOnFail();
    }

    public static TResult BreakOnFail<TResult>(this TResult result)
        where TResult : ResultBase
    {
        if (!result)
        {
            CodeHelper.Break();
        }
        return result;
    }

    public static Result<TValue> Combine<TValue>(this IEnumerable<Result<TValue>> results)
        where TValue : IAdditionOperators<TValue, TValue, TValue> =>
        Result<TValue>.Combine((x, y) => x + y, results.ToArray());

    public static void Deconstruct(this Result result, out bool isSucceed, out string message) =>
        (isSucceed, message) = (result.IsSucceed, result.Message?.ToString() ?? string.Empty);

    public static void Deconstruct<TValue>(this Result<TValue> result, out bool IsSucceed, out TValue Value) =>
        (IsSucceed, Value) = (result.IsSucceed, result.Value);

    public static TResult LogDebug<TResult>(this TResult result, ILogger logger, [CallerMemberName] object? sender = null, DateTime? time = null)
                    where TResult : ResultBase
    {
        if (result.IsSucceed)
        {
            logger.Debug(result, sender, time);
        }
        else
        {
            logger.Error(result, sender, time);
        }

        return result;
    }

    public static Result<TValue> Merge<TValue>(this IEnumerable<Result<TValue>> results, Func<TValue, TValue, TValue> add) =>
        Result<TValue>.Combine(results, add);

    public static Result<TValue> Merge<TValue>(this IEnumerable<Result<TValue>> results) where TValue : IAdditionOperators<TValue, TValue, TValue> =>
        Result<TValue>.Combine(results, (x1, x2) => x1 + x2);

    public static TResult OnDone<TResult>([DisallowNull] this TResult result, [DisallowNull] Action<TResult> action) where TResult : ResultBase
    {
        action(result);
        return result;
    }

    public static TResult OnFailure<TResult>([DisallowNull] this TResult result, [DisallowNull] Action<TResult> action) where TResult : ResultBase
    {
        if (result == false)
        {
            action(result);
        }

        return result;
    }

    public static TResult OnSucceed<TResult>([DisallowNull] this TResult result, [DisallowNull] Func<TResult> next) where TResult : ResultBase
        => result == true ? next() : result;

    public static TResult OnSucceed<TResult>([DisallowNull] this TResult result, [DisallowNull] Action<TResult> action) where TResult : ResultBase
    {
        if (result == true)
        {
            action(result);
        }

        return result;
    }

    public static Result<Stream> SerializeToXmlFile<T>(this Result<Stream> result, string filePath)
    {
        Validations.Check.MustBeArgumentNotNull(filePath);
        return result.Fluent(() => new XmlSerializer(typeof(T)).Serialize(result.Value, filePath));
    }

    /// <summary>
    /// Throws an exception if the given Result is not successful.
    /// </summary>
    /// <param name="result">The Result to check.</param>
    /// <param name="owner">The object that is throwing the exception.</param>
    /// <param name="instruction">The instruction that is throwing the exception.</param>
    /// <returns>The given Result.</returns>
    public static Result ThrowOnFail([DisallowNull] this Result result, object? owner = null, string? instruction = null)
        => InnerThrowOnFail(result, owner, instruction);

    /// <summary>
    /// Throws an exception if the given result is not successful.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="result">The result to check.</param>
    /// <param name="owner">The owner of the result.</param>
    /// <param name="instruction">The instruction associated with the result.</param>
    /// <returns>The given result.</returns>
    public static TResult ThrowOnFail<TResult>([DisallowNull] this TResult result, object? owner = null, string? instruction = null) where TResult : ResultBase
        => InnerThrowOnFail(result, owner, instruction);

    /// <summary>
    /// Throws an exception if the given <see cref="ResultTValue"/> is a failure.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="result">The <see cref="ResultTValue"/> to check.</param>
    /// <param name="owner">The object that is responsible for the operation.</param>
    /// <param name="instruction">The instruction that is responsible for the operation.</param>
    /// <returns>The given <see cref="ResultTValue"/>.</returns>
    public static Result<TValue> ThrowOnFail<TValue>([DisallowNull] this Result<TValue> result, object? owner = null, string? instruction = null)
        => InnerThrowOnFail(result, owner, instruction);

    /// <summary>
    /// Throws an exception if the result of the provided Task is a failure.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="resultAsync">The result to check.</param>
    /// <param name="owner">The owner of the result.</param>
    /// <param name="instruction">The instruction associated with the result.</param>
    /// <returns>The result of the provided Task.</returns>
    public static async Task<Result<TValue>> ThrowOnFailAsync<TValue>(this Task<Result<TValue>> resultAsync, object? owner = null, string? instruction = null)
        => InnerThrowOnFail(await resultAsync, owner, instruction);

    public static async Task<Result> ThrowOnFailAsync(this Task<Result> resultAsync, object? owner = null, string? instruction = null)
        => InnerThrowOnFail(await resultAsync, owner, instruction);

    public static Task<TResult> ToAsync<TResult>(this TResult result) where TResult : ResultBase
        => Task.FromResult(result);

    public static Result<Stream> ToFile(this Result<Stream> result, string filePath, FileMode fileMode = FileMode.Create)
    {
        Validations.Check.MustBeArgumentNotNull(filePath);
        var stream = result.Value;
        using var fileStream = new FileStream(filePath, fileMode, FileAccess.Write);
        stream.CopyTo(fileStream);

        return result;
    }

    public static async Task<Result<TValue1>> ToResultAsync<TValue, TValue1>(this Task<Result<TValue>> resultTask, Func<TValue, TValue1> getNewValue)
    {
        var result = await resultTask;
        var value1 = getNewValue(result);
        return Result<TValue1>.From(result, value1);
    }

    public static bool TryParse<TResult>([DisallowNull] this TResult input, [NotNull] out TResult result) where TResult : ResultBase
        => (result = input).IsSucceed;

    //! Compiler Error CS1988: Async methods cannot have `ref`, `in` or `out` parameters
    //x public static async Task<bool> TryAsync<TResult>([DisallowNull] this Task<TResult> input, out TResult result) where TResult : ResultBase
    //x     => (result = await input).IsSucceed;

    /// <summary>
    /// Creates a new instance of the Result class with the specified errors.
    /// </summary>
    /// <param name="errors">The errors to add to the Result.</param>
    /// <returns>A new instance of the Result class with the specified errors.</returns>
    public static Result WithError(this ResultBase result, params (object Id, object Error)[] errors) =>
        new(result) { Errors = errors };

    public static Result WithError(this ResultBase result, IEnumerable<(object Id, object Error)> errors) =>
        result.WithError(errors.ToArray());

    public static Result WithSucceed(this ResultBase result, bool? succeed) =>
        new(result) { Succeed = succeed };

    public static Result<TNewValue?> WithValue<TNewValue>(this ResultBase result, in TNewValue? newValue) =>
        new(result, newValue);

    /// <summary>
    /// Creates a new instance of the <see cref="Result{TValue}"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value to set.</param>
    /// <returns>
    /// A new instance of the <see cref="Result{TValue}"/> class with the specified value.
    /// </returns>
    public static Result<TValue> WithValue<TValue>(this Result<TValue> result, in TValue value)
        => new(result, value);

    private static TResult InnerThrowOnFail<TResult>([DisallowNull] TResult result, object? owner, string? instruction = null)
        where TResult : ResultBase
    {
        Validations.Check.MustBeArgumentNotNull(result);
        if (result.IsSucceed)
        {
            return result;
        }

        var exception =
            result.Errors?.Select(x => x.Error).Cast<Exception>().FirstOrDefault()
            ?? result.Status switch
            {
                Exception ex => ex.With(x => x.Source = owner?.ToString()),
                _ => new CommonException(result.ToString(), instruction ?? result.Message, owner: owner)
            };
        Throw(exception);
        return result;
    }
}
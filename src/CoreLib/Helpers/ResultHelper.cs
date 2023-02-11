﻿using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;

using Library.Exceptions.Validations;
using Library.Logging;
using Library.Results;
using Library.Validations;

namespace Library.Helpers;

public static class ResultHelper
{
    public static async Task<TResult> BreakOnFail<TResult>(this Task<TResult> task)
        where TResult : Result
    {
        var result = await task;
        return result.BreakOnFail();
    }

    public static TResult BreakOnFail<TResult>(this TResult result)
        where TResult : Result
    {
        if (!result)
        {
            CodeHelper.Break();
        }
        return result;
    }

    public static TResult Check<TResult>([DisallowNull] this TResult result, [DisallowNull] in Func<bool> predicate, in object? errorMessage, object? errorId = null) where TResult : ResultBase
        => InnerCheck(result, predicate(), errorMessage, errorId);

    public static TResult Check<TResult>([DisallowNull] this TResult result, bool condition, in object? errorMessage, object? errorId = null) where TResult : ResultBase
        => InnerCheck(result, condition, errorMessage, errorId);

    public static TResult Check<TResult>([DisallowNull] this TResult result, [DisallowNull] in Func<(bool Condition, object? ErrorMessage)> getErrorInfo, object? errorId = null)
        where TResult : ResultBase
    {
        var (condition, errorMessage) = getErrorInfo();
        return InnerCheck(result, condition, errorMessage, errorId);
    }

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

    public static Result<Stream> SerializeToXmlFile<T>(this Result<Stream> result, string filePath)
    {
        Validations.Check.IfArgumentNotNull(filePath);
        return result.Fluent(() => new XmlSerializer(typeof(T)).Serialize(result.Value, filePath));
    }

    public static Result ThrowOnFail([DisallowNull] this Result result, object? owner = null, string? instruction = null)
        => InnerThrowOnFail(result, owner, instruction);

    public static Result<TValue> ThrowOnFail<TValue>([DisallowNull] this Result<TValue> result, object? owner = null, string? instruction = null)
        => InnerThrowOnFail(result, owner, instruction);

    public static async Task<Result<TValue>> ThrowOnFailAsync<TValue>(this Task<Result<TValue>> resultAsync, object? owner = null, string? instruction = null)
        => InnerThrowOnFail(await resultAsync, owner, instruction);

    public static async Task<Result> ThrowOnFailAsync(this Task<Result> resultAsync, object? owner = null, string? instruction = null)
        => InnerThrowOnFail(await resultAsync, owner, instruction);

    public static Result<Stream> ToFile(this Result<Stream> result, string filePath, FileMode fileMode = FileMode.Create)
    {
        Validations.Check.IfArgumentNotNull(filePath);
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

    public static Result<StreamWriter> ToStreamWriter(this Result<Stream> result)
        => new(new(result.Value));

    public static Result<string> ToText(this Result<Stream> result)
    {
        var stream = result.Value;
        using var reader = new StreamReader(stream);
        return new(reader.ReadToEnd());
    }

    public static Result<XmlWriter> ToXmlWriter(this Result<Stream> result, bool indent = true)
        => new(XmlWriter.Create(result.ToStreamWriter(), new XmlWriterSettings { Indent = indent }));

    public static bool TryParse<TResult>([DisallowNull] this TResult input, [NotNull] out TResult result) where TResult : ResultBase
        => (result = input.ArgumentNotNull()).IsSucceed;

    //!? Compiler Error CS1988: Async methods cannot have `ref`, `in` or `out` parameters
    ////public static async Task<bool> TryAsync<TResult>([DisallowNull] this Task<TResult> input, out TResult result) where TResult : ResultBase
    ////    => (result = await input).IsSucceed;

    private static TResult InnerCheck<TResult>(TResult result, bool condition, object? errorMessage, object errorId)
            where TResult : ResultBase => condition
            ? (result with
            {
                Succeed = null,
                Errors = EnumerableHelper.ToEnumerable((errorId, errorMessage ?? string.Empty))
            })
            : result;

    private static TResult InnerThrowOnFail<TResult>([DisallowNull] TResult result, object? owner, string? instruction = null)
        where TResult : ResultBase
    {
        Validations.Check.ArgumentNotNull(result);
        if (result.IsSucceed)
        {
            return result;
        }

        var exception =
            result.Errors?.Select(x => x.Error).Cast<Exception>().FirstOrDefault()
            ?? result.Status switch
            {
                Exception ex => ex.With(x => x.Source = owner?.ToString()),
                _ => new ValidationException(result.ToString(), instruction ?? result.Message, owner: owner)
            };
        Throw(exception);
        return result;
    }

    public static Task<TResult> ToAsync<TResult>(this TResult result) where TResult : ResultBase 
        => Task.FromResult(result);
}
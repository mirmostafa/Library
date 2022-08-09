using Library.Exceptions.Validations;
using Library.Results;

namespace Library.Helpers;

public static class ResultHelper
{
    public static async Task<TResult> BreakOnFail<TResult>(this Task<TResult> task)
        where TResult : Result
    {
        var result = await task;
        if (!result)
        {
            CodeHelper.Break();
        }
        return result;
    }

    //public static TResult HasValue<TResult>(this TResult result, object? obj, in object? errorMessage, object? errorId = null)
    //    where TResult : ResultBase
    //    => MustBe(result, obj is not null, errorMessage, errorId ?? NullValueValidationException.ErrorCode);

    //public static TResult HasValue<TResult>(this TResult result, string? obj, in object errorMessage, object? errorId = null)
    //    where TResult : ResultBase
    //    => MustBe(result, !obj.IsNullOrEmpty(), errorMessage, errorId ?? NullValueValidationException.ErrorCode);

    public static bool IsValid<TValue>([NotNullWhen(true)] this Result<TValue> result)
        => result is not null and { IsSucceed: true } and { Value: not null };

    public static TResult MustBe<TResult>([DisallowNull] this TResult result, [DisallowNull] in Func<bool> predicate, in object? errorMessage, object? errorId = null)
        where TResult : ResultBase
        => result.MustBe(predicate(), errorMessage, errorId);

    public static TResult MustBe<TResult>([DisallowNull] this TResult result, bool condition, in object? errorMessage, object? errorId = null)
        where TResult : ResultBase
    {
        if (!condition)
        {
            result.Errors.Add((errorId, errorMessage ?? string.Empty));
            result.SetIsSucceed(null);
        }

        return result;
    }

    public static TResult MustBe<TResult>([DisallowNull] this TResult result, Func<(bool Condition, object? ErrorMessage)> getErrorInfo, object? errorId = null)
        where TResult : ResultBase
    {
        var (condition, errorMessage) = getErrorInfo();
        return result.MustBe(condition, errorMessage, errorId);
    }

    public static TResult Process<TResult>([DisallowNull] this TResult result)
        where TResult : ResultBase 
        => Process(result);

    public static Result<TValue> Process<TValue>([DisallowNull] this Result<TValue> result)
    {
        if (result.IsSucceed)
        {
            return result;
        }
        var exception = result.StatusCode switch
        {
            ValidationExceptionBase ex => ex,
            _ => new ValidationException(result.ToString())
        };
        Throw(exception);
        return result;
    }

    public static async Task<Result<TValue>> ProcessAsync<TValue>(this Task<Result<TValue>> resultAsync)
    {
        var result = await resultAsync;
        _ = Process(result);
        return result;
    }

    public static async Task<Result> ProcessResultAsync(this Task<Result> resultAsync)
    {
        var result = await resultAsync;
        return Process(result);
    }

    //public static Result<Stream> SerializeToXmlFile<T>(this Result<Stream> result, string filePath)
    //{
    //    Check.IfArgumentNotNull(filePath);
    //    return result.Fluent(() => new XmlSerializer(typeof(T)).Serialize(result.Value, filePath));
    //}

    //public static Result<Stream> ToFile(this Result<Stream> result, string filePath, FileMode fileMode = FileMode.Create)
    //{
    //    Check.IfArgumentNotNull(filePath);
    //    var stream = result.Value;
    //    using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
    //    stream.CopyTo(fileStream);

    //    return result;
    //}

    //public static Result<StreamWriter> ToStreamWriter(this Result<Stream> result)
    //    => new(new(result.Value));

    //public static Result<string> ToText(this Result<Stream> result)
    //{
    //    var stream = result.Value;
    //    using var reader = new StreamReader(stream);
    //    return new(reader.ReadToEnd());
    //}

    //public static Result<XmlWriter> ToXmlWriter(this Result<Stream> result, bool indent = true)
    //    => new(XmlWriter.Create(result.ToStreamWriter(), new XmlWriterSettings { Indent = indent }));

    public static ivalidationResult Validate<TValue>(this Result<TValue> result)
        => IsValid(result) ? valid.Result : invalid.Result;
}
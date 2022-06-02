using System.Runtime.CompilerServices;
using Library.Exceptions.Validations;

namespace Library.Results;

public static class ResultHelper
{
    public static TResult HasValue<TResult>(this TResult result, object? obj, in object? errorMessage, object? errorId = null)
        where TResult : ResultBase
        => MustBe(result, obj is not null, errorMessage, errorId ?? NullValueValidationException.ErrorCode);

    public static TResult HasValue<TResult>(this TResult result, string? obj, in object errorMessage, object? errorId = null)
        where TResult : ResultBase
        => MustBe(result, !obj.IsNullOrEmpty(), errorMessage, errorId ?? NullValueValidationException.ErrorCode);

    public static bool IsValid<TValue>([NotNullWhen(true)] this Result<TValue> result)
        => result is not null and { IsSucceed: true } and { Value: not null };

    public static TResult MustBe<TResult>(this TResult result, bool condition, in object? errorMessage, object? errorId = null)
        where TResult : ResultBase
    {
        if (!condition)
        {
            result.Errors.Add((errorId, errorMessage ?? string.Empty));
        }

        return result;
    }

    public static TResult MustBe<TResult>(this TResult result, in (bool condition, object? errorMessage, object? errorId) situation)
        where TResult : ResultBase
        => MustBe(result, situation.condition, situation.errorMessage, situation.errorId);

    public static TResult MustBe<TResult>(this TResult result, in (bool condition, object? errorMessage) situation)
        where TResult : ResultBase
        => MustBe(result, situation.condition, situation.errorMessage, null);

    public static TResult MustHaveValue<TResult>(this TResult result, string? obj, [CallerArgumentExpression("obj")] in string? argName = null)
        where TResult : ResultBase
        => MustBe(result, !obj.IsNullOrEmpty(), $"{argName} cannot be empty.", NullValueValidationException.ErrorCode);

    public static ivalidationResult Validate<TValue>(this Result<TValue> result)
        => IsValid(result) ? valid.Result : invalid.Result;
}
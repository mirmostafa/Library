using Library.Exceptions.Validations;
using Library.Results;
using Library.Validations;

namespace Library.Helpers;

public static class ValidationHelper
{
    public static TItem CheckValidation<TItem>([DisallowNull] this IValidatable<TItem> item) =>
        item.Validate().HandleResult();

    public static async Task<TItem> CheckValidationAsync<TItem>([DisallowNull] this IAsyncValidatable<TItem> model) =>
        await model.ValidateAsync().HandleResultAsync();

    public static async Task<TItem> CheckValidatorAsync<TItem>([DisallowNull] this IAsyncValidator<TItem> service, TItem item) =>
        await service.ValidateAsync(item).HandleResultAsync();

    public static TValue HandleResult<TValue>([DisallowNull] this Result<TValue> validationResult)
    {
        if (validationResult.IsSucceed)
        {
            return validationResult.Value;
        }
        var exception = validationResult.StatusCode switch
        {
            ValidationExceptionBase ex => ex,
            _ => new ValidationException(validationResult.ToString())
        };
        Throw(exception);
        return validationResult.Value;
    }
    public static bool HandleResult([DisallowNull] this Result validationResult)
    {
        if (validationResult.IsSucceed)
        {
            return validationResult.IsSucceed;
        }
        var exception = validationResult.StatusCode switch
        {
            ValidationExceptionBase ex => ex,
            _ => new ValidationException(validationResult.ToString())
        };
        Throw(exception);
        return validationResult.IsSucceed;
    }
    public static async Task<TValue> HandleResultAsync<TValue>(this Task<Result<TValue>> validationResultAsync)
    {
        var result = await validationResultAsync;
        _ = HandleResult(result);
        return result.Value;
    }
    public static async Task<bool> HandleResultAsync(this Task<Result> validationResultAsync)
    {
        var result = await validationResultAsync;
        return HandleResult(result);
    }

    public static Result<TItem> Validate<TItem>(Func<TItem> action)
    {
        try
        {
            var item = action();
            return Result<TItem>.CreateSuccess(item);
        }
        catch (ValidationException ex)
        {
            return Result<TItem>.CreateFail(ex.Message);
        }
    }

    public static Result<TItem> Validate<TItem>(Func<TItem> action, TItem item)
    {
        try
        {
            item = action();
            return Result<TItem>.CreateSuccess(item);
        }
        catch (ValidationException ex)
        {
            return Result<TItem>.CreateFail(ex.Message, item);
        }
    }
}
using Library.Exceptions.Validations;
using Library.Results;
using Library.Validations;

namespace Library.Helpers;

public static class ValidationHelper
{
    public static TValue Handle<TValue>([DisallowNull] this Result<TValue> validationResult)
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


    public static TItem CheckValidation<TItem>([DisallowNull] this IValidatable<TItem> item) =>
        item.Validate().Handle();
    public static async Task<TItem> CheckValidationAsync<TItem>([DisallowNull] this IAsyncValidatable<TItem> model) =>
        await model.ValidateAsync().HandleAsync();
    public static async Task<TItem> CheckValidatorAsync<TItem>([DisallowNull] this IAsyncValidator<TItem> service, TItem item) => 
        await service.ValidateAsync(item).HandleAsync();

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

    public static async Task<TValue> HandleAsync<TValue>(this Task<Result<TValue>> validationResultAsync)
    {
        var result = await validationResultAsync;
        Handle(result);
        return result.Value;
    }
}

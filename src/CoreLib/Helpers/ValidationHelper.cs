using Library.Exceptions.Validations;
using Library.Results;
using Library.Validations;

namespace Library.Helpers;

public static class ValidationHelper
{
    public static TItem CheckValidation<TItem>([DisallowNull] this IValidatable<TItem> item)
        => item.Validate().ThrowOnFail();

    public static async Task<TItem> CheckValidationAsync<TItem>([DisallowNull] this IAsyncValidatable<TItem> model)
        => await model.ValidateAsync().ThrowOnFailAsync();

    public static TItem CheckValidator<TItem>([DisallowNull] this IValidator<TItem> validator, TItem item)
        => validator.Validate(item).ThrowOnFail();

    public static async Task<TItem> CheckValidatorAsync<TItem>([DisallowNull] this IAsyncValidator<TItem> validator, TItem item)
        => await validator.ValidateAsync(item).ThrowOnFailAsync();

    public static Result<TItem> Validate<TItem>(Func<TItem> action)
    {
        try
        {
            var item = action();
            return Result<TItem>.CreateSuccess(item);
        }
        catch (ValidationException ex)
        {
            return Result<TItem>.CreateFail(message: ex.Message);
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
            return Result<TItem>.CreateFail(ex.Message, value: item);
        }
    }
}
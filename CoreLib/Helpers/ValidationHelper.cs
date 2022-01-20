using Library.Exceptions.Validations;
using Library.Results;
using Library.Validations;

namespace Library.Helpers;

public static class ValidationHelper
{
    public static TItem Handle<TItem>([DisallowNull] this Result<TItem> validationResult)
    {
        if (validationResult.IsSucceed is not true)
        {
            Throw(new ValidationException(validationResult.Message!));
        }
        return validationResult.Value;
    }

    public static TItem CheckValidation<TItem>([DisallowNull] this IValidatable<TItem> item) =>
        item.Validate().Handle();

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

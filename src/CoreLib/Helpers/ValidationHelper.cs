using Library.Results;
using Library.Validations;

namespace Library.Helpers;

public static class ValidationHelper
{
    //public static TItem CheckValidation<TItem>([DisallowNull] this IValidatable<TItem> item)
    //    => item.Validate().ThrowOnFail();

    //public static async Task<TItem> CheckValidationAsync<TItem>([DisallowNull] this IAsyncValidatable<TItem> model)
    //    => await model.ValidateAsync().ThrowOnFailAsync();

    public static TItem CheckValidator<TItem>([DisallowNull] this IValidator<TItem> validator, TItem item)
        => validator.Validate(item).ThrowOnFail();

    public static async Task<TItem> CheckValidatorAsync<TItem>([DisallowNull] this IAsyncValidator<TItem> validator, TItem item)
        => await validator.ValidateAsync(item).ThrowOnFailAsync();

    public static Result<TInput> ShouldAll<TInput>([DisallowNull] this IEnumerable<Func<TInput, Result<TInput>>> validators, in TInput input)
        => ShouldAll(input, validators.ToArray());

    public static Result<TValue> ShouldAll<TValue>(TValue input, params Func<TValue, Result<TValue>>[] validators)
    {
        var validatorList = validators.ToList();
        Check.IfHasAny(validatorList);
        var result = validatorList.First()(input);

        foreach (var validator in validators.Skip(1))
        {
            result += validator(input);
        }
        return result;
    }
}
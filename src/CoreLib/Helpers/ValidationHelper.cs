using System.Diagnostics;

using Library.Results;
using Library.Validations;

namespace Library.Helpers;

[DebuggerStepThrough]
[StackTraceHidden]
public static class ValidationHelper
{
    public static TItem CheckValidator<TItem>([DisallowNull] this IValidator<TItem> validator, TItem item)
        => validator.Validate(item).ThrowOnFail();

    public static async Task<TItem> CheckValidatorAsync<TItem>([DisallowNull] this IAsyncValidator<TItem> validator, TItem item)
        => await validator.ValidateAsync(item).ThrowOnFailAsync();

    public static Result<TInput> CheckAll<TInput>([DisallowNull] this IEnumerable<Func<TInput, Result<TInput>>> validators, in TInput input)
        => CheckAll(input, validators.ToArray());

    public static Result<TValue> CheckAll<TValue>(TValue input, params Func<TValue, Result<TValue>>[] validators)
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
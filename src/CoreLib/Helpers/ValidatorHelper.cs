using System.Diagnostics;

using Library.Results;
using Library.Validations;

namespace Library.Helpers;

[DebuggerStepThrough]
[StackTraceHidden]
public static class ValidatorHelper
{
    /// <summary>
    /// Checks the given validator and throws an exception if validation fails.
    /// </summary>
    /// <typeparam name="TItem">The type of the item to validate.</typeparam>
    /// <param name="validator">The validator to check.</param>
    /// <param name="item">The item to validate.</param>
    /// <returns>The item if validation succeeds.</returns>
    public static TItem CheckValidator<TItem>([DisallowNull] this IValidator<TItem> validator, TItem item)
        => validator.Validate(item).ThrowOnFail();

    /// <summary>
    /// Asynchronously checks the validity of an item using the specified validator and throws an exception if the validation fails.
    /// </summary>
    public static async Task<TItem> CheckValidatorAsync<TItem>([DisallowNull] this IAsyncValidator<TItem> validator, TItem item)
        => await validator.ValidateAsync(item).ThrowOnFailAsync();

    public static Result<TInput> CheckAll<TInput>([DisallowNull] this IEnumerable<Func<TInput, Result<TInput>>> validators, in TInput input)
        => CheckAll(input, validators.ToArray());

    /// <summary>
    /// Checks the input against a list of validators and returns the result.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="input">The input to check.</param>
    /// <param name="validators">The list of validators to check against.</param>
    /// <returns>The result of the validation.</returns>
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
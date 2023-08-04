using System.Diagnostics;

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
    /// Asynchronously checks the validity of an item using the specified validator and throws an
    /// exception if the validation fails.
    /// </summary>
    public static async Task<TItem> CheckValidatorAsync<TItem>([DisallowNull] this IAsyncValidator<TItem> validator, TItem item)
        => await validator.ValidateAsync(item).ThrowOnFailAsync();
}
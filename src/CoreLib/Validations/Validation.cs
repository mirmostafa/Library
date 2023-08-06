#nullable disable

using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using Library.Exceptions;
using Library.Exceptions.Validations;
using Library.Results;

namespace Library.Validations;

[DebuggerStepThrough]
[StackTraceHidden]
public static class Validation
{
    /// <summary>
    /// Checks that the specified value is not null and returns it. Throws an exception if the value
    /// is null.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="paramName">The name of the argument.</param>
    /// <returns>The value.</returns>
    [return: NotNull]
    public static TValue ArgumentNotNull<TValue>([NotNull] this TValue value, [CallerArgumentExpression(nameof(value))] string paramName = null!) =>
        InnerCheck(value, CheckBehavior.ThrowOnFail, paramName).ArgumentNotNull();

    /// <summary>
    /// Adds a rule to the ValidationResultSet to check if the value is not null.
    /// </summary>
    public static ValidationResultSet<TValue> ArgumentNotNull<TValue>(this ValidationResultSet<TValue> vrs, Func<Exception> onError = null) =>
        vrs.InnerAddRule(x => x, _ => vrs.Value is not null, onError, () => new ArgumentNullException(vrs._valueName));

    /// <summary> Traverses the rules and create a fail <code>Result<TValue></code>, at first broken
    /// rule . Otherwise created a succeed result. </summary>
    public static Result<TValue> Build<TValue>(this ValidationResultSet<TValue> vrs) =>
        InnerBuild(in vrs.Behavior, vrs.Value, vrs.Rules);

    /// <summary>
    /// The entry of validation checks
    /// </summary>
    public static ValidationResultSet<TValue> Check<TValue>(this TValue value, CheckBehavior behavior = CheckBehavior.ReturnFirstFailure, [CallerArgumentExpression(nameof(value))] string paramName = null!) =>
        InnerCheck(value, behavior, paramName);

    public static Result<TValue> GatherToResult<TValue>(this ValidationResultSet<TValue> vrs) =>
            InnerBuild(CheckBehavior.GatherAll, vrs.Value, vrs.Rules);

    public static Result<TValue> GetFirstFailure<TValue>(this ValidationResultSet<TValue> vrs) =>
            InnerBuild(CheckBehavior.ReturnFirstFailure, vrs.Value, vrs.Rules);

    /// <summary>
    /// Checks if the given value is not null and returns it. Throws an exception if the value is null.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="paramName">The name of the argument.</param>
    /// <returns>The value if it is not null.</returns>
    [return: NotNull]
    public static TValue NotNull<TValue>([NotNull] this TValue value, [CallerArgumentExpression(nameof(value))] string paramName = null!) =>
        InnerDefaultCheck(value).NotNull();

    /// <summary>
    /// Checks if the given value is not null and throws an exception if it is.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="onErrorMessage">The error message to throw if the value is null.</param>
    /// <param name="paramName">The name of the argument.</param>
    /// <returns>The value if it is not null.</returns>
    [return: NotNull]
    public static TValue NotNull<TValue>([NotNull] this TValue value, Func<string> onErrorMessage, [CallerArgumentExpression(nameof(value))] string paramName = null!) =>
        InnerDefaultCheck(value).NotNull(onErrorMessage);

    /// <summary>
    /// Checks if the given value is not null and throws an exception if it is.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="onError">The function to create the exception.</param>
    /// <param name="paramName">The name of the argument.</param>
    /// <returns>The value if it is not null.</returns>
    [return: NotNull]
    public static TValue NotNull<TValue>([NotNull] this TValue value, Func<Exception> onError, [CallerArgumentExpression(nameof(value))] string paramName = null) =>
        InnerDefaultCheck(value).NotNull(onError);

    /// <summary>
    /// Adds a rule to the ValidationResultSet to check if the value is not null.
    /// </summary>
    public static ValidationResultSet<TValue> NotNull<TValue>(this ValidationResultSet<TValue> vrs) =>
        vrs.InnerAddRule(x => x, _ => vrs.Value is not null, null, () => new NullValueValidationException(vrs._valueName));

    public static ValidationResultSet<TValue> NotNull<TValue>(this ValidationResultSet<TValue> vrs, Func<Exception> onError) =>
vrs.InnerAddRule(x => x, _ => vrs.Value is not null, onError, () => new NullValueValidationException(vrs._valueName));

    public static ValidationResultSet<TValue> NotNull<TValue>(this ValidationResultSet<TValue> vrs, Func<string> onErrorMessage) =>
vrs.InnerAddRule(x => x, _ => vrs.Value is not null, null, () =>
{
    var message = onErrorMessage?.Invoke();
    return message.IsNullOrEmpty() ? new NullValueValidationException(vrs._valueName) : new NullValueValidationException(message, null);
});

    /// <summary>
    /// Adds a rule to the validation set that checks if the specified property is not null.
    /// </summary>
    public static ValidationResultSet<TValue> NotNull<TValue>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, object>> propertyExpression) =>
        vrs.InnerAddRule(propertyExpression, x => x is not null, null, x => new NullValueValidationException(x));

    public static ValidationResultSet<TValue> NotNull<TValue>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, object>> propertyExpression, Func<Exception> onError) =>
vrs.InnerAddRule(propertyExpression, x => x is not null, onError, x => new NullValueValidationException(x));

    public static ValidationResultSet<TValue> NotNull<TValue>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, object>> propertyExpression, Func<string> onErrorMessage) =>
vrs.InnerAddRule(propertyExpression, x => x is not null, null, x => new NullValueValidationException(onErrorMessage?.Invoke() ?? x));

    public static ValidationResultSet<TValue> NotNullOrEmpty<TValue>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, string>> propertyExpression, Func<Exception> onError = null) =>
vrs.InnerAddRule(propertyExpression, x => !string.IsNullOrEmpty(x), onError, x => new NullValueValidationException(x));

    public static ValidationResultSet<TValue> NotNullOrEmpty<TValue>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, string>> propertyExpression, Func<string> onErrorMessage) =>
vrs.InnerAddRule(propertyExpression, x => !string.IsNullOrEmpty(x), null, x => new NullValueValidationException(onErrorMessage?.Invoke() ?? x));

    public static ValidationResultSet<TValue> RuleFor<TValue>(this ValidationResultSet<TValue> vrs, Func<TValue, bool> isValid, Func<Exception> onError) =>
vrs.InnerAddRule(isValid, onError);

    public static ValidationResultSet<TValue> RuleFor<TValue>(this ValidationResultSet<TValue> vrs, (Func<TValue, bool> IsValid, Func<Exception> OnError) rule) =>
vrs.InnerAddRule(rule.IsValid, rule.OnError);

    /// <summary>
    /// Adds a rule to the validation set with a custom error message.
    /// </summary>
    /// <param name="isValid">The validation rule.</param>
    /// <param name="onErrorMessage">The error message to be returned if the rule fails.</param>
    /// <returns>The validation result set.</returns>
    public static ValidationResultSet<TValue> RuleFor<TValue>(this ValidationResultSet<TValue> vrs, Func<TValue, bool> isValid, Func<string> onErrorMessage) =>
        vrs.InnerAddRule(isValid, () => new ValidationException(onErrorMessage()));

    public static ValidationResultSet<TValue> RuleFor<TValue>(this ValidationResultSet<TValue> vrs, (Func<TValue, bool> IsValid, Func<string> OnErrorMessage) rule) =>
vrs.InnerAddRule(rule.IsValid, () => new ValidationException(rule.OnErrorMessage()));

    public static ValidationResultSet<TValue> ThrowOnFail<TValue>(this ValidationResultSet<TValue> vrs) =>
vrs.Fluent(InnerBuild(CheckBehavior.ThrowOnFail, vrs.Value, vrs.Rules));

    private static Func<Exception> GetOnError<TValue, TType>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, TType>> propertyExpression, Func<string, Exception> onError) =>
                                                        () => onError(ObjectHelper.GetPropertyInfo(vrs.Value, propertyExpression).Name);

    /// <summary>
    /// Adds a rule to the validation result set.
    /// </summary>
    /// <typeparam name="TType">The type of the property.</typeparam>
    /// <param name="propertyExpression">The property expression.</param>
    /// <param name="isValid">The validation function.</param>
    /// <param name="onError">The error function.</param>
    /// <param name="onErrorAlternative">The alternative error function.</param>
    /// <returns>The validation result set.</returns>
    [DebuggerStepThrough]
    [StackTraceHidden]
    private static ValidationResultSet<TValue> InnerAddRule<TValue, TType>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, TType>> propertyExpression, Func<TType, bool> isValid, Func<Exception> onError, Func<Exception> onErrorAlternative)
    {
        [DebuggerStepThrough] bool validator(TValue x) => isValid(Invoke(propertyExpression, x));
        var error = onError ?? onErrorAlternative;
        return InnerAddRule(vrs, validator, error);
    }

    /// <summary>
    /// Adds a rule to the validation set.
    /// </summary>
    /// <param name="validator">The validator function.</param>
    /// <param name="error">The error function.</param>
    /// <returns>The validation result set.</returns>
    [DebuggerStepThrough]
    [StackTraceHidden]
    private static ValidationResultSet<TValue> InnerAddRule<TValue>(this ValidationResultSet<TValue> vrs, Func<TValue, bool> validator, Func<Exception> error)
    {
        if (vrs.Behavior == CheckBehavior.ThrowOnFail)
        {
            if (!validator(vrs.Value))
            {
                Throw(error);
            }
        }
        else
        {
            vrs.RuleList.Add((validator, error));
        }

        return vrs;
    }

    /// <summary>
    /// Adds a rule to the validation result set.
    /// </summary>
    /// <typeparam name="TType">The type of the property.</typeparam>
    /// <param name="propertyExpression">The expression of the property.</param>
    /// <param name="isValid">The validation function.</param>
    /// <param name="onError">The error function.</param>
    /// <param name="onErrorAlternative">The alternative error function.</param>
    /// <returns>The validation result set.</returns>
    [DebuggerStepThrough]
    [StackTraceHidden]
    private static ValidationResultSet<TValue> InnerAddRule<TValue, TType>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, TType>> propertyExpression, Func<TType, bool> isValid, Func<Exception> onError, Func<string, Exception> onErrorAlternative)
    {
        [DebuggerStepThrough] bool validator(TValue x) => isValid(Invoke(propertyExpression, x));
        var error = onError ?? vrs.GetOnError(propertyExpression, onErrorAlternative);
        return vrs.InnerAddRule(validator, error);
    }

    private static Result<TValue> InnerBuild<TValue>(in CheckBehavior behavior, in TValue value, in IEnumerable<(Func<TValue, bool> IsValid, Func<Exception> OnError)> rules)
    {
        // Create a new result object with the current value
        var result = new Result<TValue>(value);

        // Iterate through the rules
        foreach (var (isValid, onError) in rules)
        {
            // Check if the value is not valid
            if (!isValid(value))
            {
                // Depending on the behavior parameter, take the appropriate action
                switch (behavior)
                {
                    // Combine all errors
                    case CheckBehavior.GatherAll:
                        result += Result<TValue>.CreateFailure(onError(), value);
                        break;

                    // Return the first failure
                    case CheckBehavior.ReturnFirstFailure:
                        return Result<TValue>.CreateFailure(onError(), value);

                    // Throw an exception on failure
                    case CheckBehavior.ThrowOnFail:
                        return Result<TValue>.CreateFailure(onError(), value).ThrowOnFail();

                    // Throw an exception if an unsupported behavior is encountered
                    default:
                        throw new UnsupportedOperationException();
                }
            }
        }
        // Return the result
        return result;
    }

    private static ValidationResultSet<TValue> InnerCheck<TValue>(TValue value, CheckBehavior behavior, string paramName) =>
        new(value, behavior, paramName);

    private static ValidationResultSet<TValue> InnerDefaultCheck<TValue>(TValue value, [CallerArgumentExpression(nameof(value))] string paramName = null) =>
        InnerCheck(value, CheckBehavior.ThrowOnFail, paramName);

    private static TType Invoke<TValue, TType>(Expression<Func<TValue, TType>> propertyExpression, TValue value) =>
        propertyExpression.Compile().Invoke(value);
}
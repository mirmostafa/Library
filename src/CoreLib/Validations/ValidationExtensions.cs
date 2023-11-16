#nullable disable

using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using Library.Exceptions;
using Library.Exceptions.Validations;
using Library.Results;

namespace Library.Validations;

#if UNITTEST
    [DebuggerStepThrough]
    [StackTraceHidden]
#endif
public static class ValidationExtensions
{
    /// <summary>
    /// Checks that the specified value is not null and returns it. Throws an exception if the value
    /// is null.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="paramName">The name of the argument.</param>
    /// <returns>The value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNull]
    public static TValue ArgumentNotNull<TValue>([NotNull] this TValue value, [CallerArgumentExpression(nameof(value))] string paramName = null!) =>
        InnerCheck(value, CheckBehavior.ThrowOnFail, paramName).ArgumentNotNull();

    /// <summary>
    /// Adds a rule to the ValidationResultSet to check if the value is not null.
    /// </summary>
    [MemberNotNull(nameof(ValidationResultSet<TValue>.Value))]
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
    public static ValidationResultSet<TValue> NotNull<TValue>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, object>> propertyExpression, string? paramName = null) =>
        vrs.InnerAddRule(propertyExpression, x => x is not null, null, x => new NullValueValidationException(x), paramName);

    public static ValidationResultSet<TValue> NotNull<TValue>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, object>> propertyExpression, Func<Exception> onError) =>
        vrs.InnerAddRule(propertyExpression, x => x is not null, onError, x => new NullValueValidationException(x));

    public static ValidationResultSet<TValue> NotNull<TValue>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, object>> propertyExpression, Func<string> onErrorMessage) =>
        vrs.InnerAddRule(propertyExpression, x => x is not null, null, x => new NullValueValidationException(onErrorMessage?.Invoke() ?? x));

    public static ValidationResultSet<TValue> NotNullOrEmpty<TValue>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, string>> propertyExpression, Func<Exception> onError = null, string? paramName = null) =>
        vrs.InnerAddRule(propertyExpression, x => !string.IsNullOrEmpty(x), onError, x => new NullValueValidationException(x), paramName: paramName);

    public static ValidationResultSet<TValue> NotNullOrEmpty<TValue>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, string>> propertyExpression, Func<string> onErrorMessage) =>
        vrs.InnerAddRule(propertyExpression, x => !string.IsNullOrEmpty(x), null, x => new NullValueValidationException(onErrorMessage?.Invoke() ?? x));

    public static ValidationResultSet<TValue> RuleFor<TValue>(this ValidationResultSet<TValue> vrs, Func<TValue, bool> isValid, Func<Exception> onError) =>
        vrs.InnerAddRule(isValid, onError);

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

    public static bool TryParse<TValue>([DisallowNull] this ValidationResultSet<TValue> input, [NotNull] out Result<TValue> result) =>
        (result = input.Build()).IsSucceed;

    private static Func<Exception> GetOnError<TValue, TType>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, TType>> propertyExpression, Func<string, Exception> onError, string? paramName = null) =>
        () => onError(paramName ?? ObjectHelper.GetPropertyInfo(vrs.Value, propertyExpression).Name);

    /// <summary>
    /// Adds a rule to the validation result set.
    /// </summary>
    /// <typeparam name="TType">The type of the property.</typeparam>
    /// <param name="propertyExpression">The property expression.</param>
    /// <param name="isValid">The validation function.</param>
    /// <param name="onError">The error function.</param>
    /// <param name="onErrorAlternative">The alternative error function.</param>
    /// <returns>The validation result set.</returns>
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
    private static ValidationResultSet<TValue> InnerAddRule<TValue>(this ValidationResultSet<TValue> vrs, Func<TValue, bool> validator, Func<Exception> error)
    {
        Checker.MustBeArgumentNotNull(vrs);
        Checker.MustBeArgumentNotNull(validator);

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
    private static ValidationResultSet<TValue> InnerAddRule<TValue, TType>(this ValidationResultSet<TValue> vrs, Expression<Func<TValue, TType>> propertyExpression, Func<TType, bool> isValid, Func<Exception> onError, Func<string, Exception> onErrorAlternative, string? paramName = null)
    {
        Checker.MustBeArgumentNotNull(vrs);
        Checker.MustBeArgumentNotNull(propertyExpression);
        Checker.MustBeArgumentNotNull(isValid);

        [DebuggerStepThrough] bool validator(TValue x) => isValid(Invoke(propertyExpression, x));
        var error = onError ?? vrs.GetOnError(propertyExpression, onErrorAlternative.ArgumentNotNull(), paramName);
        return InnerAddRule(vrs, validator, error);
    }

    /// <summary>
    /// Builds the result based on the specified behavior, value, and validation rules.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated.</typeparam>
    /// <param name="behavior">The behavior mode for validation.</param>
    /// <param name="value">The value to be validated.</param>
    /// <param name="rules">
    /// A collection of validation rules, each consisting of a validation function and an error handler.
    /// </param>
    /// <returns>The result of the validation based on the specified behavior.</returns>
    private static Result<TValue> InnerBuild<TValue>(in CheckBehavior behavior, in TValue value, in IEnumerable<(Func<TValue, bool> IsValid, Func<Exception> OnError)> rules)
    {
        // Ensure that there is at least one validation rule
        Checker.MustHaveAny(rules);

        // Initialize the result with the provided value
        var result = new Result<TValue>(value);

        // Iterate through the rules
        foreach (var (isValid, onError) in rules)
        {
            // Check if the value is not valid
            if (!isValid(value))
            {
                // Ensure that the error handler function is not null
                Checker.MustBeNotNull(onError);

                // Depending on the behavior parameter, take the appropriate action
                var exception = onError();
                var errorResult = Result<TValue>.CreateFailure(value, errors: [(exception, exception)], status: exception);
                switch (behavior)
                {
                    // Combine all errors
                    case CheckBehavior.GatherAll:
                        result += errorResult;
                        break;

                    // Return the result with the first error and stop checking
                    case CheckBehavior.ReturnFirstFailure:
                        return errorResult;

                    /// Throw an exception on failure. It is verified in the method
                    /// <see cref="InnerAddRule{TValue}(ValidationResultSet{TValue}, Func{TValue, bool}, Func{Exception})"/>
                    /// .
                    case CheckBehavior.ThrowOnFail:
                        // Create a failure result and throw an exception
                        return errorResult.ThrowOnFail();

                    // Throw an exception if an unsupported behavior is encountered
                    default:
                        throw new UnsupportedOperationException();
                }
            }
        }
        // Return the final result after all validations
        return result;
    }

    private static ValidationResultSet<TValue> InnerCheck<TValue>(TValue value, CheckBehavior behavior, string paramName) =>
        new(value, behavior, paramName);

    private static ValidationResultSet<TValue> InnerDefaultCheck<TValue>(TValue value, [CallerArgumentExpression(nameof(value))] string paramName = null) =>
        InnerCheck(value, CheckBehavior.ThrowOnFail, paramName);

    private static TType Invoke<TValue, TType>(Expression<Func<TValue, TType>> propertyExpression, TValue value) =>
        propertyExpression.Compile().Invoke(value);
}
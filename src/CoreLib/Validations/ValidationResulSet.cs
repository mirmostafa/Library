#nullable disable

using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using Library.DesignPatterns.Markers;
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
    /// The entry of validation checks
    /// </summary>
    public static ValidationResultSet<TValue> Check<TValue>(this TValue value, CheckBehavior behavior = CheckBehavior.ReturnFirstFailure, [CallerArgumentExpression(nameof(value))] string paramName = null!) =>
        InnerCheck(value, behavior, paramName);

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

    private static ValidationResultSet<TValue> InnerCheck<TValue>(TValue value, CheckBehavior behavior, string paramName) =>
        new(value, behavior, paramName);

    private static ValidationResultSet<TValue> InnerDefaultCheck<TValue>(TValue value, [CallerArgumentExpression(nameof(value))] string paramName = null) =>
        InnerCheck(value, CheckBehavior.ThrowOnFail, paramName);
}

[DebuggerStepThrough]
[StackTraceHidden]
public sealed class ValidationResultSet<TValue>(TValue value, CheckBehavior behavior, string valueName) : IBuilder<Result<TValue>>
{
    private readonly CheckBehavior _behavior = behavior;
    private readonly List<(Func<TValue, bool> IsValid, Func<Exception> OnError)> _rules = new();
    private readonly string _valueName = valueName;

    public IEnumerable<(Func<TValue, bool> IsValid, Func<Exception> OnError)> Rules
        => this._rules.ToEnumerable();

    public TValue Value { get; } = value;

    public static implicit operator Result<TValue>(ValidationResultSet<TValue> source) =>
        source.Build();

    public static implicit operator TValue(ValidationResultSet<TValue> source) =>
        source.Value;

    /// <summary>
    /// Adds a rule to the ValidationResultSet to check if the value is not null.
    /// </summary>
    public ValidationResultSet<TValue> ArgumentNotNull(Func<Exception> onError = null) =>
        this.InnerAddRule(x => x, _ => this.Value is not null, onError, () => new ArgumentNullException(this._valueName));

    /// <summary> Traverses the rules and create a fail <code>Result<TValue></code>, at first broken
    /// rule . Otherwise created a succeed result. </summary>
    public Result<TValue> Build() =>
        InnerBuild(in this._behavior, this.Value, this.Rules);

    public Result<TValue> GatherToResult() =>
        InnerBuild(CheckBehavior.GatherAll, this.Value, this.Rules);

    public Result<TValue> GetFirstFailure() =>
        InnerBuild(CheckBehavior.ReturnFirstFailure, this.Value, this.Rules);

    /// <summary>
    /// Adds a rule to the ValidationResultSet to check if the value is not null.
    /// </summary>
    public ValidationResultSet<TValue> NotNull() =>
        this.InnerAddRule(x => x, _ => this.Value is not null, null, () => new NullValueValidationException(this._valueName));

    public ValidationResultSet<TValue> NotNull(Func<Exception> onError) =>
        this.InnerAddRule(x => x, _ => this.Value is not null, onError, () => new NullValueValidationException(this._valueName));

    public ValidationResultSet<TValue> NotNull(Func<string> onErrorMessage) =>
        this.InnerAddRule(x => x, _ => this.Value is not null, null, () =>
        {
            var message = onErrorMessage?.Invoke();
            return message.IsNullOrEmpty() ? new NullValueValidationException(this._valueName) : new NullValueValidationException(message, null);
        });

    /// <summary>
    /// Adds a rule to the validation set that checks if the specified property is not null.
    /// </summary>
    public ValidationResultSet<TValue> NotNull(Expression<Func<TValue, object>> propertyExpression) =>
        this.InnerAddRule(propertyExpression, x => x is not null, null, x => new NullValueValidationException(x));

    public ValidationResultSet<TValue> NotNull(Expression<Func<TValue, object>> propertyExpression, Func<Exception> onError) =>
        this.InnerAddRule(propertyExpression, x => x is not null, onError, x => new NullValueValidationException(x));

    public ValidationResultSet<TValue> NotNull(Expression<Func<TValue, object>> propertyExpression, Func<string> onErrorMessage) =>
        this.InnerAddRule(propertyExpression, x => x is not null, null, x => new NullValueValidationException(onErrorMessage?.Invoke() ?? x));

    public ValidationResultSet<TValue> NotNullOrEmpty(Expression<Func<TValue, string>> propertyExpression, Func<Exception> onError = null) =>
        this.InnerAddRule(propertyExpression, x => !string.IsNullOrEmpty(x), onError, x => new NullValueValidationException(x));

    public ValidationResultSet<TValue> NotNullOrEmpty(Expression<Func<TValue, string>> propertyExpression, Func<string> onErrorMessage) =>
        this.InnerAddRule(propertyExpression, x => !string.IsNullOrEmpty(x), null, x => new NullValueValidationException(onErrorMessage?.Invoke() ?? x));

    public ValidationResultSet<TValue> RuleFor(Func<TValue, bool> isValid, Func<Exception> onError) =>
        this.InnerAddRule(isValid, onError);

    public ValidationResultSet<TValue> RuleFor((Func<TValue, bool> IsValid, Func<Exception> OnError) rule) =>
        this.InnerAddRule(rule.IsValid, rule.OnError);

    /// <summary>
    /// Adds a rule to the validation set with a custom error message.
    /// </summary>
    /// <param name="isValid">The validation rule.</param>
    /// <param name="onErrorMessage">The error message to be returned if the rule fails.</param>
    /// <returns>The validation result set.</returns>
    public ValidationResultSet<TValue> RuleFor(Func<TValue, bool> isValid, Func<string> onErrorMessage) =>
        this.InnerAddRule(isValid, () => new ValidationException(onErrorMessage()));

    public ValidationResultSet<TValue> RuleFor((Func<TValue, bool> IsValid, Func<string> OnErrorMessage) rule) =>
        this.InnerAddRule(rule.IsValid, () => new ValidationException(rule.OnErrorMessage()));

    public ValidationResultSet<TValue> ThrowOnFail() =>
        this.Fluent(InnerBuild(CheckBehavior.ThrowOnFail, this.Value, this.Rules));

    private static Result<TValue> InnerBuild(in CheckBehavior behavior, in TValue value, in IEnumerable<(Func<TValue, bool> IsValid, Func<Exception> OnError)> rules)
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

    private static TType Invoke<TType>(Expression<Func<TValue, TType>> propertyExpression, TValue value) =>
        propertyExpression.Compile().Invoke(value);

    private Func<Exception> GetOnError<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<string, Exception> onError) =>
        () => onError(ObjectHelper.GetPropertyInfo(this.Value, propertyExpression).Name);

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
    private ValidationResultSet<TValue> InnerAddRule<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<TType, bool> isValid, Func<Exception> onError, Func<string, Exception> onErrorAlternative)
    {
        [DebuggerStepThrough] bool validator(TValue x) => isValid(Invoke(propertyExpression, x));
        var error = onError ?? this.GetOnError(propertyExpression, onErrorAlternative);
        return this.InnerAddRule(validator, error);
    }

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
    private ValidationResultSet<TValue> InnerAddRule<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<TType, bool> isValid, Func<Exception> onError, Func<Exception> onErrorAlternative)
    {
        [DebuggerStepThrough] bool validator(TValue x) => isValid(Invoke(propertyExpression, x));
        var error = onError ?? onErrorAlternative;
        return this.InnerAddRule(validator, error);
    }

    /// <summary>
    /// Adds a rule to the validation set.
    /// </summary>
    /// <param name="validator">The validator function.</param>
    /// <param name="error">The error function.</param>
    /// <returns>The validation result set.</returns>
    [DebuggerStepThrough]
    [StackTraceHidden]
    private ValidationResultSet<TValue> InnerAddRule(Func<TValue, bool> validator, Func<Exception> error)
    {
        if (this._behavior == CheckBehavior.ThrowOnFail)
        {
            if (!validator(this.Value))
            {
                Throw(error);
            }
        }
        else
        {
            this._rules.Add((validator, error));
        }

        return this;
    }
}

/// <summary>
/// Enum to define the behavior when checking a condition.
/// </summary>
public enum CheckBehavior
{
    GatherAll,
    ReturnFirstFailure,
    ThrowOnFail,
}
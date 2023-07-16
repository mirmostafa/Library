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
    public static TValue ArgumentNotNull<TValue>([NotNull] this TValue value, [CallerArgumentExpression(nameof(value))] string paramName = null!)
        => Check(value, CheckBehavior.ThrowOnFail, paramName).ArgumentNotNull();

    /// <summary>
    /// The entry of validation checks
    /// </summary>
    public static ValidationResultSet<TValue> Check<TValue>(this TValue value, CheckBehavior behavior = CheckBehavior.ReturnFirstFailure, [CallerArgumentExpression(nameof(value))] string paramName = null!)
        => new(value, behavior, paramName);

    /// <summary>
    /// Checks if the given value is not null and returns it. Throws an exception if the value is null.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="paramName">The name of the argument.</param>
    /// <returns>The value if it is not null.</returns>
    [return: NotNull]
    public static TValue NotNull<TValue>([NotNull] this TValue value, [CallerArgumentExpression(nameof(value))] string paramName = null!)
        => Check(value, CheckBehavior.ThrowOnFail).NotNull();

    /// <summary>
    /// Checks if the given value is not null and throws an exception if it is.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="onErrorMessage">The error message to throw if the value is null.</param>
    /// <param name="paramName">The name of the argument.</param>
    /// <returns>The value if it is not null.</returns>
    [return: NotNull]
    public static TValue NotNull<TValue>([NotNull] this TValue value, Func<string> onErrorMessage, [CallerArgumentExpression(nameof(value))] string paramName = null!)
        => Check(value, CheckBehavior.ThrowOnFail).NotNull(onErrorMessage);

    /// <summary>
    /// Checks if the given value is not null and throws an exception if it is.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="onError">The function to create the exception.</param>
    /// <param name="paramName">The name of the argument.</param>
    /// <returns>The value if it is not null.</returns>
    [return: NotNull]
    public static TValue NotNull<TValue>([NotNull] this TValue value, Func<Exception> onError, [CallerArgumentExpression(nameof(value))] string paramName = null!)
        => Check(value, CheckBehavior.ThrowOnFail).NotNull(onError);
}

[DebuggerStepThrough]
[StackTraceHidden]
public sealed class ValidationResultSet<TValue> : IBuilder<Result<TValue>>
{
    #region Fields, ctors and properties

    private readonly CheckBehavior _behavior;
    private readonly List<(Func<TValue, bool> IsValid, Func<Exception> OnError)> _rules;
    private readonly string _valueName;

    public ValidationResultSet(TValue value, CheckBehavior behavior, string valueName)
        => (this.Value, this._valueName, this._behavior, this._rules) = (value, valueName, behavior, new());

    public IEnumerable<(Func<TValue, bool> IsValid, Func<Exception> OnError)> Rules
        => this._rules.ToEnumerable();

    public TValue Value { get; }

    #endregion Fields, ctors and properties

    #region Public methods

    public static implicit operator Result<TValue>(ValidationResultSet<TValue> source) =>
        source.Build();

    public static implicit operator TValue(ValidationResultSet<TValue> source) =>
        source.Value;

    /// <summary> Traverses the rules and create a fail <code>Result<TValue></code>, at first broken
    /// rule . Otherwise created a succeed result. </summary>
    public Result<TValue> Build()
        => this.InnerBuild(this._behavior);

    public Result<TValue> GatherToResult()
        => this.InnerBuild(CheckBehavior.GatherAll);

    public ValidationResultSet<TValue> ThrowOnFail()
    {
        foreach (var (isValid, onError) in this.Rules)
        {
            if (!isValid(this.Value))
            {
                throw onError();
            }
        }
        return this;
    }

    public Result<TValue> ToResult()
        => new(this.Value);

    private Result<TValue> InnerBuild(CheckBehavior behavior)
    {
        // Create a new result object with the current value
        var result = new Result<TValue>(this.Value);

        // Iterate through the rules
        foreach (var (isValid, onError) in this.Rules)
        {
            // Check if the value is not valid
            if (!isValid(this.Value))
            {
                // Depending on the behavior parameter, take the appropriate action
                switch (behavior)
                {
                    // Combine all errors
                    case CheckBehavior.GatherAll:
                        result += Result<TValue>.CreateFailure(onError(), this.Value);
                        break;

                    // Return the first failure
                    case CheckBehavior.ReturnFirstFailure:
                        return Result<TValue>.CreateFailure(onError(), this.Value);

                    // Throw an exception on failure
                    case CheckBehavior.ThrowOnFail:
                        return Result<TValue>.CreateFailure(onError(), this.Value).ThrowOnFail();

                    // Throw an exception if an unsupported behavior is encountered
                    default:
                        throw new UnsupportedOperationException();
                }
            }
        }
        // Return the result
        return result;
    }

    #endregion Public methods

    #region Validators

    /// <summary>
    /// Adds a rule to the ValidationResultSet to check if the value is not null.
    /// </summary>
    [MemberNotNull(nameof(Value))]
    public ValidationResultSet<TValue> ArgumentNotNull(Func<Exception> onError = null)
        => this.AddRule(x => x, _ => this.Value is not null, onError, () => new ArgumentNullException(this._valueName));

    public ValidationResultSet<TValue> ArgumentOutOfRange<TProperty>(Expression<Func<TValue, TProperty>> propertyExpression, Func<TProperty, bool> isValid)
        => this.AddRule(propertyExpression, isValid, null, x => new ArgumentOutOfRangeException(x));

    public ValidationResultSet<TValue> ArgumentOutOfRange(Func<TValue, bool> isValid)
        => this.AddRule(x => x, isValid, null, () => new ArgumentOutOfRangeException(this._valueName));

    public ValidationResultSet<TValue> NotBiggerThan(Expression<Func<TValue, int>> propertyExpression, int max, Func<Exception> onError = null)
        => this.AddRule(propertyExpression, x => !(x > max), onError, x => new ValidationException($"{x} must be bigger than {max}"));

    /// <summary>
    /// Adds a rule to the ValidationResultSet to check if the value is not null.
    /// </summary>
    public ValidationResultSet<TValue> NotNull()
        => this.AddRule(x => x, _ => this.Value is not null, null, () => new NullValueValidationException(this._valueName));

    public ValidationResultSet<TValue> NotNull(Func<Exception> onError)
        => this.AddRule(x => x, _ => this.Value is not null, onError, () => new NullValueValidationException(this._valueName));

    public ValidationResultSet<TValue> NotNull(Func<string> onErrorMessage)
        => this.AddRule(x => x, _ => this.Value is not null, null, () =>
        {
            var message = onErrorMessage?.Invoke();
            return message.IsNullOrEmpty() ? new NullValueValidationException(this._valueName) : new NullValueValidationException(message, null);
        });

    /// <summary>
    /// Adds a rule to the validation set that checks if the specified property is not null.
    /// </summary>
    public ValidationResultSet<TValue> NotNull(Expression<Func<TValue, object>> propertyExpression)
        => this.AddRule(propertyExpression, x => x is not null, null, x => new NullValueValidationException(x));

    public ValidationResultSet<TValue> NotNull(Expression<Func<TValue, object>> propertyExpression, Func<Exception> onError)
        => this.AddRule(propertyExpression, x => x is not null, onError, x => new NullValueValidationException(x));

    public ValidationResultSet<TValue> NotNull(Expression<Func<TValue, object>> propertyExpression, Func<string> onErrorMessage)
        => this.AddRule(propertyExpression, x => x is not null, null, x => new NullValueValidationException(onErrorMessage?.Invoke() ?? x));

    public ValidationResultSet<TValue> NotNullOrEmpty(Expression<Func<TValue, string>> propertyExpression, Func<Exception> onError = null)
        => this.AddRule(propertyExpression, x => !string.IsNullOrEmpty(x), onError, x => new NullValueValidationException(x));

    public ValidationResultSet<TValue> NotNullOrEmpty(Expression<Func<TValue, string>> propertyExpression, Func<string> onErrorMessage)
        => this.AddRule(propertyExpression, x => !string.IsNullOrEmpty(x), null, x => new NullValueValidationException(onErrorMessage?.Invoke() ?? x));

    public ValidationResultSet<TValue> RuleFor(Func<TValue, bool> isValid, Func<Exception> onError)
        => this.InnerAddRule(isValid, onError);

    public ValidationResultSet<TValue> RuleFor((Func<TValue, bool> IsValid, Func<Exception> OnError) rule)
        => this.InnerAddRule(rule.IsValid, rule.OnError);

    /// <summary>
    /// Adds a rule to the validation set with a custom error message.
    /// </summary>
    /// <param name="isValid">The validation rule.</param>
    /// <param name="onErrorMessage">The error message to be returned if the rule fails.</param>
    /// <returns>The validation result set.</returns>
    public ValidationResultSet<TValue> RuleFor(Func<TValue, bool> isValid, Func<string> onErrorMessage)
        => this.InnerAddRule(isValid, () => new ValidationException(onErrorMessage()));

    public ValidationResultSet<TValue> RuleFor((Func<TValue, bool> IsValid, Func<string> OnErrorMessage) rule)
        => this.InnerAddRule(rule.IsValid, () => new ValidationException(rule.OnErrorMessage()));

    #endregion Validators

    #region Private methods

    private static TType Invoke<TType>(Expression<Func<TValue, TType>> propertyExpression, TValue value)
        => propertyExpression.Compile().Invoke(value);

    /// <summary>
    /// Adds a rule to the validation result set.
    /// </summary>
    /// <typeparam name="TType">The type of the property.</typeparam>
    /// <param name="propertyExpression">The expression of the property.</param>
    /// <param name="isValid">The validation function.</param>
    /// <param name="onError">The error function.</param>
    /// <param name="onErrorAlternative">The alternative error function.</param>
    /// <returns>The validation result set.</returns>
    private ValidationResultSet<TValue> AddRule<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<TType, bool> isValid, Func<Exception> onError, Func<string, Exception> onErrorAlternative)
    {
        bool validator(TValue x) => isValid(Invoke(propertyExpression, x));
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
    private ValidationResultSet<TValue> AddRule<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<TType, bool> isValid, Func<Exception> onError, Func<Exception> onErrorAlternative)
    {
        var error = onError ?? onErrorAlternative;
        bool validator(TValue x) => isValid(Invoke(propertyExpression, x));
        return this.InnerAddRule(validator, error);
    }

    private Func<Exception> GetOnError<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<string, Exception> onError)
        => () => onError(this.GetPropertyName(propertyExpression));

    private string GetPropertyName<TType>(Expression<Func<TValue, TType>> propertyExpression)
        => ObjectHelper.GetPropertyInfo(this.Value, propertyExpression).Name;

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

    #endregion Private methods
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
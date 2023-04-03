using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using Library.DesignPatterns.Markers;
using Library.Exceptions;
using Library.Exceptions.Validations;
using Library.Results;

namespace Library.Validations;

public static class Validation
{
    [return: NotNull]
    public static TValue ArgumentNotNull<TValue>([NotNull] this TValue? value, [CallerArgumentExpression(nameof(value))] string argName = null!)
        => Check(value, CheckBehavior.ThrowOnFail).ArgumentNotNull();

    public static ValidationResultSet<TValue> Check<TValue>(this TValue value, CheckBehavior behavior = CheckBehavior.ThrowOnFail, [CallerArgumentExpression(nameof(value))] string argName = null!)
        => new(value, behavior, argName);

    [return: NotNull]
    public static TValue NotNull<TValue>([NotNull] this TValue? value, [CallerArgumentExpression(nameof(value))] string argName = null!)
        => Check(value, CheckBehavior.ThrowOnFail).NotNull();

    [return: NotNull]
    public static TValue NotNull<TValue>([NotNull] this TValue? value, Func<string> onErrorMessage, [CallerArgumentExpression(nameof(value))] string argName = null!)
        => Check(value, CheckBehavior.ThrowOnFail).NotNull(onErrorMessage);

    [return: NotNull]
    public static TValue NotNull<TValue>([NotNull] this TValue? value, Func<Exception> onError, [CallerArgumentExpression(nameof(value))] string argName = null!)
        => Check(value, CheckBehavior.ThrowOnFail).NotNull(onError);
}

public sealed class ValidationResultSet<TValue> : IBuilder<Result<TValue?>>
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

    public static implicit operator TValue(ValidationResultSet<TValue> source)
        => source.Value;

    /// <summary>
    /// Traverses the rules and create a fail <code>Result<TValue></code>, at first broken rule . Otherwise created a succeed result.
    /// </summary>
    /// <returns>Create a fail <code>Result<TValue></code>, at first broken rule . Otherwise created a succeed result.</returns>
    public Result<TValue?> Build()
        => this.InnerBuild(this._behavior);

    public TValue ThrowOnFail()
    {
        foreach (var (isValid, onError) in this.Rules)
        {
            if (!isValid(this.Value))
            {
                throw onError();
            }
        }
        return this.Value;
    }

    public Result<TValue?> ToResult()
        => this.InnerBuild(CheckBehavior.GatherAll);

    private Result<TValue?> InnerBuild(CheckBehavior behavior)
    {
        var result = new Result<TValue?>(this.Value);
        foreach (var (isValid, onError) in this.Rules)
        {
            if (!isValid(this.Value))
            {
                switch (behavior)
                {
                    case CheckBehavior.GatherAll:
                        result += Result<TValue>.CreateFailure(onError(), this.Value);
                        break;

                    case CheckBehavior.ReturnFirstFailure:
                        return Result<TValue>.CreateFailure(onError(), this.Value);

                    case CheckBehavior.ThrowOnFail:
                        return Result<TValue>.CreateFailure(onError(), this.Value).ThrowOnFail();

                    default:
                        throw new UnsupportedOperationException();
                }
            }
        }
        return result;
    }

    #endregion Public methods

    #region Validators

    public ValidationResultSet<TValue> ArgumentNotNull(Func<Exception>? onError = null)
        => this.AddRule(x => x, _ => this.Value is not null, onError, () => new ArgumentNullException(this._valueName));

    public ValidationResultSet<TValue> ArgumentOutOfRange<TProperty>(Expression<Func<TValue, TProperty?>> propertyExpression, Func<TProperty?, bool> isValid)
        => this.AddRule(propertyExpression, isValid, null, x => new ArgumentOutOfRangeException(x));

    [DebuggerStepThrough]
    [StackTraceHidden]
    public ValidationResultSet<TValue> ArgumentOutOfRange(Func<TValue?, bool> isValid)
        => this.AddRule(x => x, isValid, null, () => new ArgumentOutOfRangeException(this._valueName));

    public ValidationResultSet<TValue> NotBiggerThan(Expression<Func<TValue, int>> propertyExpression, int max, Func<Exception>? onError = null)
        => this.AddRule(propertyExpression, x => !(x > max), onError, x => new ValidationException($"{x} must be bigger than {max}"));

    public ValidationResultSet<TValue> NotNull()
        => this.AddRule(x => x, _ => this.Value is not null, null, () => new NullValueValidationException(this._valueName));

    public ValidationResultSet<TValue> NotNull(Func<Exception>? onError)
        => this.AddRule(x => x, _ => this.Value is not null, onError, () => new NullValueValidationException(this._valueName));

    public ValidationResultSet<TValue> NotNull(Func<string> onErrorMessage)
        => this.AddRule(x => x, _ => this.Value is not null, null, () =>
        {
            var message = onErrorMessage?.Invoke();
            return message.IsNullOrEmpty() ? new NullValueValidationException(this._valueName) : new NullValueValidationException(message, null);
        });

    public ValidationResultSet<TValue> NotNull(Expression<Func<TValue, object?>> propertyExpression)
        => this.AddRule(propertyExpression, x => x is not null, null, x => new NullValueValidationException(x));

    public ValidationResultSet<TValue> NotNull(Expression<Func<TValue, object?>> propertyExpression, Func<Exception>? onError)
        => this.AddRule(propertyExpression, x => x is not null, onError, x => new NullValueValidationException(x));

    public ValidationResultSet<TValue> NotNull(Expression<Func<TValue, object>> propertyExpression, Func<string>? onErrorMessage)
        => this.AddRule(propertyExpression, x => x is not null, null, x => new NullValueValidationException(onErrorMessage?.Invoke() ?? x));

    public ValidationResultSet<TValue> NotNullOrEmpty(Expression<Func<TValue, string?>> propertyExpression, Func<Exception>? onError = null)
        => this.AddRule(propertyExpression, x => !string.IsNullOrEmpty(x), onError, x => new NullValueValidationException(x));

    public ValidationResultSet<TValue> NotNullOrEmpty(Expression<Func<TValue, string?>> propertyExpression, Func<string> onErrorMessage)
        => this.AddRule(propertyExpression, x => !string.IsNullOrEmpty(x), null, x => new NullValueValidationException(onErrorMessage?.Invoke() ?? x));

    public ValidationResultSet<TValue> RuleFor(Func<TValue, bool> isValid, Func<Exception> onError)
        => this.InnerAddRule(isValid, onError);

    public ValidationResultSet<TValue> RuleFor((Func<TValue, bool> IsValid, Func<Exception> OnError) rule)
        => this.InnerAddRule(rule.IsValid, rule.OnError);

    public ValidationResultSet<TValue> RuleFor(Func<TValue, bool> isValid, Func<string> onErrorMessage)
        => this.InnerAddRule(isValid, () => new ValidationException(onErrorMessage()));

    public ValidationResultSet<TValue> RuleFor((Func<TValue, bool> IsValid, Func<string> OnErrorMessage) rule)
        => this.InnerAddRule(rule.IsValid, () => new ValidationException(rule.OnErrorMessage()));

    #endregion Validators

    #region Private methods

    private static TType Invoke<TType>(Expression<Func<TValue, TType>> propertyExpression, TValue value)
        => propertyExpression.Compile().Invoke(value);

    private ValidationResultSet<TValue> AddRule<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<TType, bool> isValid, Func<Exception>? onError, Func<string, Exception> onErrorAlternative)
    {
        bool validator(TValue x) => isValid(Invoke(propertyExpression, x));
        var error = onError ?? this.GetOnError(propertyExpression, onErrorAlternative);
        return this.InnerAddRule(validator, error);
    }

    [DebuggerStepThrough]
    [StackTraceHidden]
    private ValidationResultSet<TValue> AddRule<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<TType, bool> isValid, Func<Exception>? onError, Func<Exception> onErrorAlternative)
    {
        var error = onError ?? onErrorAlternative;
        bool validator(TValue x) => isValid(Invoke(propertyExpression, x));
        return this.InnerAddRule(validator, error);
    }

    private Func<Exception> GetOnError<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<string, Exception> onError)
        => () => onError(this.GetPropertyName(propertyExpression));

    private string GetPropertyName<TType>(Expression<Func<TValue, TType>> propertyExpression)
        => ObjectHelper.GetPropertyInfo(this.Value, propertyExpression).Name;

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

public enum CheckBehavior
{
    GatherAll,
    ReturnFirstFailure,
    ThrowOnFail,
}
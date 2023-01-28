﻿using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using Library.DesignPatterns.Markers;
using Library.Exceptions.Validations;
using Library.Results;

namespace Library.Validations;

public sealed class ValidationResultSet<TValue> : IBuilder<Result<TValue>>
{
    #region Fields, ctors and properties

    private readonly List<(Func<TValue, bool> IsValid, Func<Exception> OnError)> _rules;
    private readonly bool _throwOnFail;
    private readonly string _valueName;

    public ValidationResultSet(TValue value, bool throwOnFail, string valueName)
        => (this.Value, this._valueName, this._throwOnFail, this._rules) = (value, valueName, throwOnFail, new());

    public IEnumerable<(Func<TValue, bool> IsValid, Func<Exception> OnError)> Rules
        => this._rules.ToEnumerable();

    public TValue Value { get; }

    #endregion Fields, ctors and properties

    #region Interfaces implementations

    public static implicit operator TValue(ValidationResultSet<TValue> source)
        => source.Value;

    public Result<TValue> Build()
    {
        var result = new Result<TValue>(this.Value);
        foreach (var (isValid, onError) in this.Rules)
        {
            if (!isValid(this.Value))
            {
                result += Result<TValue>.CreateFail(onError(), this.Value);
            }
        }
        return result;
    }

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

    #endregion Interfaces implementations

    #region Validators

    public ValidationResultSet<TValue> ArgumentNotNull(Func<Exception>? onError = null)
        => this.AddRule(x => x, _ => this.Value is not null, onError, () => new ArgumentNullException(this._valueName));

    public ValidationResultSet<TValue> NotBiggerThan(Expression<Func<TValue, int>> propertyExpression, int max, Func<Exception>? onError = null)
        => this.AddRule(propertyExpression, x => !(x > max), onError, x => new ValidationException(x));

    public ValidationResultSet<TValue> NotNull()
        => this.AddRule(x => x, _ => this.Value is not null, null, () => new NullValueValidationException(this._valueName));

    public ValidationResultSet<TValue> NotNull(Func<Exception>? onError)
        => this.AddRule(x => x, _ => this.Value is not null, onError, () => new NullValueValidationException(this._valueName));

    public ValidationResultSet<TValue> NotNull(Func<string> onErrorMessage)
        => this.AddRule(x => x, _ => this.Value is not null, null, () => new NullValueValidationException(onErrorMessage() ?? this._valueName));

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
        => this.AddRole(isValid, onError);
    public ValidationResultSet<TValue> RuleFor((Func<TValue, bool> IsValid, Func<Exception> OnError) rule)
        => this.AddRole(rule.IsValid, rule.OnError);
    public ValidationResultSet<TValue> RuleFor(Func<TValue, bool> isValid, Func<string> onErrorMessage)
        => this.AddRole(isValid, () => new ValidationException(onErrorMessage()));
    public ValidationResultSet<TValue> RuleFor((Func<TValue, bool> IsValid, Func<string> OnErrorMessage) rule)
        => this.AddRole(rule.IsValid, () => new ValidationException(rule.OnErrorMessage()));

    #endregion Validators

    #region Private methods

    private static TType Invoke<TType>(Expression<Func<TValue, TType>> propertyExpression, TValue value)
        => propertyExpression.Compile().Invoke(value);

    private ValidationResultSet<TValue> AddRole(Func<TValue, bool> validator, Func<Exception> error)
    {
        if (this._throwOnFail)
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

    private ValidationResultSet<TValue> AddRule<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<TType, bool> isValid, Func<Exception>? onError, Func<string, Exception> onErrorAlternative)
    {
        bool validator(TValue x) => isValid(Invoke(propertyExpression, x));
        var error = onError ?? this.GetOnError(propertyExpression, onErrorAlternative);
        return this.AddRole(validator, error);
    }

    private ValidationResultSet<TValue> AddRule<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<TType, bool> isValid, Func<Exception>? onError, Func<Exception> onErrorAlternative)
    {
        var error = onError ?? onErrorAlternative;
        bool validator(TValue x) => isValid(Invoke(propertyExpression, x));
        return this.AddRole(validator, error);
    }

    private Func<Exception> GetOnError<TType>(Expression<Func<TValue, TType>> propertyExpression, Func<string, Exception> onError)
        => () => onError(this.GetPropertyName(propertyExpression));

    private string GetPropertyName<TType>(Expression<Func<TValue, TType>> propertyExpression)
        => ObjectHelper.GetPropertyInfo(this.Value, propertyExpression).Name;

    #endregion Private methods
}

public static class Validation
{
    [return: NotNull]
    public static TValue ArgumentNotNull<TValue>([NotNull] this TValue value, [CallerArgumentExpression(nameof(value))] string argName = null!)
        => Check(value, true).ArgumentNotNull();

    public static ValidationResultSet<TValue> Check<TValue>(this TValue value, bool throwOnFail = false, [CallerArgumentExpression(nameof(value))] string argName = null!)
        => new(value, throwOnFail, argName);

    [return: NotNull]
    public static TValue NotNull<TValue>([NotNull] this TValue value, [CallerArgumentExpression(nameof(value))] string argName = null!)
        => Check(value, true).NotNull();

    [return: NotNull]
    public static TValue NotNull<TValue>([NotNull] this TValue value, Func<string> onErrorMessage, [CallerArgumentExpression(nameof(value))] string argName = null!)
        => Check(value, true).NotNull(onErrorMessage);

    [return: NotNull]
    public static TValue NotNull<TValue>([NotNull] this TValue value, Func<Exception> onError, [CallerArgumentExpression(nameof(value))] string argName = null!)
        => Check(value, true).NotNull(onError);
}
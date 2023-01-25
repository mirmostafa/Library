using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using Library.DesignPatterns.Markers;
using Library.Exceptions.Validations;
using Library.Results;

namespace Library.Validations;

public sealed class ValidationResultSet<TValue> : IBuilder<Result<TValue>>
{
    #region Fields, ctors and properties

    private readonly bool _isLazyCheck;
    private readonly List<(Func<TValue, bool> IsValid, Func<Exception> OnError)> _rules;
    private readonly string _valueName;

    public ValidationResultSet(TValue value, bool isLazyCheck, string valueName)
        => (this.Value, this._valueName, this._isLazyCheck, this._rules) = (value, valueName, isLazyCheck, new());

    public IEnumerable<(Func<TValue, bool> IsValid, Func<Exception> OnError)> Rules
        => this._rules.ToEnumerable();

    public TValue Value { get; }

    #endregion Fields, ctors and properties

    #region Interfaces implementations

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
    => this.AddRule(x => x, x => this.Value is not null, onError, () => new ArgumentNullException(this._valueName));

    public ValidationResultSet<TValue> IsNotBiggerThan(Expression<Func<TValue, int>> propertyExpression, int max, Func<Exception>? onError = null)
        => this.AddRule(propertyExpression, x => !(x > max), onError, x => new ValidationException(x));

    public ValidationResultSet<TValue> IsNullOrEmpty(Expression<Func<TValue, string>> propertyExpression, Func<Exception>? onError = null)
        => this.AddRule(propertyExpression, x => !string.IsNullOrEmpty(x), onError, x => new NullReferenceException(x));

    public ValidationResultSet<TValue> RuleFor(Func<TValue, bool> isValid, Func<Exception> onError)
        => this.AddRole(isValid, onError);

    #endregion Validators

    #region Private methods

    private static TType Invoke<TType>(Expression<Func<TValue, TType>> propertyExpression, TValue value)
    => propertyExpression.Compile().Invoke(value);

    private ValidationResultSet<TValue> AddRole(Func<TValue, bool> validator, Func<Exception> error)
    {
        if (!this._isLazyCheck)
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
    public static ValidationResultSet<TValue> Check<TValue>(this TValue value, bool isLazyCheck = true, [CallerArgumentExpression(nameof(value))] string argName = null!)
        => new(value, isLazyCheck, argName);
}
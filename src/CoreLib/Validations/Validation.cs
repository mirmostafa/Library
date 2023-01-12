#nullable disable

using System.Diagnostics;
using System.Runtime.CompilerServices;

using Library.DesignPatterns.Markers;
using Library.Exceptions.Validations;
using Library.Results;

namespace Library.Validations;

public static class ValidationExtension
{
    public static ValidationResult<TValue> ArgumentIsNotNull<TValue>(this ValidationResult<TValue> validation, (object Id, object Data)? ifIsNull = null)
        => Is(validation, x => x is null, ifIsNull ?? (-1, new ArgumentNullException(validation.VariableName)));

    [DebuggerStepThrough]
    public static ValidationCheck<TValue> Be<TValue>(this ValidationCheck<TValue> validation, Func<TValue, bool> predicate, Func<Exception> onError)
    {
        if (predicate(validation.GetValue()))
        {
            Throw(onError);
        }
        return validation;
    }

    [DebuggerStepThrough]
    public static ValidationCheck<TValue> BeNotNull<TValue>(this ValidationCheck<TValue> validation)
        => Be(validation, x => x?.Equals(default) ?? true, () => new NullValueValidationException(validation.VariableName));

    [DebuggerStepThrough]
    public static ValidationCheck<TValue> BeNotNull<TValue>(this ValidationCheck<TValue> validation, Func<string> errorMessage)
        => Be(validation, x => x?.Equals(default) ?? true, () => new NullValueValidationException(errorMessage(), null));

    public static ValidationResult<TValue> If<TValue>(this TValue value, [CallerArgumentExpression("value")] string argName = null)
        => new(value, argName);

    public static ValidationResult<TValue> Is<TValue>(this ValidationResult<TValue> validation, Func<TValue, bool> predicate, (object Id, object Data)? ifIsNull = null)
    {
        if (predicate(validation.GetResult().Value))
        {
            validation.GetResult().Errors.Add(ifIsNull ?? (-1, null!));
        }
        return validation;
    }

    public static ValidationResult<TValue> IsNotNull<TValue>(this ValidationResult<TValue> validation, (object Id, object Data)? ifIsNull = null)
        => Is(validation, x => x?.Equals(default) ?? true, ifIsNull ?? (-1, new NullValueValidationException(validation.VariableName)));

    public static ValidationCheck<TValue> Should<TValue>(this TValue value, [CallerArgumentExpression("value")] string argName = null)
            => new(value, argName);

    public static TValue ThrowOnFail<TValue>(this ValidationResult<TValue> validation)
    {
        _ = validation.GetResult().ThrowOnFail();
        return validation.GetValue();
    }
}

public sealed class ValidationResult<TValue> : IBuilder<TValue>
{
    private readonly Result<TValue> _result;

    internal ValidationResult(TValue value, string variableName)
    {
        this._result = new(value);
        this.VariableName = variableName;
    }

    internal string VariableName { get; }

    public static implicit operator Result<TValue>(ValidationResult<TValue> validation)
        => validation.GetResult();

    public TValue Build()
        => this.ThrowOnFail();

    public Result<TValue> GetResult()
        => this._result;

    public TValue GetValue()
        => this.GetResult().Value;
}

public sealed record ValidationCheck<TValue>
{
    internal ValidationCheck(TValue value, string variableName)
    {
        this.Value = value;
        this.VariableName = variableName;
    }

    internal string VariableName { get; init; }

    public TValue Value { get; }

    public TValue GetValue()
        => this.Value;
}
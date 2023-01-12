#nullable disable

using System.Runtime.CompilerServices;

using Library.DesignPatterns.Markers;
using Library.Exceptions.Validations;
using Library.Results;

namespace Library.Validations;

public static class ValidationExtension
{
    public static Validation<TValue> ArgumentBeNotNull<TValue>(this Validation<TValue> validation, (object Id, object Data)? ifIsNull = null)
        => Be(validation, x => x is null, ifIsNull ?? (-1, new ArgumentNullException(validation.VariableName)));

    public static Validation<TValue> Be<TValue>(this Validation<TValue> validation, Func<TValue, bool> predicate, (object Id, object Data)? ifIsNull = null)
    {
        if (predicate(validation.GetResult().Value))
        {
            validation.GetResult().Errors.Add(ifIsNull ?? (-1, null!));
        }
        return validation;
    }

    public static Validation<TValue> BeNotNull<TValue>(this Validation<TValue> validation, (object Id, object Data)? ifIsNull = null)
        => Be(validation, x => x?.Equals(default) ?? true, ifIsNull ?? (-1, new NullValueValidationException(validation.VariableName)));

    public static Validation<TValue> Should<TValue>(this TValue value, [CallerArgumentExpression("value")] string argName = null)
        => new(value, argName);

    public static TValue ThrowOnFail<TValue>(this Validation<TValue> validation)
    {
        _ = validation.GetResult().ThrowOnFail();
        return validation.GetValue();
    }
}

public sealed class Validation<TValue> : IBuilder<TValue>
{
    private readonly Result<TValue> _result;

    internal Validation(TValue value, string variableName)
    {
        this._result = new(value);
        this.VariableName = variableName;
    }

    internal string VariableName { get; }

    public static implicit operator Result<TValue>(Validation<TValue> validation)
        => validation.GetResult();

    public TValue Build()
        => this.ThrowOnFail();

    public Result<TValue> GetResult() 
        => this._result;

    public TValue GetValue()
        => this.GetResult().Value;
}
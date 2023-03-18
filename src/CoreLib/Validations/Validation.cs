#nullable disable

using System.Diagnostics;
using System.Runtime.CompilerServices;

using Library.DesignPatterns.Markers;
using Library.Exceptions.Validations;
using Library.Results;

namespace Library.Validations;

[Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
public static class ValidationExtension
{
    public static ValidationResult<TValue> ArgumentIsNotNull<TValue>(this ValidationResult<TValue> validation, (object Id, object Data)? ifIsNull = null)
        => Is(validation, x => x is null, ifIsNull ?? (-1, new ArgumentNullException(validation.VariableName)));

    public static ValidationResult<string> ArgumentIsNotNull(this ValidationResult<string> validation, (object Id, object Data)? ifIsNull = null)
        => Is(validation, x => x.IsNullOrEmpty(), ifIsNull ?? (-1, new ArgumentNullException(validation.VariableName)));

    [DebuggerStepThrough]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static ValidationCheck<TValue> Be<TValue>(this ValidationCheck<TValue> validation, in Func<TValue, bool> predicate, in Func<Exception> onError)
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
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static ValidationCheck<TValue> BeNotNull<TValue>(this ValidationCheck<TValue> validation, Func<string> errorMessage)
        => Be(validation, x => x?.Equals(default) ?? true, () => new NullValueValidationException(errorMessage(), null));

    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static ValidationResult<TValue> If<TValue>(this TValue value, [CallerArgumentExpression(nameof(value))] string argName = null)
        => new(value, argName);

    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static ValidationResult<TValue> Is<TValue>(this ValidationResult<TValue> validation, in Func<TValue, bool> predicate, (object Id, object Data)? ifIsNull = null)
    {
        if (predicate(validation.Result.Value))
        {
            var oldResult = validation.Result;
            var newResult = oldResult with { Succeed = false, Errors = EnumerableHelper.ToEnumerable(ifIsNull ?? (-1, null!)) };
            return new(newResult, validation.VariableName);
        }
        return validation;
    }

    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static ValidationResult<TValue> IsNotNull<TValue>(this ValidationResult<TValue> validation, (object Id, object Data)? ifIsNull = null)
        => Is(validation, x => x?.Equals(default) ?? true, ifIsNull ?? (-1, new NullValueValidationException(validation.VariableName)));

    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static ValidationCheck<TValue> Should<TValue>(this TValue value, [CallerArgumentExpression(nameof(value))] string argName = null)
            => new(value, argName);

    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static TValue ThrowOnFail<TValue>(this ValidationResult<TValue> validation)
    {
        var result = validation.Result;
        return result.ThrowOnFail();
    }
}

[Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
public sealed class ValidationResult<TValue> : IBuilder<TValue>
{
    internal ValidationResult(TValue value, string variableName)
    {
        this.Result = value is Result<TValue> r ? r : new(value);
        this.VariableName = variableName;
    }

    internal ValidationResult(Result<TValue> result, string variableName)
    {
        this.Result = result;
        this.VariableName = variableName;
    }

    public Result<TValue> Result { get; }

    public TValue Value => this.Result.Value;

    internal string VariableName { get; }

    public static implicit operator Result<TValue>(in ValidationResult<TValue> validation)
        => validation.Result;

    public TValue BuildAll()
        => this.ThrowOnFail();
}

[Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
public sealed record ValidationCheck<TValue>
{
    internal ValidationCheck(in TValue value, in string variableName)
    {
        this.Value = value;
        this.VariableName = variableName;
    }

    internal string VariableName { get; init; }

    public TValue Value { get; }

    public TValue GetValue()
        => this.Value;

    public static implicit operator TValue(ValidationCheck<TValue> vc)
        => vc.Value;
}
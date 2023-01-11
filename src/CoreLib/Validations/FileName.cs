using Library.Results;

namespace Library.Validations;

public static class ValidationExtension
{
    public static Validation<TValue> Be<TValue>(this Validation<TValue> validation, Func<TValue, bool> predicate, (object? Id, object Data)? ifIsNull = null)
    {
        if (!predicate(validation.Result.Value))
        {
            validation.Result.Errors.Add(ifIsNull ?? (-1, null!));
        }
        return validation;
    }

    public static Validation<TValue> BeNotNull<TValue>(this Validation<TValue> validation, (object? Id, object Data)? ifIsNull = null)
        => Be(validation, x => !(x?.Equals(default) ?? true), ifIsNull ?? (-1, null!));

    public static Validation<TValue> Should<TValue>(this TValue value)
        => new(value);

    public static TValue ThrowOnFail<TValue>(this Validation<TValue> validation)
    {
        _ = validation.Result.ThrowOnFail();
        return validation.GetValue();
    }
}

public sealed class Validation<TValue>
{
    public Validation(TValue value) 
        => this.Result = new(value);

    public Result<TValue> Result { get; }
    public TValue GetValue() 
        => this.Result.Value;

    public static implicit operator Result<TValue>(Validation<TValue> validation)
        => validation.Result;
}
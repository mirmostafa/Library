using Library.Results;

namespace Library.Validations;

public interface IAsyncValidator
{
    Task<Result> ValidateAsync();
}

public interface IAsyncValidator<TItem>
{
    Task<Result<TItem?>> ValidateAsync(TItem? item);
}

public interface IValidatable<T>
{
    Result<T> Validate();
}

public interface IValidator
{
    Result Validate();
}

public interface IValidator<TItem>
{
    Result<TItem?> Validate(in TItem? item);
}
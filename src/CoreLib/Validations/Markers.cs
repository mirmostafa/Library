using Library.Results;

namespace Library.Validations;

public interface IValidatable
{
    Result Validate();
}
public interface IValidatable<T>
{
    Result<T> Validate();
}

public interface IAsyncValidatable
{
    Task<Result> ValidateAsync();
}

public interface IAsyncValidatable<T>
{
    Task<Result<T>> ValidateAsync();
}

public interface IValidator<TItem>
{
    Result<TItem> Validate(TItem item);
}
public interface IAsyncValidator<TItem>
{
    Task<Result<TItem>> ValidateAsync(TItem item);
}

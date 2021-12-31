using Library.Results;

namespace Library.Validations;

public interface IValidatable
{
    Result IsValid();
}

public interface IAsyncValidatable
{
    Task<Result> IsValidAsync();
}

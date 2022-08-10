namespace Library.Web.Results;

public interface IApiResult
{
    bool IsFailure { get; }
    string? Message { get; }
    object? Status { get; }
    bool IsSucceed { get; }
}

public interface IApiResult<T> : IApiResult
{
    T? Value { get; }
}
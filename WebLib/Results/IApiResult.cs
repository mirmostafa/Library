namespace Library.Web.Results;

public interface IApiResult
{
    bool Failure { get; }
    string? Message { get; }
    int? StatusCode { get; }
    bool IsSucceed { get; }
}

public interface IApiResult<T> : IApiResult
{
    T? Value {  get; }
}
namespace Library.Cqrs;

public interface IQueryParameter<TQueryResult>
{
}
public interface IQueryResult<TResult> : IQueryResult
{
    TResult Result { get; }
}

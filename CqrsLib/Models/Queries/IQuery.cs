namespace Library.Cqrs;

public interface IQuery<TQueryResult>
{
}
public interface IQueryResult<TResult> : IQueryResult
{
    TResult Result { get; }
}

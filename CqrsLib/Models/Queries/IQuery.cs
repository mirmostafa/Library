namespace Library.Cqrs.Models.Queries;

public interface IQuery<TQueryResult>
{
}
public interface IQueryResult<TResult> : IQueryResult
{
    TResult Result { get; }
}

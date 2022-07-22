namespace Library.Cqrs.Models.Queries;

public interface IQueryResult
{
}

public interface IQueryResult<TResult> : IQueryResult
{
    TResult Result { get; }
}
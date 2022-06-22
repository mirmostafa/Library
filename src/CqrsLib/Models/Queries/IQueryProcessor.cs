namespace Library.Cqrs.Models.Queries;

public interface IQueryProcessor
{
    Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query);
}

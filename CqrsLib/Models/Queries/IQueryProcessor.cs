namespace Library.Cqrs;

public interface IQueryProcessor
{
    Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query);
}

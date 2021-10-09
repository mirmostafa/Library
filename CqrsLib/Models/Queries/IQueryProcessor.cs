namespace Library.Cqrs;

public interface IQueryProcessor
{
    Task<TResult> ExecuteAsync<TResult>(IQueryParameter<TResult> query);
}

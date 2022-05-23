namespace Library.Cqrs.Models.Commands;

public interface ICommandProcessor
{
    Task<TResult> ExecuteAsync<TCommand, TResult>(TCommand command);
}
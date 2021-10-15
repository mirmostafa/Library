namespace Library.Cqrs;

public interface ICommandProcessor
{
    //Task<CommandResult<TResult>> ExecuteAsync<TCommand, TResult>(TCommand command);
    Task<TResult> ExecuteAsync<TCommand, TResult>(TCommand command);
}


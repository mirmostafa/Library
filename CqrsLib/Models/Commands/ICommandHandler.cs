namespace Library.Cqrs.Models.Commands;

public interface ICommandHandler<in TCommand, TCommandResult>
    where TCommand : ICommand
    // where TCommand : ICommand
{
    //xTask<CommandResult<TCommandResult>> HandleAsync(TCommand command);
    Task<TCommandResult> HandleAsync(TCommand command);
}

namespace Library.Cqrs;

public interface ICommandHandler<in TCommandParameters, TCommandResult>
    where TCommandParameters : ICommandParameter
    // where TCommandResult : ICommandResult
{
    //xTask<CommandResult<TCommandResult>> HandleAsync(TCommandParameters command);
    Task<TCommandResult> HandleAsync(TCommandParameters command);
}

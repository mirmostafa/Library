using Library.Cqrs.Models.Commands;

namespace Library.Cqrs.Engine.Command;

public sealed class ValidationCommandHandlerDecorator<TCommand, TResult>(
      ICommandHandler<TCommand, TResult> decoratedHandler
    , ICommandValidator<TCommand> validator)
    : ICommandHandler<TCommand, TResult>
        where TCommand : ICommand
        where TResult : ICommandResult
{
    public async Task<TResult> HandleAsync(TCommand command)
    {
        await validator.ValidateAsync(command);
        return await decoratedHandler.HandleAsync(command);
    }
}
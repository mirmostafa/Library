using Library.Cqrs.Models.Commands;

namespace Library.Cqrs.Engine.Command;

public sealed class ValidationCommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand
    where TResult : ICommandResult
{
    private readonly ICommandHandler<TCommand, TResult> _decoratedHandler;
    private readonly ICommandValidator<TCommand> _validator;

    public ValidationCommandHandlerDecorator(ICommandHandler<TCommand, TResult> decoratedHandler, ICommandValidator<TCommand> validator)
        => (this._decoratedHandler, this._validator) = (decoratedHandler, validator);

    public async Task<TResult> HandleAsync(TCommand command)
    {
        await this._validator.ValidateAsync(command);
        return await this._decoratedHandler.HandleAsync(command);
    }
}
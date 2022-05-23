namespace Library.Cqrs.Models.Commands;

public interface ICommandValidator<in TCommand>
    where TCommand : ICommand
{
    ValueTask ValidateAsync(TCommand command);
}
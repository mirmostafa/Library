namespace Library.Cqrs;

public interface ICommandValidator<in TCommand>
    where TCommand : ICommand
{
    ValueTask ValidateAsync(TCommand command);
}

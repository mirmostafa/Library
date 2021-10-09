namespace Library.Cqrs;

public interface ICommandValidator<in TCommand>
    where TCommand : ICommandParameter
{
    ValueTask ValidateAsync(TCommand command);
}

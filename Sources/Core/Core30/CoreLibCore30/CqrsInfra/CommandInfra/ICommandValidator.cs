using System.Threading.Tasks;

namespace Mohammad.CqrsInfra.CommandInfra
{
    public interface ICommandValidator<in TCommand> where TCommand : ICommand
    {
        ValueTask ValidateAsync(TCommand command);
    }
}
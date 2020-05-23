using System.Threading.Tasks;

namespace Mohammad.CqrsInfra.CommandInfra
{
    public interface ICommandHandler<in TCommand, TResult>
        where TCommand : ICommand
        where TResult : ICommandResult
    {
        Task<CommandResult<TResult>> HandleAsync(TCommand command);
    }
}
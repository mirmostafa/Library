using System.Threading.Tasks;

namespace Mohammad.CqrsInfra.CommandInfra
{
    public interface ICommandProcessor
    {
        Task<CommandResult<TResult>> ExecuteAsync<TCommand, TResult>(TCommand command);
    }
}
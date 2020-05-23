using System.Threading.Tasks;
using Autofac;

namespace Mohammad.CqrsInfra.CommandInfra
{
    public sealed class CommandProcessor : ICommandProcessor
    {
        private readonly ILifetimeScope _container;

        public CommandProcessor(ILifetimeScope container) => this._container = container;

        public Task<CommandResult<TResult>> ExecuteAsync<TCommand, TResult>(TCommand command)
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            dynamic handler = this._container.ResolveKeyed("2", handlerType);
            return handler.HandleAsync((dynamic)command);
        }
    }
}
using Autofac;
using Library.Cqrs.Models.Commands;
using Library.Validations;

namespace Library.Cqrs.Engine.Command;

internal sealed class CommandProcessor : ICommandProcessor
{
    private readonly ILifetimeScope _container;

    public CommandProcessor(ILifetimeScope container) =>
        this._container = container;

#if !DEBUG
        [System.Diagnostics.DebuggerStepThrough]
#endif
    public Task<TCommandResult> ExecuteAsync<Parameters, TCommandResult>(Parameters parameters)
    {
        Check.MustBeArgumentNotNull(parameters);

        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(parameters.GetType(), typeof(TCommandResult));
        dynamic handler = this._container.ResolveKeyed("2", handlerType);
        return handler.HandleAsync((dynamic)parameters);
    }
}
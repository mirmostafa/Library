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
    //public Task<CommandResult<TCommandResult>> ExecuteAsync<Parameters, TCommandResult>(Parameters parameters)
    //{
    //    if (parameters is null)
    //    {
    //        throw new NullReferenceException(nameof(parameters));
    //    }

    //    var handlerType = typeof(ICommandHandler<,>).MakeGenericType(parameters.GetType(), typeof(TCommandResult));
    //    dynamic handler = this._container.ResolveKeyed("2", handlerType);
    //    return handler.HandleAsync((dynamic)parameters);
    //}

    public Task<TCommandResult> ExecuteAsync<Parameters, TCommandResult>(Parameters parameters)
    {
        Check.IfArgumentNotNull(parameters);

        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(parameters.GetType(), typeof(TCommandResult));
        dynamic handler = this._container.ResolveKeyed("2", handlerType);
        return handler.HandleAsync((dynamic)parameters);
    }
}

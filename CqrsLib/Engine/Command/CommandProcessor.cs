using Autofac;
using HanyCo.Infra.Markers;
using HanyCo.Infra.Security.Services;

namespace Library.Cqrs.Engine.Command;

internal sealed class CommandProcessor : ICommandProcessor
{
    private readonly ILifetimeScope _container;
    private readonly ISecurityService _securityService;

    public CommandProcessor(ILifetimeScope container, ISecurityService securityService)
    {
        this._container = container;
        this._securityService = securityService;
    }

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
        if (parameters is null)
        {
            throw new NullReferenceException(nameof(parameters));
        }
        if(parameters is ISecurityHeadered securityHeadered)
        {
            //this._securityService.HasPermissionAsync(entryId, HttpContext)
        }
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(parameters.GetType(), typeof(TCommandResult));
        dynamic handler = this._container.ResolveKeyed("2", handlerType);
        return handler.HandleAsync((dynamic)parameters);
    }
}

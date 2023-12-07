using Autofac;

using Library.Cqrs.Models.Commands;
using Library.Validations;

namespace Library.Cqrs.Engine.Command;

internal sealed class CommandProcessor(ILifetimeScope container) : ICommandProcessor
{
    private readonly ILifetimeScope _container = container;

#if !DEBUG
        [System.Diagnostics.DebuggerStepThrough]
#endif
    public async Task<TCommandResult> ExecuteAsync<Parameters, TCommandResult>(Parameters parameters)
    {
        Check.MustBeArgumentNotNull(parameters);

        var validatorType = typeof(ICommandValidator<>).MakeGenericType(parameters.GetType());
        if (validatorType != null)
        {
            dynamic validator = this._container.ResolveKeyed("3", validatorType);
            if (validator != null)
            {
                await validator.ValidateAsync((dynamic)parameters);
            }
        }
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(parameters.GetType(), typeof(TCommandResult));
        dynamic handler = this._container.ResolveKeyed("2", handlerType);
        return await handler.HandleAsync((dynamic)parameters);
    }
}
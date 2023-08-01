using Autofac;
using Library.Cqrs.Models.Queries;
using Library.Validations;

namespace Library.Cqrs.Engine.Query;

internal sealed class QueryProcessor : IQueryProcessor
{
    private readonly ILifetimeScope _container;

    public QueryProcessor(ILifetimeScope container)
        => this._container = container;

#if !DEBUG
        [System.Diagnostics.DebuggerStepThrough]
#endif
    public Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query)
    {
        Check.MustBeArgumentNotNull(query);

        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        dynamic handler = this._container.ResolveKeyed("1", handlerType);
        return handler.HandleAsync((dynamic)query);
    }
}
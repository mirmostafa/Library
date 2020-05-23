using System.Threading.Tasks;
using Autofac;

namespace Mohammad.CqrsInfra.QueryInfra
{
    public sealed class QueryProcessor : IQueryProcessor
    {
        #region Fields

        private readonly ILifetimeScope _Container;

        #endregion

        public QueryProcessor(ILifetimeScope container) => this._Container = container;

        public Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = this._Container.ResolveKeyed("1", handlerType);
            return handler.HandleAsync((dynamic)query);
        }
    }
}
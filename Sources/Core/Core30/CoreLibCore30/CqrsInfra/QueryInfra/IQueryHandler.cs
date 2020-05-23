using System.Threading.Tasks;

namespace Mohammad.CqrsInfra.QueryInfra
{
    public interface IQueryHandler<in TQuery, TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
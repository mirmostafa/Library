using System.Threading.Tasks;

namespace Mohammad.CqrsInfra.QueryInfra
{
    public interface IQueryProcessor
    {
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query);
    }
}
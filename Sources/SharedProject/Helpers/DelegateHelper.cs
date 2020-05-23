using System;
using System.Threading.Tasks;

namespace Mohammad.Helpers
{
    public static class DelegateHelper
    {
        public static TEntity Apply<TEntity>(this TEntity entity, Action<TEntity> action)
        {
            action(entity);
            return entity;
        }

        public static TEntity Apply<TEntity>(this Func<TEntity> entity, Action<TEntity> action)
        {
            var result = entity();
            action(result);
            return result;
        }

        public static async Task<TEntity> ApplyAsync<TEntity>(this TEntity entity, Func<TEntity, Task> action)
        {
            await action(entity);
            return entity;
        }

        public static async Task<TEntity> ApplyAsync<TEntity>(this Func<TEntity> entity, Func<TEntity, Task> action)
        {
            var result = entity();
            await action(result);
            return result;
        }
    }
}
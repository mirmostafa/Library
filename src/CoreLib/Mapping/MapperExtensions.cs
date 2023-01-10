using System.Reflection;

using Library.Data.Markers;

namespace Library.Mapping;

public static class MapperExtensions
{
    public static TEntity ForMember<TEntity>(this TEntity entity, in Action<TEntity> action)
        where TEntity : IEntity
    {
        action(entity);
        return entity;
    }

    public static TEntity ForMember<TEntity>(in TEntity entity, in Action<TEntity> action)
        => entity.Fluent(action);

    public static TEntity? ForMemberIfNotNull<TEntity>(TEntity? entity, Action<TEntity> action)
            => Functional.IfTrue(entity is not null && action is not null, () => entity.Fluent(action));
}
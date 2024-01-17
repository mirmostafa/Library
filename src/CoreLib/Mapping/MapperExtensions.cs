using Library.Data.Markers;
using Library.Validations;

namespace Library.Mapping;

public static class MapperExtensions
{
    public static TEntity ForMember<TEntity>(this TEntity entity, in Action<TEntity> action)
        where TEntity : IEntity
    {
        Check.MustBeArgumentNotNull(action);
        action(entity);
        return entity;
    }

    public static TEntity ForMember<TEntity>(in TEntity entity, in Action<TEntity> action)
        => entity.Fluent(action);

    public static TEntity? ForMemberIfNotNull<TEntity>(TEntity? entity, Action<TEntity> action)
            => CodeHelper.IfTrue<Fluency<TEntity>>(entity is not null && action is not null, () => entity.ArgumentNotNull().Fluent(action));
}
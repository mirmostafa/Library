using System.Reflection;

namespace Library.Mapping;
public static class MapperExtensions
{
    public static TEntity ForMember<TEntity>(this TEntity entity, in Action<TEntity> action)
    {
        action(entity);
        return entity;
    }
    public static TEntity? ForMemberIfNotNull<TEntity>(this TEntity? entity, Action<TEntity> action)
        => IfTrue(entity is not null && action is not null, () => entity.Fluent(action));

    public static TEntity ForMember<TEntity>(in TEntity entity, in Action<TEntity> action)
        => entity.Fluent(action);

    internal static void Copy<TSource, TDestination>(TSource source, TDestination destination, PropertyInfo dstProp)
        where TDestination : class
    {
        var mapping = dstProp.GetCustomAttribute<MappingAttribute>()?.MapFrom;
        var name = (mapping is { } info) && (info.SourceClassName is null || info.SourceClassName == typeof(TDestination).Name)
                ? info.SourcePropertyName ?? dstProp.Name
                : dstProp.Name;
        if (source!.GetType().GetProperty(name) is { } srcProp)
        {
            var (match, ex) = CatchFunc(() => srcProp.GetValue(source) == dstProp.GetValue(destination), false);
            if (!match)
            {
                try
                {
                    dstProp.SetValue(destination, srcProp.GetValue(source));
                }
                catch 
                {

                }
            }
        }
    }
}

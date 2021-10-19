using System.Reflection;

namespace Library.Mapping;
public static class MapperExtensions
{
    public static TEntity ForMember<TEntity>(this TEntity entity, in Action<TEntity> action)
        => Fluent(entity, action);

    public static TEntity ForMember<TEntity>(in TEntity entity, in Action<TEntity> action)
        => Fluent(entity, action);

    public static void Copy<TSource, TDestination>(TSource source, TDestination destination, PropertyInfo dstProp)
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
                _ = Catch(() => dstProp.SetValue(destination, srcProp.GetValue(source)));
            }
        }
    }
}

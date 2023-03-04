using System.Diagnostics;
using System.Reflection;

using Library.Interfaces;
using Library.Validations;

namespace Library.Mapping;

[DebuggerStepThrough]
[StackTraceHidden]
public sealed class Mapper : IMapper
{
    [return: MaybeNull]
    public TDestination Map<TSource, TDestination>(in TSource source, in TDestination destination)
        where TDestination : class
    {
        if (source is null)
        {
            return null;
        }
        Check.IfArgumentNotNull(destination);

        return source switch
        {
            //IConvertible <TSource, TDestination> convertible => (TDestination)convertible,
            IConvertible<TDestination> convertible => convertible.Convert(),
            _ => toDst(source, destination)
        };

        static TDestination toDst(TSource source, TDestination destination)
        {
            var props = typeof(TDestination).GetProperties();
            var result = destination;
            foreach (var prop in props)
            {
                Copy(source, result, prop);
            }

            return result;
        }
    }

    public TDestination Map<TDestination>(in object source) where TDestination : class, new()
        => this.Map(source, new TDestination())!;

    public TDestination? MapExcept<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> except)
                        where TDestination : class
    {
        if (source is null)
        {
            return null;
        }
        Check.IfArgumentNotNull(source);
        var exceptProps = except(destination).GetType().GetProperties().Select(x => x.Name).ToArray();
        var props = typeof(TDestination).GetProperties();
        var result = destination;
        foreach (var prop in props)
        {
            if (!exceptProps.Contains(prop.Name))
            {
                Copy(source, result, prop);
            }
        }

        return result;
    }

    public TDestination MapExcept<TSource, TDestination>(in TSource source, in Func<TDestination, object> except)
        where TDestination : class, new()
    {
        Check.IfArgumentNotNull(source);
        var destination = new TDestination();

        var exceptProps = except(destination).GetType().GetProperties().Select(x => x.Name).ToArray();
        var props = typeof(TDestination).GetProperties();
        var result = destination;
        foreach (var prop in props)
        {
            if (!exceptProps.Contains(prop.Name))
            {
                Copy(source, result, prop);
            }
        }

        return result;
    }

    public TDestination? MapOnly<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> onlyProps)
        where TDestination : class
    {
        if (source is null)
        {
            return null;
        }
        Check.IfArgumentNotNull(destination);

        var justProps = onlyProps(destination).GetType().GetProperties().Select(x => x.Name).ToArray();
        var dstProps = typeof(TDestination).GetProperties();
        var result = destination;
        foreach (var prop in dstProps)
        {
            if (justProps.Contains(prop.Name))
            {
                Copy(source, result, prop);
            }
        }

        return result;
    }

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
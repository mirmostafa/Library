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
    public TDestination Map<TDestination>(in object source, in TDestination destination)
        where TDestination : class
    {
        if (source == null)
        {
            return null;
        }

        Check.MustBeArgumentNotNull(destination);
        var props = typeof(TDestination).GetProperties();
        var result = destination;
        foreach (var prop in props)
        {
            Copy(source, result, prop);
        }

        return result;
    }

    public TDestination? Map<TDestination>(in IConvertible<TDestination> source)
        where TDestination : class
        => source?.Convert();

    public TDestination Map<TDestination>(in object source) where TDestination : class, new()
        => this.Map(source, new TDestination())!;

    public TDestination Map<TSelf, TDestination, TMapper>(TSelf source)
        where TMapper : IMappable<TSelf, TDestination>, new()
        where TDestination : class, new()
    {
        var mapper = new TMapper();
        var converter = new ConvertMapperToConverter<TSelf, TDestination, TMapper>(source, mapper);
        return this.Map<TDestination>(converter);
    }

    public TDestination? Map<TSource, TDestination>(in TSource? source, in TDestination destination)
        where TDestination : class => this.MapExcept<TDestination>(source, destination, default!);

    public TDestination? MapExcept<TDestination>(in object? source, in TDestination destination, in Func<TDestination, object>? except)
        where TDestination : class
    {
        if (source is null)
        {
            return null;
        }
        Check.MustBeArgumentNotNull(source);
        var exceptProps = (except?.Invoke(destination).GetType().GetProperties().Select(x => x.Name) ?? Enumerable.Empty<object>()).ToArray();
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

    public TDestination MapExcept<TDestination>(in object source, in Func<TDestination, object> except)
        where TDestination : class, new()
    {
        Check.MustBeArgumentNotNull(source);
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

    public TDestination MapExcept<TSource, TDestination>(in TSource source, in Func<TDestination, object> except) where TDestination : class, new() => throw new NotImplementedException();

    public TDestination? MapExcept<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> except) where TDestination : class => throw new NotImplementedException();

    public TDestination? MapOnly<TDestination>(in object source, in TDestination destination, in Func<TDestination, object> onlyProps)
                where TDestination : class
    {
        if (source is null)
        {
            return null;
        }
        Check.MustBeArgumentNotNull(destination);

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

    public TDestination? MapOnly<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> onlyProps) where TDestination : class => throw new NotImplementedException();

    internal static void Copy<TDestination>(object source, TDestination destination, PropertyInfo dstProp)
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
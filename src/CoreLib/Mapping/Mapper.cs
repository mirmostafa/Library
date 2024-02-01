using System.Diagnostics;
using System.Reflection;

using Library.DesignPatterns.Markers;
using Library.Interfaces;
using Library.Validations;

namespace Library.Mapping;

[DebuggerStepThrough]
[StackTraceHidden]
[Stateless]
public sealed class Mapper : IMapper
{
    private static readonly HashSet<CustomMapper> _customMappers = [];

    public IMapper ConfigureMapFor<TSource, TDestination>(Func<TSource, TDestination> customMapper)
    {
        _ = _customMappers.Add(CustomMapper.New(customMapper));
        return this;
    }

    public IMapper ConfigureMapFor<TSource, TDestination>(Func<TDestination> customMapper)
    {
        _ = _customMappers.Add(CustomMapper.New<TSource, TDestination>(customMapper));
        return this;
    }

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

    [return: NotNullIfNotNull(nameof(source))]
    public TDestination? Map<TDestination>(in IConvertible<TDestination> source)
        where TDestination : class
        => source?.Convert();

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

    [return: NotNull]
    public TDestination Map<TDestination>(in object source)
    {
        var destinationType = typeof(TDestination);
        var customMapper = FindCustomMapper(source.GetType(), destinationType, 1);
        if (customMapper != null)
        {
            return (TDestination)customMapper.Map.DynamicInvoke(source)!;
        }
        var ctor = destinationType.GetConstructor([]);
        if (ctor != null)
        {
            var destination = (TDestination)ctor.Invoke([]);
            return Copy(source, destination)!;
        }
        throw new NotSupportedException();
    }

    [return: NotNullIfNotNull(nameof(destination))]
    public TDestination? Map<TDestination>(object source, in TDestination destination)
    {
        var customMapper = FindCustomMapper(source.GetType(), typeof(TDestination), 2);
        return customMapper != null
            ? (TDestination?)customMapper.Map.DynamicInvoke(source, destination)
            : Copy(source, destination);
    }

    public TDestination Map<TDestination>(in object source, Func<TDestination> getDestination)
        => Copy(source, getDestination());

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
        Check.MustBeArgumentNotNull(onlyProps);

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
    {
        var mapping = dstProp.GetCustomAttribute<PropertyMappingAttribute>()?.MapFrom;
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

    private static TDestination Copy<TDestination>(object source, TDestination destination)
    {
        var destProps = typeof(TDestination).GetProperties().Compact();
        foreach (var prop in destProps)
        {
            Copy<TDestination>(source, destination, prop);
        }
        return destination;
    }

    private static CustomMapper? FindCustomMapper(Type sourceType, Type destinationType, int paramsCount)
            => _customMappers.FirstOrDefault(x => x == (sourceType, destinationType, paramsCount));
}

internal sealed class ConvertMapperToConverter<TSelf, TDestination, TMapper>(TSelf self, [DisallowNull] TMapper mapper) : IConvertible<TDestination?>
    where TMapper : IMappable<TSelf, TDestination>
{
    public TDestination? Convert()
        => mapper.Map(self);
}
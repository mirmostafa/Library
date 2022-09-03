using System.Collections;

namespace Library.Mapping;

public interface IMapper
{
    IEnumerable<TDestination?> Map<TDestination>(IEnumerable? sources) where TDestination : class, new();

    TDestination Map<TDestination>(in object source) where TDestination : class?, new();

    TDestination Map<TDestination>(in object source, Func<TDestination> instantiator) where TDestination : class;

    IEnumerable<TDestination?> Map<TSource, TDestination>(IEnumerable<TSource> sources, Action<TSource, TDestination> finalize) where TDestination : class, new();

    TDestination? Map<TSource, TDestination>(in TSource source, in TDestination destination) where TDestination : class?;

    TDestination MapExcept<TSource, TDestination>(in TSource source, in Func<TDestination, object> except) where TDestination : class, new();

    TDestination? MapExcept<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> except) where TDestination : class;

    TDestination? MapOnly<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> onlyProps) where TDestination : class;
}
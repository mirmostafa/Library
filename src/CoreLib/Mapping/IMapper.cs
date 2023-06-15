using Library.Extensions.Options;
using Library.Interfaces;

namespace Library.Mapping;

public interface IMappable<in TSource, out TDestination>
{
    TDestination? Map(TSource? self);
}

public interface IMapper
{
    TDestination Map<TDestination>(in object source) where TDestination : class, new();

    TDestination? Map<TSource, TDestination>(in TSource source, in TDestination destination) where TDestination : class;

    [return: MaybeNull]
    TDestination Map<TDestination>(in object source, in TDestination destination) where TDestination : class;

    TDestination Map<TSelf, TDestination, TMapper>(TSelf source) where TMapper : IMappable<TSelf, TDestination>, new() where TDestination : class, new();

    public TDestination? Map<TDestination>(in IConvertible<TDestination> source) where TDestination : class;

    TDestination MapExcept<TSource, TDestination>(in TSource source, in Func<TDestination, object> except) where TDestination : class, new();

    TDestination? MapExcept<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> except) where TDestination : class;

    TDestination? MapExcept<TDestination>(in object source, in TDestination destination, in Func<TDestination, object> except) where TDestination : class;

    TDestination MapExcept<TDestination>(in object source, in Func<TDestination, object> except) where TDestination : class, new();

    TDestination? MapOnly<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> onlyProps) where TDestination : class;

    TDestination? MapOnly<TDestination>(in object source, in TDestination destination, in Func<TDestination, object> onlyProps) where TDestination : class;
}

internal sealed class ConvertMapperToConverter<TSelf, TDestination, TMapper>(TSelf self, [DisallowNull] TMapper mapper) : IConvertible<TDestination?>
    where TMapper : IMappable<TSelf, TDestination>
{
    public TDestination? Convert()
        => mapper.Map(self);
}

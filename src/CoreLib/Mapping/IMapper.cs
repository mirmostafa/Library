using Library.Interfaces;
using Library.Validations;

namespace Library.Mapping;

public interface IMappable<in TSource, out TDestination>
{
    TDestination? Map(TSource? self);
}

public interface IMapper
{
    static IMapper New()
        => new Mapper();

    IMapper ConfigureMapFor<TSource, TDestination>(Func<TSource, TDestination> mapper);

    IMapper ConfigureMapFor<TSource, TDestination>(Func<TDestination> customMapper);

    TDestination Map<TDestination>(in object source);

    [return: NotNullIfNotNull(nameof(source))]
    TDestination? Map<TSource, TDestination>(in TSource source, in TDestination destination) where TDestination : class;

    [return: MaybeNull]
    TDestination Map<TDestination>(in object source, in TDestination destination) where TDestination : class;

    TDestination Map<TSelf, TDestination, TMapper>(TSelf source) where TMapper : IMappable<TSelf, TDestination>, new() where TDestination : class, new();

    public TDestination? Map<TDestination>(in IConvertible<TDestination> source) where TDestination : class;

    TDestination Map<TDestination>(in object source, Func<TDestination> getDestination);

    TDestination Map<TSource, TDestination>(in TSource source, Func<TSource, TDestination> getDestination)
        => getDestination.ArgumentNotNull()(source);

    TDestination Map<TDestination>(in object source, Func<object, TDestination> getDestination)
        => this.Map<object, TDestination>(source, getDestination);

    [return: NotNullIfNotNull(nameof(destination))]
    TDestination? Map<TDestination>(object source, in TDestination destination);

    TDestination MapExcept<TSource, TDestination>(in TSource source, in Func<TDestination, object> except) where TDestination : class, new();

    TDestination? MapExcept<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> except) where TDestination : class;

    TDestination? MapExcept<TDestination>(in object source, in TDestination destination, in Func<TDestination, object> except) where TDestination : class;

    TDestination MapExcept<TDestination>(in object source, in Func<TDestination, object> except) where TDestination : class, new();

    TDestination? MapOnly<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> onlyProps) where TDestination : class;

    TDestination? MapOnly<TDestination>(in object source, in TDestination destination, in Func<TDestination, object> onlyProps) where TDestination : class;
}
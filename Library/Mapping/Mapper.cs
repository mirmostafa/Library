﻿using System.Collections;

using static Library.Mapping.MapperExtensions;

namespace Library.Mapping
{
    public sealed class Mapper : IMapper
    {
        public TDestination Map<TSource, TDestination>(in TSource source, in TDestination destination)
            where TDestination : class
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (source is null)
            {
                return null;
            }

            var props = typeof(TDestination).GetProperties();
            var result = destination;
            foreach (var prop in props)
            {
                Copy(source, result, prop);
            }

            return result;
        }

        public TDestination? MapExcept<TSource, TDestination>(in TSource source, in TDestination destination, in Func<TDestination, object> except)
            where TDestination : class
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (source is null)
            {
                return null;
            }

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

        public TDestination? MapExcept<TSource, TDestination>(in TSource source, in Func<TDestination, object> except)
            where TDestination : class, new()
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

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
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (source is null)
            {
                return null;
            }

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

        public TDestination Map<TDestination>(in object source) where TDestination : class, new()
            => this.Map(source, new TDestination());

        public TDestination MapExcept<TDestination>(in object source, in Func<TDestination, object> except)
            where TDestination : class, new()
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

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

        public IEnumerable<TDestination?> Map<TDestination>(IEnumerable? sources)
            where TDestination : class, new()
        {
            if (sources is null)
            {
                foreach (var item in Enumerable.Empty<TDestination>())
                {
                    yield return item;
                }
            }
            else
            {
                var dstProps = typeof(TDestination).GetProperties();
                foreach (var source in sources)
                {
                    if (source is null)
                    {
                        yield return null;
                    }

                    var result = new TDestination();
                    foreach (var prop in dstProps)
                    {
                        Copy(source, result, prop);
                    }

                    yield return result;
                }
            }
        }

        public IEnumerable<TDestination?> Map<TSource, TDestination>(IEnumerable<TSource> sources, Action<TSource, TDestination> finalize)
            where TDestination : class, new()
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            var dstProps = typeof(TDestination).GetProperties();
            foreach (var source in sources)
            {
                if (source is null)
                {
                    yield return null;
                }

                var result = new TDestination();
                foreach (var prop in dstProps)
                {
                    Copy(source, result, prop);
                    finalize(source, result);
                }

                yield return result;
            }
        }

        public TDestination Map<TDestination>(in object source, Func<TDestination> instantiator)
            where TDestination : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (instantiator is null)
            {
                throw new ArgumentNullException(nameof(instantiator));
            }

            var result = instantiator();
            var dstProps = typeof(TDestination).GetProperties();
            foreach (var prop in dstProps)
            {
                Copy(source, result, prop);
            }

            return result;
        }
    }
}

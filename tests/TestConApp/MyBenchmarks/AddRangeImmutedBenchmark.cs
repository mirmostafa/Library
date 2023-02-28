using BenchmarkDotNet.Attributes;

namespace ConAppTest.MyBenchmarks;

[MemoryDiagnoser(false)]
public class AddRangeImmutedBenchmark
{
    public static IEnumerable<T> AddRangeImmutedContact<T>(IEnumerable<T>? source, IEnumerable<T>? items)
        => (source, items) switch
        {
            (null, null) => Enumerable.Empty<T>(),
            (_, null) => source,
            (null, _) => items,
            (_, _) => source.Concat(items)
        };

    public static IEnumerable<T> AddRangeImmutedForEach<T>(IEnumerable<T>? source, IEnumerable<T>? items)
    {
        if (source != null)
        {
            foreach (var item in source)
            {
                yield return item;
            }
        }

        if (items != null)
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<T> AddRangeImmutedSmart<T>(IEnumerable<T>? source, IEnumerable<T>? items)
    {
        return (source, items) switch
        {
            (null, null) => Enumerable.Empty<T>(),
            (_, null) => source,
            (null, _) => items,
            (_, _) => addRangeImmutedIterator(source, items)
        };
        static IEnumerable<T> addRangeImmutedIterator(IEnumerable<T> source, IEnumerable<T> items)
        {
            foreach (var item in source)
            {
                yield return item;
            }
            foreach (var item in items)
            {
                yield return item;
            }
        }
    }

    [Benchmark]
    public void _00_AddRangeImmutedContactFullTest()
    {
        var a = Enumerable.Range(1, 10 ^ 10);
        var b = Enumerable.Range(1, 10 ^ 10);
        _ = AddRangeImmutedContact(a, b).ToList();
    }

    [Benchmark]
    public void _00_AddRangeImmutedForEachFullTest()
    {
        var a = Enumerable.Range(1, 10 ^ 10);
        var b = Enumerable.Range(1, 10 ^ 10);
        _ = AddRangeImmutedForEach(a, b).ToList();
    }

    [Benchmark]
    public void _00_AddRangeImmutedSmartFullTest()
    {
        var a = Enumerable.Range(1, 10 ^ 10);
        var b = Enumerable.Range(1, 10 ^ 10);
        _ = AddRangeImmutedSmart(a, b).ToList();
    }

    [Benchmark]
    public void _01_AddRangeImmutedContactNullTest()
    {
        var a = Enumerable.Range(1, 10 ^ 10);
        int[]? b = null;
        _ = AddRangeImmutedContact(a, b).ToList();
    }

    [Benchmark]
    public void _01_AddRangeImmutedForEachNullTest()
    {
        var a = Enumerable.Range(1, 10 ^ 10);
        int[]? b = null;
        _ = AddRangeImmutedForEach(a, b).ToList();
    }

    [Benchmark]
    public void _01_AddRangeImmutedSmartNullTest()
    {
        var a = Enumerable.Range(1, 10 ^ 10);
        int[]? b = null;
        _ = AddRangeImmutedSmart(a, b).ToList();
    }
}
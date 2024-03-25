using BenchmarkDotNet.Attributes;

using Library.Helpers;

namespace ConAppTest.MyBenchmarks;

[MemoryDiagnoser(false)]
public sealed class RangeEnumeratorBenchmarks: IBenchmark<RangeEnumeratorBenchmarks>
{
    [Params(10, 1000, 1_000_000)]
    public int _size = 5;

    [Benchmark]
    public void NormalForEach()
    {
        var sum = 0;
        var range = Enumerable.Range(0, this._size);
        foreach (var item in range)
        {
            sum++;
        }
    }

    [Benchmark]
    public void RangeForEach()
    {
        var sum = 0;
        foreach (var item in 0.._size)
        {
            sum++;
        }
    }
}
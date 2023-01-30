using BenchmarkDotNet.Attributes;

using Library.Helpers;

namespace ConAppTest.MyBenchmarks;

[MemoryDiagnoser(false)]
public class FastListBenchmark : IBenchmark<FastListBenchmark>
{
    [Obsolete]
    private FastList<int>? _fastList;
    private List<int>? _list;

    [Benchmark]
    [Obsolete]
    public void FastListAdd()
    {
        int buffer;
        foreach (var item in _fastList)
        {
            buffer = item;
        }
    }

    [Benchmark]
    public void ListAdd()
    {
        int buffer;
        foreach (var item in _list)
        {
            buffer = item;
        }
    }

    [GlobalSetup]
    [Obsolete]
    public void Setup()
    {
        this._fastList = FastList<int>.New(0);
        _ = Enumerable.Range(1, 10 ^ 5).ForEach(x => this._fastList.Add(x)).Build();

        this._list = new List<int>();
        _ = Enumerable.Range(1, 10 ^ 5).ForEach(this._list.Add).Build();
    }
}
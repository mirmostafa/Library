using BenchmarkDotNet.Attributes;

using Library.Collections;
using Library.Helpers;

namespace ConAppTest.MyBenchmarks;

[MemoryDiagnoser(false)]
public class FastListBenchmark
{
    private FastList<int> _fastList;
    private List<int> _list;

    [GlobalSetup]
    public void Setup()
    {
        _fastList = FastList<int>.New(0);
        _ = Enumerable.Range(1, 10 ^ 5).ForEach(x => _fastList.Add(x)).Build();

        _list = new List<int>();
        _ = Enumerable.Range(1, 10 ^ 5).ForEach(_list.Add).Build();
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

    [Benchmark]
    public void FastListAdd()
    {
        int buffer;
        foreach (var item in _fastList)
        {
            buffer = item;
        }
    }

}
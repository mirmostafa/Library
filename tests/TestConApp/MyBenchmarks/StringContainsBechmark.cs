using BenchmarkDotNet.Attributes;

namespace TestConApp.MyBenchmarks;

[MemoryDiagnoser(false)]
public class ContainsBenchmark : IBenchmark<ContainsBenchmark>
{
    private IEnumerable<string>? _array;
    private string? _searchString;

    [Params(true, false)]
    public bool IgnoreCase { get; set; }

    [GlobalSetup]
    public void _Setup()
    {
        this._array = Enumerable.Range(1, 1000).Select(x => x.ToString());
        this._searchString = "500";
    }

    [Benchmark]
    public bool Contains()
        => this._array!.Contains(this._searchString!, this.IgnoreCase);
}
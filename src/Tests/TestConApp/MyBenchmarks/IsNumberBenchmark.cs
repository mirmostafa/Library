using BenchmarkDotNet.Attributes;

namespace ConAppTest.MyBenchmarks;

[MemoryDiagnoser(false)]
public class IsNumberBenchmark : IBenchmark<IsNumberBenchmark>
{
    private readonly string _text = "This is not a number";

    [Benchmark]
    public bool IsNumberByLinq() //! Winner
        => this._text.All(char.IsNumber);

    [Benchmark]
    public bool IsNumberByTryParse()
        => double.TryParse(this._text, out _);
}
using System.Diagnostics;

using BenchmarkDotNet.Attributes;

using Library.Coding;

namespace ConAppTest.MyBenchmarks;

[MemoryDiagnoser(false)]
public sealed class StopwatchBenchmarks : IBenchmark<StopwatchBenchmarks>
{
    [Benchmark]
    public TimeSpan MyWay()
        => LibStopwatch.StartNew().Elapsed;

    //! in .NET 8.0
    //x [Benchmark]
    //x public TimeSpan New()
    //x     => Stopwatch.GetElapsedTime(Stopwatch.GetTimestamp());

    [Benchmark]
    public TimeSpan Old()
        => Stopwatch.StartNew().Elapsed;
}
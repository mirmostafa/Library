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

    [Benchmark]
    public TimeSpan New()
        => Stopwatch.GetElapsedTime(Stopwatch.GetTimestamp());

    [Benchmark]
    public TimeSpan Old()
        => Stopwatch.StartNew().Elapsed;
}
using BenchmarkDotNet.Running;

using ConAppTest.MyBenchmarks;

namespace ConAppTest;

internal partial class Program
{
    private static void Main(string[] args) => BenchmarkRunner.Run<StopwatchBenchmarks>();
}
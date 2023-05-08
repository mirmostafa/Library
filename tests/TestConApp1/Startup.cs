using BenchmarkDotNet.Running;

using ConAppTest.MyBenchmarks;

internal class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<StringSeparateByAiBenchmark>();
    }
}
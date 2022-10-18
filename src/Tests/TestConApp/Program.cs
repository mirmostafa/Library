using BenchmarkDotNet.Running;

using ConAppTest.MyBenchmarks;

using Library.Helpers;
using Library.MultistepProgress;

internal partial class Program
{
    private static async Task Main()
    {
        BenchmarkRunner.Run<RangeEnumeratorBenchmarks>();
        //RangeEnumeratorBenchmarks instance = new();
        //instance.RangeForEach();
    }
}
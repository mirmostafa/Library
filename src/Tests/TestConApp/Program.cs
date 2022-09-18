using BenchmarkDotNet.Running;

using ConAppTest.MyBenchmarks;

using Library.Helpers;

internal class Program
{
    private static void Main()
    {
        BenchmarkRunner.Run<IsNullOrEmptyBenchmarks>();
        //Console.WriteLine(FactorialBenchmarks.Instance.MyConventional2());
    }
}
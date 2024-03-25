using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace ConAppTest.MyBenchmarks;

[MemoryDiagnoser(false)]
public sealed class AwaitRefBenchmark
{
    private static readonly Task<string> MyTask = Task.FromResult("");

    [Benchmark]
    public async Task<string> Task_Await()
    {
        return await Task_AwaitInner();
    }

    private async Task<string> Task_AwaitInner()
    {
        return await MyTask;
    }

    [Benchmark]
    public async Task<string> Task_NoAwait()
    {
        return await Task_AwaitInner();
    }

    private Task<string> Task_NoAwaitInner()
    {
        return MyTask;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace ConAppTest.MyBenchmarks;

[MemoryDiagnoser(false)]
public class AwaitValBenchmark
{
    private static readonly Task<int> MyTask = Task.FromResult(1);

    [Benchmark]
    public async Task<int> Task_Await()
    {
        return await Task_AwaitInner();
    }

    private async Task<int> Task_AwaitInner()
    {
        return await MyTask;
    }

    [Benchmark]
    public async Task<int> Task_NoAwait()
    {
        return await Task_AwaitInner();
    }

    private Task<int> Task_NoAwaitInner()
    {
        return MyTask;
    }
}

﻿using BenchmarkDotNet.Running;

namespace ConAppTest.MyBenchmarks;

public interface IBenchmark<TBenchmark>
    where TBenchmark : new()
{
    public static TBenchmark GetInstance()
        => new TBenchmark();

    public static void Run()
        => BenchmarkRunner.Run<TBenchmark>();
}
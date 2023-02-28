using BenchmarkDotNet.Running;

using ConAppTest.MyBenchmarks;

BenchmarkRunner.Run<AddRangeImmutedBenchmark>();
WriteLine("Ready.");
using System.Numerics;
using System.Runtime.InteropServices;

using BenchmarkDotNet.Attributes;

namespace ConAppTest.MyBenchmarks;

[MemoryDiagnoser(false)]
public sealed class FactorialBenchmarks : IBenchmark<FactorialBenchmarks>
{
    [Params(40, 4_000)]
    public int Number { get; set; }

    [Benchmark]
    public BigInteger _Exam1()
    {
        return Range(this.Number).Aggregate(new BigInteger(1), (p, item) => p * item);

        static IEnumerable<BigInteger> Range(BigInteger source)
        {
            for (BigInteger i = 1; i < source; i++)
            {
                yield return i;
            }
        }
    }

    [Benchmark]
    public BigInteger AsParallel()
    {
        BigInteger result = 1;

        foreach (var item in Enumerable.Range(1, this.Number).AsParallel())
        {
            result *= item;
        }
        return result;
    }

    [Benchmark]
    public BigInteger Conventional()
    {
        BigInteger result = 1;
        for (var i = 1; i < this.Number + 1; i++)
        {
            result *= i;
        }
        return result;
    }

    [Benchmark]
    public BigInteger EmbededAsParallel()
    {
        BigInteger result = 1;

        foreach (var item in Range(1, this.Number).AsParallel())
        {
            result *= item;
        }
        return result;

        static IEnumerable<int> Range(int start, int count)
        {
            var max = start + count - 1;
            var current = start;
            while (unchecked(++current) < max)
            {
                yield return current;
            }
        }
    }

    [Benchmark]
    public BigInteger EmbededForEachToListAsParallel()
    {
        BigInteger result = 1;

        foreach (var item in Range(1, this.Number).ToList().AsParallel())
        {
            result *= item;
        }
        return result;

        static IEnumerable<int> Range(int start, int count)
        {
            var max = start + count - 1;
            var current = start;
            while (unchecked(++current) < max)
            {
                yield return current;
            }
        }
    }

    [Benchmark]
    public BigInteger ForEachAsParallel()
    {
        BigInteger result = 1;

        foreach (var item in Enumerable.Range(1, this.Number).AsParallel())
        {
            result *= item;
        }
        return result;
    }

    [Benchmark]
    public BigInteger ForEachParallelTest() // 🏆 Winner
    {
        BigInteger result = 1;
        _ = Parallel.ForEach(Enumerable.Range(1, this.Number), x => result *= x);
        return result;
    }

    [Benchmark]
    public BigInteger ForEachSpan()
    {
        BigInteger result = 1;

        foreach (var item in CollectionsMarshal.AsSpan(Enumerable.Range(1, this.Number).ToList()))
        {
            result *= item;
        }
        return result;
    }

    [Benchmark]
    public BigInteger ForToArray()
    {
        BigInteger result = 1;

        var data = Enumerable.Range(1, this.Number).ToArray();

        for (var i = 0; i < data.Length; i++)
        {
            result *= data[i];
        }
        return result;
    }

    [Benchmark]
    public BigInteger ForToList()
    {
        BigInteger result = 1;

        var data = Enumerable.Range(1, this.Number).ToList();

        for (var i = 0; i < data.Count; i++)
        {
            result *= data[i];
        }
        return result;
    }

    [Benchmark]
    public BigInteger ForToListAsSpan()
    {
        BigInteger result = 1;

        var data = CollectionsMarshal.AsSpan(Enumerable.Range(1, this.Number).ToList());

        for (var i = 0; i < data.Length; i++)
        {
            result *= data[i];
        }
        return result;
    }

    //[Benchmark]
    public BigInteger Recursive()
    {
        return Factorial(this.Number);
        static BigInteger Factorial(in long num) => num <= 1 ? num : num * Factorial(num - 1);
    }

    //[Benchmark]
    public BigInteger Stirling()
    {
        var n = this.Number;
        if (n == 1)
        {
            return 1;
        }

        double z;
        var e = 2.71; // value of natural e

        z = Math.Sqrt(2 * 3.14 * n) *
            Math.Pow(n / e, n);
        return (BigInteger)z;
    }

    [Benchmark]
    public BigInteger ToArrayAsParallel()
    {
        BigInteger result = 1;

        foreach (var item in Enumerable.Range(1, this.Number).ToArray().AsParallel())
        {
            result *= item;
        }
        return result;
    }

    [Benchmark]
    public BigInteger ToListAsParallel()
    {
        BigInteger result = 1;

        foreach (var item in Enumerable.Range(1, this.Number).ToList().AsParallel())
        {
            result *= item;
        }
        return result;
    }
}
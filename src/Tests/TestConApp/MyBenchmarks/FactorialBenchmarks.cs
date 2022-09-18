using System.Numerics;

using BenchmarkDotNet.Attributes;

namespace ConAppTest.MyBenchmarks;

[MemoryDiagnoser(false)]
public class FactorialBenchmarks
{
    public static readonly FactorialBenchmarks Instance = new();
    private readonly int number = 40;

    //[Benchmark]
    public BigInteger Conventional()
    {
        BigInteger result = 1;
        for (var i = 1; i < this.number + 1; i++)
        {
            result *= i;
        }
        return result;
    }

    [Benchmark]
    public BigInteger MyConventional1() //! 🏆 Winner
    {
        BigInteger result = 1;

        foreach (var item in Enumerable.Range(1, this.number).AsParallel())
        {
            result *= item;
        }
        return result;
    }

    [Benchmark]
    public BigInteger MyConventional2()
    {
        BigInteger result = 1;

        foreach (var item in Range(1, this.number).AsParallel())
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
    public BigInteger MyConventional3()
    {
        BigInteger result = 1;

        foreach (var item in Enumerable.Range(1, this.number).ToList().AsParallel())
        {
            result *= item;
        }
        return result;
    }

    [Benchmark]
    public BigInteger MyConventional4()
    {
        BigInteger result = 1;

        foreach (var item in Range(1, this.number).ToList().AsParallel())
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

    //[Benchmark]
    public BigInteger Recursive()
    {
        return Factorial(this.number);
        static BigInteger Factorial(in long num) => num <= 1 ? num : num * Factorial(num - 1);
    }

    //[Benchmark]
    public BigInteger Stirling()
    {
        var n = this.number;
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
}
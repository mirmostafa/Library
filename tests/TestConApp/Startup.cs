Fibonacci(2000).Distinct().WriteLine();

static IEnumerable<long> Fibonacci(long count)
{
    return FibonacciRecursiveIterator(count, 0L, 1L);

    static IEnumerable<long> FibonacciRecursiveIterator(long count, long a, long b)
    {
        if (count <= 0)
        {
            yield break;
        }

        yield return a;

        foreach (var number in FibonacciRecursiveIterator(count - 1, b, a + b))
        {
            yield return number;
        }
    }
}
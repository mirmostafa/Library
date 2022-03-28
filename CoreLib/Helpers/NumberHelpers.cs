using System.Collections.Immutable;
using Library.Globalization;

namespace Library.Helpers;
public static class NumberHelper
{
    public static int Add(in int x, in int y)
    {
        return x + y;
    }

    public static bool IsBetween(this int num, in int min, in int max)
    {
        return num > min && num <= max;
    }

    public static bool IsPrime(int number)
    {
        return Enumerable.Range(2, Math.Sqrt(number).ToInt() - 1).All(d => number % d != 0);
    }

    public static string ToPersian(this int number)
    {
        var persianNumbers = PersianTools.PersianDigits.Select(x => x.ToString()).ToImmutableArray();
        var result = "";
        while (number > 0)
        {
            result = persianNumbers[number % 10] + result;
            number /= 10;
        }
        return result;
    }

    public static string ToString(this int? number, string format = "0", int defaultValue = 0)
    {
        return (number ?? defaultValue).ToString(format);
    }
}
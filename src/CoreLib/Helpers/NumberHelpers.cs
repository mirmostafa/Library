using System.Collections.Immutable;

using Library.Globalization;

namespace Library.Helpers;

public static class NumberHelper
{
    private static readonly string[] _sizeSuffixes = { "", "K", "M", "G", "T", "P", "E", "Z", "Y" };


    public static int RandomNumber(int? seed = null)
    {
        var rnd = seed is null ? new Random() : new Random(seed.Value);
        return rnd.Next();
    }

    public static IEnumerable<int> RandomNumbers(int count, int? seed = null)
    {
        var rnd = seed is null ? new Random() : new Random(seed.Value);
        return Enumerable.Range(0, count).Select(_ => rnd.Next());
    }

    public static bool IsBetween(this int num, in int min, in int max) 
        => num > min && num <= max;

    public static bool IsPrime(int number)
        => Enumerable.Range(2, Math.Sqrt(number).Cast().ToInt() - 1).All(d => number % d != 0);

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

    public static string ToStandardMetricScale(this long value, int measure = 1000, int decimalPlaces = 1)
    {
        if (value < 0)
        {
            return "-" + ToStandardMetricScale(-value, measure, decimalPlaces);
        }
        if (measure == 1)
        {
            return value.ToString();
        }
        var i = 0;
        decimal dValue = value;
        while (Math.Round(dValue, decimalPlaces) >= measure)
        {
            dValue /= measure;
            i++;
        }

        return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, _sizeSuffixes[i]);
    }

    public static string ToString(this int? number, string format = "0", int defaultValue = 0)
        => (number ?? defaultValue).ToString(format);
}
using System.Globalization;
using System.Text.RegularExpressions;
using Library.Globalization;
using Library.Globalization.Pluralization;
using Library.Results;
using Library.Validations;

namespace Library.Helpers;
//{
/// <summary>
///     A utility to do some common tasks about strings
/// </summary>
public static class StringHelper
{
    public static string? Add(this string? s, in int count, char add = ' ', bool before = false) =>
        count is 0 ? s : before ? s?.PadLeft(s.Length + count, add) : s?.PadRight(s.Length + count, add);

    public static string Add(this string s, in string s1) =>
        string.Concat(s, s1);

    public static IEnumerable<int> AllIndexesOf(this string str, string value, bool ignoreCase = false)
    {
        var buffer = ignoreCase ? str.ArgumentNotNull(nameof(str)).ToLower(CultureInfo.CurrentCulture) : str.ArgumentNotNull(nameof(str));
        var stat = ignoreCase ? value.ArgumentNotNull(nameof(value)).ToLower(CultureInfo.CurrentCulture) : value.ArgumentNotNull(nameof(value));
        var result = 0;
        int currentIndex;
        while ((currentIndex = buffer.IndexOf(stat, StringComparison.Ordinal)) != -1)
        {
            result += currentIndex;
            yield return result;
            result += stat.Length;
            buffer = buffer[(currentIndex + stat.Length)..];
        }
    }

    public static bool AnyCharInString(this string str, in string range)
        => range.Any(c => str.Contains(c.ToString()));

    public static string ArabicCharsToPersian(this string value)
        => value.IsNullOrEmpty() ? value : value.ReplaceAll(PersianTools.InvalidArabicCharPairs.Select(x => (x.Arabic, x.Persian)));

    public static bool CheckAllValidations(in string text, in Func<char, bool> regularValidate)
        => text.All(regularValidate);

    public static bool CheckAnyValidations(in string text, in Func<char, bool> regularValidate)
        => text.Any(regularValidate);

    [return: NotNull]
    public static string[] Compact(params string[] strings) => strings.Where(item
         => !item.IsNullOrEmpty()).ToArray();

    [return: NotNull]
    public static IEnumerable<string> Compact(this IEnumerable<string?>? strings)
        => (strings?.Where(item => !item.IsNullOrEmpty()).Select(s => s!)) ?? Enumerable.Empty<string>();

    public static int CompareTo(this string str1, in string str, bool ignoreCase = false)
        => string.Compare(str1, str, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);

    public static string ConcatStrings(this IEnumerable<string> values) =>
        string.Concat(values.ToArray());

    public static bool Contains(string str, in string value, bool ignoreCase = true)
        => ignoreCase
            ? str.ArgumentNotNull(nameof(str)).ToLowerInvariant().Contains(value.ArgumentNotNull(nameof(value)).ToLowerInvariant())
            : str.ArgumentNotNull(nameof(str)).Contains(value);

    public static bool Contains(this IEnumerable<string> array, string str, bool ignoreCase)
        => array.Any(s => s.CompareTo(str, ignoreCase) == 0);
    public static bool ContainsAny(this string str, in IEnumerable<string?> array)
        => array.Any(item => str.Contains(item));

    public static bool ContainsAny(this string str, params object[] array)
        => str.ContainsAny(array.Select(x => x?.ToString()).AsEnumerable());

    public static bool ContainsOf(this string str, in string target)
        => str.ToLower().Contains(target.ToLower());

    public static string CorrectUnicodeProblem(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        text = text.Replace((char)1740, (char)1610);
        text = text.Replace((char)1705, (char)1603);
        return Encoding.UTF8.GetString(Encoding.Unicode.GetBytes(text));
    }

    public static bool EndsWithAny(this string str, in IEnumerable<object> array)
        => array.Any(item => item is { } s && str.EndsWith(s.ToString() ?? string.Empty));

    public static bool EndsWithAny(this string str, in IEnumerable<string> values)
        => values.Any(str.EndsWith);

    public static bool EndsWithAny(this string str, params object[] array)
        => str.EndsWithAny(array.AsEnumerable());

    public static bool EqualsTo(this string str1, in string str, bool ignoreCase = true)
        => str1.CompareTo(str, ignoreCase) == 0;

    public static bool EqualsToAny(this string str1, bool ignoreCase, params string[] array)
        => array.Any(s => str1.EqualsTo(s, ignoreCase));

    public static bool EqualsToAny(this string str1, params string[] array)
        => array.Any(s => str1.EqualsTo(s));

    public static IEnumerable<(string Key, string Value)> GetKeyValues(this string keyValueStr, char keyValueSeparator = '=', char separator = ';')
        => keyValueStr.Split(separator).Select(raw => raw.Split(keyValueSeparator)).Select(keyValue => (keyValue[0], keyValue[1]));

    public static string? GetLongest(this string[] strings)
        => strings?.Max();

    public static string? GetPhrase(this string str, in int index, in char start, char end = default)
    {
        if (end == default(char))
        {
            end = start;
        }

        var buffer = str;
        int startIndex;
        int endIndex;
        int len;
        (var succeed1, var lenOfStart) = str.TryCountOf(start, index * 2);
        if (!succeed1)
        {
            return null;
        }

        if (start == end)
        {
            startIndex = index != 0 ? lenOfStart + 1 : str.IndexOf(start, 0) + 1;
            endIndex = str.IndexOf(end, startIndex + 1);
            len = endIndex - startIndex;
        }
        else
        {
            startIndex = index != 0 ? lenOfStart + 1 : str.IndexOf(start, 0) + 1;
            (var succeed2, var lenOfEnd) = str.TryCountOf(end, index * 2);
            if (!succeed2)
            {
                return null;
            }

            endIndex = index != 0 ? lenOfEnd + 1 : str.IndexOf(end, 0) + 1;
            len = endIndex - startIndex;
        }

        var result = buffer.Slice(startIndex, len);
        return result;
    }

    public static IEnumerable<string?> GetPhrases(this string str, char start, char end = default)
    {
        var index = 0;
        var result = GetPhrase(str, index, start, end);
        while (!result.IsNullOrEmpty())
        {
            yield return result;
            result = GetPhrase(str, ++index, start, end);
        }
    }

    public static IEnumerable<string> GroupBy(this IEnumerable<char> chars, char separator)
    {
        var buffer = new StringBuilder();
        foreach (var c in chars)
        {
            if (c == separator)
            {
                yield return buffer.ToString();
                _ = buffer.Clear();
            }
            else
            {
                _ = buffer.Append(c);
            }
        }
    }

    public static IEnumerable<string> GroupBy(this string str, char separator)
    {
        var buffer = new StringBuilder();
        foreach (var c in str)
        {
            if (c == separator)
            {
                yield return buffer.ToString();
                _ = buffer.Clear();
            }
            else
            {
                _ = buffer.Append(c);
            }
        }
    }

    public static bool HasPersian(this string text)
        => CheckAnyValidations(text, IsPersian);

    public static string? IfNull(this string? s, in string? value)
        => s is null ? value : s;

    public static string IfNullOrEmpty(this string? str, string defaultValue)
        => str.IsNullOrEmpty() ? defaultValue : str;

    public static int? IndexOf([DisallowNull] this IEnumerable<string?> items, in string? item, bool trimmed = false, bool ignoreCase = true)
    {
        Check.ArgumentNotNull(items, nameof(items));

        var itm = get(item, trimmed);
        var index = 0;
        var enumerator = items.GetEnumerator();

        while (enumerator.MoveNext())
        {
            if (equals(get(enumerator.Current, trimmed), itm, ignoreCase))
            {
                return index;
            }

            index++;
        }

        return null;

        static string? get(string? current, bool trimmed) =>
            trimmed ? current?.Trim() : current;
        static bool equals(string? current, string? item, bool ignoreCase) =>
            string.Compare(current, item, ignoreCase) == 0;
    }

    public static bool IsCommon(this char c) =>
        c == ' ';

    public static bool IsDigit(this char c, in bool canAcceptMinusKey = false) =>
        (canAcceptMinusKey && c == '-') || char.IsDigit(c);

    public static bool IsDigitOrControl(this char key) =>
        char.IsDigit(key) || char.IsControl(key);

    public static bool IsEmpty([NotNullWhen(false)] in string? s) =>
        s?.Length is 0;

    public static bool IsEnglish(this char c)
        => IsCommon(c) || (c is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z'));

    public static bool IsEnglish(this string text)
        => CheckAllValidations(text, IsEnglish);

    public static bool IsEnglishOrNumber(this string text, bool canAcceptMinusKey = false)
        => CheckAllValidations(text, c => IsDigit(c, canAcceptMinusKey) || IsEnglish(c));

    public static bool IsInRange(this string? text, bool ignoreCase, params string[] range)
        => IsInRange(text, false, ignoreCase, range);

    public static bool IsInRange(this string? text, bool trimmed, bool ignoreCase, IEnumerable<string> range)
        => range.IndexOf(text, trimmed, ignoreCase) >= 0;

    public static bool IsInRange(this string? text, bool trimmed, bool ignoreCase, params string[] range)
        => range.IndexOf(text, trimmed, ignoreCase) >= 0;

    public static bool IsInRange(this string? text, params string[] range)
        => IsInRange(text, true, range);

    public static bool IsInString(this char c, in string text)
        => text?.Contains(c.ToString()) ?? false;

    public static bool IsInteger(this string text)
        => int.TryParse(text, out _);

    public static bool IsLetter(this char c)
        => IsEnglish(c) || IsPersian(c);

    public static bool IsLetterText(this string text)
        => CheckAllValidations(text, c => IsEnglish(c) || IsPersian(c));

    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str)
        => string.IsNullOrEmpty(str);

    public static bool IsNumber(this string text)
        => double.TryParse(text, out _);

    public static bool IsPersian(this char c)
        => IsCommon(c) || PersianTools.Chars.Any(pc => pc == c) || PersianTools.SpecialChars.Any(pc => pc == c);

    public static bool IsPersian(this string text)
        => CheckAllValidations(text, IsPersian);

    public static bool IsPersianDigit(this char c)
        => PersianTools.PersianDigits.Any(x => c == x);

    public static bool IsPersianOrNumber(this string text, bool canAcceptMinusKey)
        => CheckAllValidations(text, c => IsDigit(c, canAcceptMinusKey) || IsPersian(c));

    public static bool IsUnicode(this string str) =>
        str.Any(c => c > 255);

    public static int CountOf(this string str, in char c, int index)
    {
        if (index == 0)
        {
            return str.IndexOf(c);
        }

        var foundCount = 0;
        index++;
        var i = 0;
        while (foundCount < index)
        {
            if (str[i] == c)
            {
                foundCount++;
            }

            i++;
        }

        return i - 1;
    }

    public static string Merge(string quatStart, string quatEnd, string separator, params object[] array)
    {
        var result = array.Aggregate(string.Empty, (current, str) => current + quatStart + str + quatEnd + separator + " ").Trim();
        return result[..^1];
    }

    public static string Merge(string quat, string separator, params string[] array) =>
        array.Merge(quat, separator);
    public static string Merge(this IEnumerable<string> array, in string separator) =>
        string.Join(separator, array.ToArray());
    public static string Merge(this IEnumerable<string> array, in char separator) =>
        string.Join(separator, array.ToArray());
    public static string Merge(this IEnumerable<string> array, string quat, string separator) =>
        array.Aggregate(string.Empty, (current, str) => $"{current}{quat}{str}{quat}{separator} ").Trim()[..^1];

    public static string MergePair(this IEnumerable<(string, string)> splitPair, string keyValueSeparator = "=", string statementSeparator = ";") =>
        string.Join(statementSeparator, splitPair.Select(pair => $"{pair.Item1}{keyValueSeparator}{pair.Item2}"));

    public static string PatternPolicy(in string text)
    {
        var result = string.Empty;
        var t = CorrectUnicodeProblem(text);

        char[] outChar =
        {
                '~', '`', '!', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '{', '[', '}', ']', ':', ';', '\'', '"', '<', ',', '>', '?', '|', '\\'
        };

        var arrayString = t.Split(outChar);

        return arrayString.Aggregate(result, (current, s) => current + s);
    }

    /// <summary>
    /// Pluralizes the specified text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    public static string? Pluralize(string? text)
        => text.IsNullOrEmpty() ? null : Pluralizer.Pluralize(text);

    public static string? Remove(this string? s, in string? value)
        => s is null ? null : (value is null ? s : s.Replace(value, ""));

    public static string RemoveFromEnd(this string str, in int count)
        => str.ArgumentNotNull(nameof(str)).Slice(0, str.Length - count);

    public static string RemoveFromEnd(this string str, in string oldValue, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
    {
        str.ArgumentNotNull(nameof(str));
        return oldValue.IsNullOrEmpty()
            ? str
            : str.EndsWith(oldValue, comparison)
                ? str.Slice(0, str.Length - oldValue.Length)
                : str;
    }
    public static string RemoveFromStart(this string str, in string oldValue, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
    {
        str.ArgumentNotNull(nameof(str));
        return oldValue.IsNullOrEmpty()
            ? str
            : str.StartsWith(oldValue, comparison) ? str.Slice(oldValue.Length, str.Length - oldValue.Length) : str;
    }

    public static string Repeat(this string text, in int count)
    {
        Check.IfArgumentBiggerThan(count, 0);
        return string.Concat(Enumerable.Repeat(text, count));
    }

    public static string Replace2(this string s, char old, in char replacement, in int count = 1)
        => Replace2(s, old.ToString(), replacement.ToString(), count);

    public static string Replace2(this string s, in string old, in string replacement, in int count = 1)
        => new Regex(Regex.Escape(old)).Replace(s, replacement, count);

    public static string ReplaceAll(this string value, in IEnumerable<(char OldValue, char NewValue)> items)
        => items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));

    public static string ReplaceAll(this string value, in IEnumerable<string> oldValues, string newValue)
        => oldValues.Aggregate(value, (current, oldValue) => current.Replace(oldValue, newValue));

    public static string ReplaceAll(this string value, in IEnumerable<(string OldValue, string NewValue)> items)
        => items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));

    public static string ReplaceAll(this string value, params (char OldValue, char NewValue)[] items)
        => items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));

    public static string ReplaceAll(this string value, params (string OldValue, string NewValue)[] items)
        => items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));

    public static string SeparateCamelCase(this string? str)
    {
        if (str == null)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        for (var i = 0; i < str.Length; i++)
        {
            if (char.IsUpper(str[i]) && i > 0)
            {
                sb.Append(' ');
            }

            sb.Append(str[i]);
        }

        return sb.ToString();
    }

    public static string? SetPhrase(this string str, int index, string newStr, char start, char end = default)
    {
        var startStr = str.Slice(0, index + 1);
        var phrase = GetPhrase(str, index, start, end);
        if (phrase is null)
        {
            return null;
        }

        var endEnd = str.Slice(index + phrase.Length);
        var result = string.Concat(startStr, newStr, endEnd);
        return result;
    }

    /// <summary>
    /// Singularizes the specified text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    public static string? Singularize(string? text)
        => text.IsNullOrEmpty() ? null : Pluralizer.Singularize(text);

    public static string Space(in int count) =>
        new(' ', count);

    public static IEnumerable<string> Split(this string value, int groupSize)
    {
        Check.IfArgumentNotNull(value, nameof(value));

        for (var x = 0; x < value.Length; x += groupSize)
        {
            yield return value.Slice(x, Math.Min(groupSize, value.Length - x));
        }
    }

    public static IEnumerable<string> SplitCamelCase(this string? value)
    {
        if (value == null)
        {
            yield break;
        }

        var sb = new StringBuilder();
        foreach (var c in value)
        {
            if (char.IsUpper(c))
            {
                if (sb.Length > 0)
                {
                    yield return sb.ToString();
                }

                sb.Clear();
            }
            sb.Append(c);
        }
        if (sb.Length > 0)
        {
            yield return sb.ToString();
        }
    }

    public static string SplitMerge(this string str, char splitter, in string startSeparator, in string endSeparator)
    {
        var result = new StringBuilder();
        foreach (var part in str.Split(splitter))
        {
            _ = result.Append($"{startSeparator}{part}{endSeparator}");
        }

        return result.ToString();
    }

    public static IEnumerable<(string Key, string Value)> SplitPair([DisallowNull] string str, [DisallowNull] string keyValueSeparator = "=", [DisallowNull] string statementSeparator = ";")
    {
        Check.IfArgumentNotNull(str);
        Check.IfArgumentNotNull(keyValueSeparator);
        Check.IfArgumentNotNull(statementSeparator);

        var keyValuePirs = str.Split(statementSeparator).Trim();
        foreach (var keyValuePair in keyValuePirs)
        {
            var keyValue = keyValuePair.Split(keyValueSeparator);
            yield return (keyValue[0], keyValue[1]);
        }
    }

    public static string SqlEncodingToUtf(this string obj)
        => Encoding.UTF8.GetString(Encoding.GetEncoding(1256).GetBytes(obj));

    public static bool StartsWithAny(this string str, in IEnumerable<string> values)
        => values.Any(str.StartsWith);

    public static bool StartsWithAny(this string str, params string[] values)
        => values.Any(str.StartsWith);

    public static byte[] ToBytes([DisallowNull] this string value, [DisallowNull] in Encoding encoding)
    {
        Check.ArgumentNotNull(value);
        Check.ArgumentNotNull(encoding);

        return encoding.GetBytes(value);
    }

    public static string ToCulturalNumber(this string value, in bool correctPersianChars, in Language toLanguage)
    {
        value = toLanguage switch
        {
            Language.Persian => value.ToPersianDigits(),
            Language.English => value.ToEnglishDigits(),
            _ => value
        };

        if (correctPersianChars)
        {
            value = ArabicCharsToPersian(value);
        }

        return value;
    }

    public static string ToPersianDigits(this string value)
        => value.ReplaceAll(PersianTools.Digits.Select(n => (n.English, n.Persian)));

    public static string ToEnglishDigits(this string value)
        => value.ReplaceAll(PersianTools.Digits.Select(n => (n.Persian, n.English)));

    public static string ToHex(in string str)
    {
        var sb = new StringBuilder();
        var bytes = IsUnicode(str) ? Encoding.Unicode.GetBytes(str) : Encoding.ASCII.GetBytes(str);
        foreach (var b in bytes)
        {
            _ = sb.Append(b.ToString("X"));
        }

        return sb.ToString();
    }

    public static IEnumerable<int> ToInt(this IEnumerable<string> array)
        => array.Where(str => str.IsNumber()).Select(str => str.ToInt());

    public static IEnumerable<string> ToLower(this IEnumerable<string> strings)
        => strings.Select(str => str.ToLower());

    public static string ToUnicode(this string str)
        => Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(str));

    public static IEnumerable<string> Trim(this IEnumerable<string> strings)
        => strings.Where(item => !IsNullOrEmpty(item));

    public static IEnumerable<string> TrimAll(this IEnumerable<string> values)
        => values.Select(t => t.Trim());

    public static IEnumerable<string> TrimAll(this IEnumerable<string> values, params char[] trimChars)
        => values.Select(t => t.Trim(trimChars));

    public static string? Truncate(this string? value, in int length)
        => length > value?.Length ? value : value?[..^length];

    public static TryMethodResult<int> TryCountOf(this string str, char c, int index)
    {
        var result = CatchFunc(() => CountOf(str, c, index));
        return new(result.Exception is null, result.Result);
    }

    public static string Slice(this string s, in int start, in int? length = null)
    {
        //Check.IfArgumentNotNull(s, nameof(s));
        ReadOnlySpan<char> span = s;
        var slice = length is { } l ? span.Slice(start, l) : span[start..];
        return new(slice);
    }
    public static bool IsValidIranianNationalCode(string input)
    {
        if (!Regex.IsMatch(input, @"^\d{10}$"))
        {
            return false;
        }

        var check = Convert.ToInt32(input.Slice(9, 1));
        var sum = Enumerable.Range(0, 9)
            .Select(x => Convert.ToInt32(input.Slice(x, 1)) * (10 - x))
            .Sum() % 11;

        return sum < 2 ? check == sum : check + sum == 11;
    }
}
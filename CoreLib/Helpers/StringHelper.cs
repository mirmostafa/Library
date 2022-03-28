using System.Diagnostics.Contracts;
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
    /// <summary>
    /// Adds the specified char to the string.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="count">The count.</param>
    /// <param name="add">The add.</param>
    /// <param name="before">if set to <c>true</c> [before].</param>
    /// <returns></returns>
    [Pure]
    public static string? Add(this string? s, in int count, char add = ' ', bool before = false)
    {
        return count is 0 ? s : before ? s?.PadLeft(s.Length + count, add) : s?.PadRight(s.Length + count, add);
    }

    /// <summary>
    /// Adds the specified string.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="s1">The s1.</param>
    /// <returns></returns>
    [Pure]
    public static string Add(this string s, in string s1)
    {
        return string.Concat(s, s1);
    }

    public static IEnumerable<int> AllIndexesOf(this string str, string value, bool ignoreCase = false)
    {
        string buffer = ignoreCase ? str.ArgumentNotNull(nameof(str)).ToLower(CultureInfo.CurrentCulture) : str.ArgumentNotNull(nameof(str));
        string stat = ignoreCase ? value.ArgumentNotNull(nameof(value)).ToLower(CultureInfo.CurrentCulture) : value.ArgumentNotNull(nameof(value));
        int result = 0;
        int currentIndex;
        while ((currentIndex = buffer.IndexOf(stat, StringComparison.Ordinal)) != -1)
        {
            result += currentIndex;
            yield return result;
            result += stat.Length;
            buffer = buffer[(currentIndex + stat.Length)..];
        }
    }

    [Pure]
    public static bool AnyCharInString(this string str, in string range)
    {
        return range.Any(c => str.Contains(c.ToString()));
    }

    [Pure]
    public static string ArabicCharsToPersian(this string value)
    {
        return value.IsNullOrEmpty() ? value : value.ReplaceAll(PersianTools.InvalidArabicCharPairs.Select(x => (x.Arabic, x.Persian)));
    }

    [Pure]
    public static bool CheckAllValidations(in string text, in Func<char, bool> regularValidate)
    {
        return text.All(regularValidate);
    }

    public static bool CheckAnyValidations(in string text, in Func<char, bool> regularValidate)
    {
        return text.Any(regularValidate);
    }

    [return: NotNull]
    [Pure]
    public static string[] Compact(params string[] strings)
    {
        return strings.Where(item => !item.IsNullOrEmpty()).ToArray();
    }

    [return: NotNull]
    [Pure]
    public static IEnumerable<string> Compact(this IEnumerable<string?>? strings)
    {
        return (strings?.Where(item => !item.IsNullOrEmpty()).Select(s => s!)) ?? Enumerable.Empty<string>();
    }

    [Pure]
    public static int CompareTo(this string str1, in string str, bool ignoreCase = false)
    {
        return string.Compare(str1, str, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
    }

    [Pure]
    public static string ConcatStrings(this IEnumerable<string> values)
    {
        return string.Concat(values.ToArray());
    }

    [Pure]
    public static bool Contains(string str, in string value, bool ignoreCase = true)
    {
        return ignoreCase
                ? str.ArgumentNotNull(nameof(str)).ToLowerInvariant().Contains(value.ArgumentNotNull(nameof(value)).ToLowerInvariant())
                : str.ArgumentNotNull(nameof(str)).Contains(value);
    }

    [Pure]
    public static bool Contains(this IEnumerable<string> array, string str, bool ignoreCase)
    {
        return array.Any(s => s.CompareTo(str, ignoreCase) == 0);
    }

    [Pure]
    public static bool ContainsAny(this string str, in IEnumerable<string?> array)
    {
        return array.Any(str.Contains);
    }

    [Pure]
    public static bool ContainsAny(this string str, params object[] array)
    {
        return str.ContainsAny(array.Select(x => x?.ToString()).AsEnumerable());
    }

    [Pure]
    public static bool ContainsOf(this string str, in string target)
    {
        return str.ToLower().Contains(target.ToLower());
    }

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

    [Pure]
    public static bool EndsWithAny(this string str, in IEnumerable<object> array)
    {
        return array.Any(item => item is { } s && str.EndsWith(s.ToString() ?? string.Empty));
    }

    [Pure]
    public static bool EndsWithAny(this string str, in IEnumerable<string> values)
    {
        return values.Any(str.EndsWith);
    }

    [Pure]
    public static bool EndsWithAny(this string str, params object[] array)
    {
        return str.EndsWithAny(array.AsEnumerable());
    }

    [Pure]
    public static bool EqualsTo(this string str1, in string str, bool ignoreCase = true)
    {
        return str1.CompareTo(str, ignoreCase) == 0;
    }

    [Pure]
    public static bool EqualsToAny(this string str1, bool ignoreCase, params string[] array)
    {
        return array.Any(s => str1.EqualsTo(s, ignoreCase));
    }

    [Pure]
    public static bool EqualsToAny(this string str1, params string[] array)
    {
        return array.Any(s => str1.EqualsTo(s));
    }

    [Pure]
    public static IEnumerable<(string Key, string Value)> GetKeyValues(this string keyValueStr, char keyValueSeparator = '=', char separator = ';')
    {
        return keyValueStr.Split(separator).Select(raw => raw.Split(keyValueSeparator)).Select(keyValue => (keyValue[0], keyValue[1]));
    }

    [Pure]
    public static string? GetLongest(this string[] strings)
    {
        return strings?.Max();
    }

    [Pure]
    public static string? GetPhrase(this string str, in int index, in char start, char end = default)
    {
        if (end == default(char))
        {
            end = start;
        }

        string buffer = str;
        int startIndex;
        int endIndex;
        int len;
        (bool succeed1, int lenOfStart) = str.TryCountOf(start, index * 2);
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
            (bool succeed2, int lenOfEnd) = str.TryCountOf(end, index * 2);
            if (!succeed2)
            {
                return null;
            }

            endIndex = index != 0 ? lenOfEnd : str.IndexOf(end, 0);
            len = endIndex - startIndex;
        }

        string result = buffer.Slice(startIndex, len);
        return result;
    }

    public static IEnumerable<string?> GetPhrases(this string str, char start, char end = default)
    {
        int index = 0;
        string result = GetPhrase(str, index, start, end);
        while (!result.IsNullOrEmpty())
        {
            yield return result;
            result = GetPhrase(str, ++index, start, end);
        }
    }

    [Pure]
    public static IEnumerable<string> GroupBy(this IEnumerable<char> chars, char separator)
    {
        StringBuilder buffer = new();
        foreach (char c in chars)
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

    [Pure]
    public static IEnumerable<string> GroupBy(this string str, char separator)
    {
        StringBuilder buffer = new();
        foreach (char c in str)
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

    [Pure]
    public static bool HasPersian(this string text)
    {
        return CheckAnyValidations(text, IsPersian);
    }

    [Pure]
    public static string? IfNull(this string? s, in string? value)
    {
        return s is null ? value : s;
    }

    [Pure]
    public static string IfNullOrEmpty(this string? str, string defaultValue)
    {
        return str.IsNullOrEmpty() ? defaultValue : str;
    }

    public static int? IndexOf([DisallowNull] this IEnumerable<string?> items, in string? item, bool trimmed = false, bool ignoreCase = true)
    {
        Check.IfArgumentNotNull(items, nameof(items));

        string itm = get(item, trimmed);
        int index = 0;
        IEnumerator<string> enumerator = items.GetEnumerator();

        while (enumerator.MoveNext())
        {
            if (equals(get(enumerator.Current, trimmed), itm, ignoreCase))
            {
                return index;
            }

            index++;
        }

        return null;

        static string? get(string? current, bool trimmed)
        {
            return trimmed ? current?.Trim() : current;
        }

        static bool equals(string? current, string? item, bool ignoreCase)
        {
            return string.Compare(current, item, ignoreCase) == 0;
        }
    }

    [Pure]
    public static bool IsCommon(this char c)
    {
        return c == ' ';
    }

    [Pure]
    public static bool IsDigit(this char c, in bool canAcceptMinusKey = false)
    {
        return (canAcceptMinusKey && c == '-') || char.IsDigit(c);
    }

    [Pure]
    public static bool IsDigitOrControl(this char key)
    {
        return char.IsDigit(key) || char.IsControl(key);
    }

    [Pure]
    public static bool IsEmpty([NotNullWhen(false)] in string? s)
    {
        return s?.Length is 0;
    }

    [Pure]
    public static bool IsEnglish(this char c)
    {
        return IsCommon(c) || (c is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z'));
    }

    public static bool IsEnglish(this string text)
    {
        return CheckAllValidations(text, IsEnglish);
    }

    public static bool IsEnglishOrNumber(this string text, bool canAcceptMinusKey = false)
    {
        return CheckAllValidations(text, c => IsDigit(c, canAcceptMinusKey) || IsEnglish(c));
    }

    public static bool IsInRange(this string? text, bool ignoreCase, params string[] range)
    {
        return IsInRange(text, false, ignoreCase, range);
    }

    public static bool IsInRange(this string? text, bool trimmed, bool ignoreCase, IEnumerable<string> range)
    {
        return range.IndexOf(text, trimmed, ignoreCase) >= 0;
    }

    public static bool IsInRange(this string? text, bool trimmed, bool ignoreCase, params string[] range)
    {
        return range.IndexOf(text, trimmed, ignoreCase) >= 0;
    }

    public static bool IsInRange(this string? text, params string[] range)
    {
        return IsInRange(text, true, range);
    }

    public static bool IsInString(this char c, in string text)
    {
        return text?.Contains(c.ToString()) ?? false;
    }

    public static bool IsInteger(this string text)
    {
        return int.TryParse(text, out _);
    }

    public static bool IsLetter(this char c)
    {
        return IsEnglish(c) || IsPersian(c);
    }

    public static bool IsLetterText(this string text)
    {
        return CheckAllValidations(text, c => IsEnglish(c) || IsPersian(c));
    }

    [Pure]
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str)
    {
        return str?.Length is null or 0;
    }

    public static bool IsNumber(this string text)
    {
        return double.TryParse(text, out _);
    }

    public static bool IsPersian(this char c)
    {
        return IsCommon(c) || PersianTools.Chars.Any(pc => pc == c) || PersianTools.SpecialChars.Any(pc => pc == c);
    }

    public static bool IsPersian(this string text)
    {
        return CheckAllValidations(text, IsPersian);
    }

    public static bool IsPersianDigit(this char c)
    {
        return PersianTools.PersianDigits.Any(x => c == x);
    }

    public static bool IsPersianOrNumber(this string text, bool canAcceptMinusKey)
    {
        return CheckAllValidations(text, c => IsDigit(c, canAcceptMinusKey) || IsPersian(c));
    }

    public static bool IsUnicode(this string str)
    {
        return str.Any(c => c > 255);
    }

    public static int CountOf(this string str, in char c, int index)
    {
        if (index == 0)
        {
            return str.IndexOf(c);
        }

        int foundCount = 0;
        index++;
        int i = 0;
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
        string result = array.Aggregate(string.Empty, (current, str) => current + quatStart + str + quatEnd + separator + " ").Trim();
        return result[..^1];
    }

    public static string Merge(string quat, string separator, params string[] array)
    {
        return array.Merge(quat, separator);
    }

    public static string Merge(this IEnumerable<string> array, in string separator)
    {
        return string.Join(separator, array.ToArray());
    }

    public static string Merge(this IEnumerable<string> array, in char separator)
    {
        return string.Join(separator, array.ToArray());
    }

    public static string Merge(this IEnumerable<string> array, string quat, string separator)
    {
        return array.Aggregate(string.Empty, (current, str) => $"{current}{quat}{str}{quat}{separator} ").Trim()[..^1];
    }

    public static string MergePair(this IEnumerable<(string, string)> splitPair, string keyValueSeparator = "=", string statementSeparator = ";")
    {
        return string.Join(statementSeparator, splitPair.Select(pair => $"{pair.Item1}{keyValueSeparator}{pair.Item2}"));
    }

    public static string PatternPolicy(in string text)
    {
        string result = string.Empty;
        string t = CorrectUnicodeProblem(text);

        char[] outChar =
        {
                '~', '`', '!', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '{', '[', '}', ']', ':', ';', '\'', '"', '<', ',', '>', '?', '|', '\\'
        };

        string[] arrayString = t.Split(outChar);

        return arrayString.Aggregate(result, (current, s) => current + s);
    }

    /// <summary>
    /// Pluralizes the specified text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    public static string? Pluralize(string? text)
    {
        return text.IsNullOrEmpty() ? null : Pluralizer.Pluralize(text);
    }

    [return: NotNullIfNotNull("str")]
    public static string? Remove(this string? str, in string? value)
    {
        return str is null ? null : (value is null ? str : str.Replace(value, ""));
    }

    public static string RemoveFromEnd(this string str, in int count)
    {
        return str.ArgumentNotNull(nameof(str)).Slice(0, str.Length - count);
    }

    public static string RemoveFromEnd(this string str, in string oldValue, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
    {
        _ = str.ArgumentNotNull(nameof(str));
        return oldValue.IsNullOrEmpty()
            ? str
            : str.EndsWith(oldValue, comparison)
                ? str.Slice(0, str.Length - oldValue.Length)
                : str;
    }
    public static string RemoveFromStart(this string str, in string oldValue, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
    {
        _ = str.ArgumentNotNull(nameof(str));
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
    {
        return Replace2(s, old.ToString(), replacement.ToString(), count);
    }

    public static string Replace2(this string s, in string old, in string replacement, in int count = 1)
    {
        return new Regex(Regex.Escape(old)).Replace(s, replacement, count);
    }

    public static string ReplaceAll(this string value, in IEnumerable<(char OldValue, char NewValue)> items)
    {
        return items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));
    }

    public static string ReplaceAll(this string value, in IEnumerable<string> oldValues, string newValue)
    {
        return oldValues.Aggregate(value, (current, oldValue) => current.Replace(oldValue, newValue));
    }

    public static string ReplaceAll(this string value, in IEnumerable<(string OldValue, string NewValue)> items)
    {
        return items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));
    }

    public static string ReplaceAll(this string value, params (char OldValue, char NewValue)[] items)
    {
        return items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));
    }

    public static string ReplaceAll(this string value, params (string OldValue, string NewValue)[] items)
    {
        return items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));
    }

    public static string SeparateCamelCase(this string? str)
    {
        if (str == null)
        {
            return string.Empty;
        }

        StringBuilder sb = new();
        for (int i = 0; i < str.Length; i++)
        {
            if (char.IsUpper(str[i]) && i > 0)
            {
                _ = sb.Append(' ');
            }

            _ = sb.Append(str[i]);
        }

        return sb.ToString();
    }

    public static string? SetPhrase(this string str, int index, string newStr, char start, char end = default)
    {
        string startStr = str.Slice(0, index + 1);
        string phrase = GetPhrase(str, index, start, end);
        if (phrase is null)
        {
            return null;
        }

        string endEnd = str.Slice(index + phrase.Length);
        string result = string.Concat(startStr, newStr, endEnd);
        return result;
    }

    /// <summary>
    /// Singularizes the specified text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    public static string? Singularize(string? text)
    {
        return text.IsNullOrEmpty() ? null : Pluralizer.Singularize(text);
    }

    public static string Space(in int count)
    {
        return new(' ', count);
    }

    public static IEnumerable<string> Split(this string value, int groupSize)
    {
        Check.IfArgumentNotNull(value, nameof(value));

        for (int x = 0; x < value.Length; x += groupSize)
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

        StringBuilder sb = new();
        foreach (char c in value)
        {
            if (char.IsUpper(c))
            {
                if (sb.Length > 0)
                {
                    yield return sb.ToString();
                }

                _ = sb.Clear();
            }
            _ = sb.Append(c);
        }
        if (sb.Length > 0)
        {
            yield return sb.ToString();
        }
    }

    public static string SplitMerge(this string str, char splitter, in string startSeparator, in string endSeparator)
    {
        StringBuilder result = new();
        foreach (string part in str.Split(splitter))
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

        IEnumerable<string> keyValuePirs = str.Split(statementSeparator).Trim();
        foreach (string keyValuePair in keyValuePirs)
        {
            string[] keyValue = keyValuePair.Split(keyValueSeparator);
            yield return (keyValue[0], keyValue[1]);
        }
    }

    public static string SqlEncodingToUtf(this string obj)
    {
        return Encoding.UTF8.GetString(Encoding.GetEncoding(1256).GetBytes(obj));
    }

    public static bool StartsWithAny(this string str, in IEnumerable<string> values)
    {
        return values.Any(str.StartsWith);
    }

    public static bool StartsWithAny(this string str, params string[] values)
    {
        return values.Any(str.StartsWith);
    }

    public static byte[] ToBytes([DisallowNull] this string value, [DisallowNull] in Encoding encoding)
    {
        Check.IfArgumentNotNull(value);
        Check.IfArgumentNotNull(encoding);

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
    {
        return value.ReplaceAll(PersianTools.Digits.Select(n => (n.English, n.Persian)));
    }

    public static string ToEnglishDigits(this string value)
    {
        return value.ReplaceAll(PersianTools.Digits.Select(n => (n.Persian, n.English)));
    }

    public static string ToHex(in string str)
    {
        StringBuilder sb = new();
        byte[] bytes = IsUnicode(str) ? Encoding.Unicode.GetBytes(str) : Encoding.ASCII.GetBytes(str);
        foreach (byte b in bytes)
        {
            _ = sb.Append(b.ToString("X"));
        }

        return sb.ToString();
    }

    public static IEnumerable<int> ToInt(this IEnumerable<string> array)
    {
        return array.Where(str => str.IsNumber()).Select(str => str.ToInt());
    }

    public static IEnumerable<string> ToLower(this IEnumerable<string> strings)
    {
        return strings.Select(str => str.ToLower());
    }

    public static string ToUnicode(this string str)
    {
        return Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(str));
    }

    public static IEnumerable<string> Trim(this IEnumerable<string> strings)
    {
        return strings.Where(item => !IsNullOrEmpty(item));
    }

    public static IEnumerable<string> TrimAll(this IEnumerable<string> values)
    {
        return values.Select(t => t.Trim());
    }

    public static IEnumerable<string> TrimAll(this IEnumerable<string> values, params char[] trimChars)
    {
        return values.Select(t => t.Trim(trimChars));
    }

    public static string? Truncate(this string? value, in int length)
    {
        return length > value?.Length ? value : value?[..^length];
    }

    public static TryMethodResult<int> TryCountOf(this string str, char c, int index)
    {
        (int Result, Exception Exception) result = CatchFunc(() => CountOf(str, c, index));
        return new(result.Exception is null, result.Result);
    }

    public static string Slice(this string s, in int start, in int? length = null)
    {
        //Check.IfArgumentNotNull(s, nameof(s));
        ReadOnlySpan<char> span = s;
        ReadOnlySpan<char> slice = length is { } l ? span.Slice(start, l) : span[start..];
        return new(slice);
    }
    public static bool IsValidIranianNationalCode(string input)
    {
        if (!Regex.IsMatch(input, @"^\d{10}$"))
        {
            return false;
        }

        int check = Convert.ToInt32(input.Slice(9, 1));
        int sum = Enumerable.Range(0, 9)
            .Select(x => Convert.ToInt32(input.Slice(x, 1)) * (10 - x))
            .Sum() % 11;

        return sum < 2 ? check == sum : check + sum == 11;
    }

    public static IEnumerable<string> Separate(this string @this, string separator)
    {
        var checkpoint = 0;
        var indexOfSeparator = @this.IndexOf(separator);
        while (indexOfSeparator >= 0)
        {
            var result = @this[checkpoint..indexOfSeparator];
            yield return result;
            checkpoint = indexOfSeparator + separator.Length;
            indexOfSeparator = @this.IndexOf(separator, checkpoint);
        }
    }
}
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using Library.Globalization;
using Library.Globalization.Pluralization;
using Library.Results;
using Library.Validations;

namespace Library.Helpers;

/// <summary>
/// A utility to do some common tasks about strings
/// </summary>
public static class StringHelper
{
    /// <summary>
    /// Adds the specified char to the string.
    /// </summary>
    /// <param name="s">     The s.</param>
    /// <param name="count"> The count.</param>
    /// <param name="add">   The add.</param>
    /// <param name="before">if set to <c>true</c> [before].</param>
    /// <returns></returns>
    [Pure]
    public static string? Add(this string? s, in int count, char add = ' ', bool before = false)
        => count is 0 ? s : before ? s?.PadLeft(s.Length + count, add) : s?.PadRight(s.Length + count, add);

    [Pure]
    public static string AddEnd(this string s, in string s1)
        => string.Concat(s, s1);

    [Pure]
    public static string AddStart(this string s, in string s1)
        => string.Concat(s1, s);

    public static IEnumerable<int> AllIndexesOf(this string str, string value, bool ignoreCase = false)
    {
        var buffer = ignoreCase ? str.ArgumentNotNull().ToLower(CultureInfo.CurrentCulture) : str.ArgumentNotNull();
        var stat = ignoreCase ? value.ArgumentNotNull().ToLower(CultureInfo.CurrentCulture) : value.ArgumentNotNull();
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

    [Pure]
    public static bool AnyCharInString(this string str, in string range)
        => str is not null && range.Any(c => str.Contains(c.ToString()));

    [Pure]
    public static string ArabicCharsToPersian(this string value)
        => value.IsNullOrEmpty() ? value : value.ReplaceAll(PersianTools.InvalidArabicCharPairs.Select(x => (x.Arabic, x.Persian)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CheckAllValidations(in string text, in Func<char, bool> regularValidate)
        => text.All(regularValidate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CheckAnyValidations(in string text, in Func<char, bool> regularValidate)
        => text.Any(regularValidate);

    [Pure]
    [return: NotNull]
    public static string[] Compact(params string[] strings)
        => strings.Where(item => !item.IsNullOrEmpty()).ToArray();

    [Pure]
    [return: NotNull]
    public static IEnumerable<string> Compact(this IEnumerable<string?>? strings)
        => (strings?.Where(item => !item.IsNullOrEmpty()).Select(s => s!)) ?? Enumerable.Empty<string>();

    [Pure]
    public static int CompareTo(this string str1, in string str, bool ignoreCase = false)
        => string.Compare(str1, str, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);

    public static string ConcatAll(IEnumerable<string> strings, string sep)
        => (sep?.Length ?? 0) switch
        {
            0 => string.Concat(strings),
            _ => string.Join(sep, strings),
        };

    [return: NotNull]
    public static string ConcatStrings(this IEnumerable<string> values)
        => string.Concat(values.ToArray());

    public static bool Contains(string str, in string value, bool ignoreCase = true) => str != null
&& (ignoreCase
                    ? str.ToLowerInvariant().Contains(value?.ToLowerInvariant())
                    : str.Contains(value));

    [Pure]
    public static bool Contains(this IEnumerable<string> array, string str, bool ignoreCase)
        => array.Any(s => s.CompareTo(str, ignoreCase) == 0);

    [Pure]
    public static bool ContainsAny(this string str, in IEnumerable<string> array)
        => array.Any(str.Contains);

    public static bool ContainsAny(this string str, IEnumerable<char> array)
        => array.Any(str.Contains);

    public static bool ContainsOf(this string str, in string target)
        => str.ToLower().Contains(target.ToLower());

    public static string CorrectUnicodeProblem(in string text)
        => ArabicCharsToPersian(text);

    public static int CountOf(this string str, char c, int index = 0)
        => str?[index..].Where(x => x == c).Count() ?? 0;

    [Pure]
    public static bool EndsWithAny(this string str, in IEnumerable<object> array)
        => array.Any(item => item is { } o && o?.ToString() is { } s && !s.IsNullOrEmpty() && str.EndsWith(s));

    [Pure]
    public static bool EndsWithAny(this string str, in IEnumerable<string> values)
        => values.Compact().Any(str.EndsWith);

    [Pure]
    public static bool EndsWithAny(this string str, params object[] array)
        => str.EndsWithAny(array.AsEnumerable());

    [Pure]
    public static bool EqualsTo(this string str1, in string str, bool ignoreCase = true)
        => str1.CompareTo(str, ignoreCase) == 0;

    [Pure]
    public static bool EqualsToAny(this string str1, bool ignoreCase, params string[] array)
        => array.Any(s => str1.EqualsTo(s, ignoreCase));

    [Pure]
    public static bool EqualsToAny(this string str1, params string[] array)
        => array.Any(s => str1.EqualsTo(s));

    public static string? FixSize(string? str, int maxLength, char gapChar = ' ')
        => str.IsNullOrEmpty()
            ? Add(string.Empty, maxLength, gapChar)
            : str.Length > maxLength ? str[..maxLength] : str.Add(maxLength - str.Length, gapChar);

    [Pure]
    public static string Format(this string format, params object[] args)
        => string.Format(format, args);

    [Pure]
    public static IEnumerable<(string Key, string Value)> GetKeyValues(this string keyValueStr, char keyValueSeparator = '=', char separator = ';')
        => keyValueStr.Split(separator).Select(raw => raw.Split(keyValueSeparator)).Select(keyValue => (keyValue[0], keyValue[1]));

    public static string? GetPhrase(this string? str, in int index, in char start, char end = default)
    {
        if (str.IsNullOrEmpty())
        {
            return null;
        }

        if (end == default(char))
        {
            end = start;
        }
        var buffer = str;
        for (var i = 0; i < index; i++)
        {
            var (text, indexOfStart) = find(buffer, start, end);
            if (indexOfStart is null or < 0 || text is null)
            {
                return null;
            }
            var nextIndex = indexOfStart.Value + text.Length + 2;
            buffer = buffer[nextIndex..];
        }

        var (result, _) = find(buffer, start, end);
        return result;

        static (string? Text, int? StartIndex) find(string str, char start, char end)
        {
            var indexOfStart = str.IndexOf(start);
            if (indexOfStart < 0)
            {
                return (default, default);
            }

            var indexOfEnd = str.IndexOf(end, indexOfStart + 1);
            return indexOfEnd < 0 ? ((string? Text, int? StartIndex))(default, default) : ((string? Text, int? StartIndex))(str.Substring(indexOfStart + 1, indexOfEnd - indexOfStart - 1), indexOfStart);
        }
    }

    public static string GetPhrase(in string text, in string start, in string end)
    {
        if (start.IsNullOrEmpty() && end.IsNullOrEmpty())
        {
            return string.Empty;
        }

        var prefixOffset = text.AsSpan().IndexOf(start);
        var startIndex = prefixOffset == -1 ? 0 : (prefixOffset + start.Length);
        var endIndex = text.AsSpan(startIndex).IndexOf(end);
        var buffer = endIndex == -1 ? text.AsSpan(startIndex) : text.AsSpan(startIndex, endIndex);
        var result = buffer.ToString();
        return result;
    }

    public static IEnumerable<string?> GetPhrases(this string str, char start, char end = default)
    {
        var index = 0;
        var result = str.GetPhrase(index, start, end);
        while (!result.IsNullOrEmpty())
        {
            yield return result;
            result = str.GetPhrase(++index, start, end);
        }
    }

    [Pure]
    public static IEnumerable<string> GroupBy(this IEnumerable<char> chars, char separator)
    {
        StringBuilder buffer = new();
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

    [Pure]
    public static IEnumerable<string> GroupBy(this string str, char separator)
    {
        StringBuilder buffer = new();
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

    [Pure]
    public static bool HasPersian(in string text)
        => CheckAnyValidations(text, IsPersian);

    [Pure]
    public static string? IfNull(this string? s, in string? value)
        => s is null ? value : s;

    [Pure]
    public static string IfNullOrEmpty(this string? str, string defaultValue)
        => str.IsNullOrEmpty() ? defaultValue : str;

    public static int? IndexOf([DisallowNull] this IEnumerable<string?> items, in string? item, bool trimmed = false, bool ignoreCase = true)
    {
        Check.IfArgumentNotNull(items, nameof(items));

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

        static string? get(string? current, bool trimmed)
            => trimmed ? current?.Trim() : current;

        static bool equals(string? current, string? item, bool ignoreCase)
            => string.Compare(current, item, ignoreCase) == 0;
    }

    public static string Initialize(int count, Func<int, string> initializer)
    {
        _ = Check.MustBe(() => count >= 0, () => new ArgumentException("Input must be non-negative string", nameof(count))).ThrowOnFail();

        var res = new StringBuilder(count);
        var i = 0;
        var num = checked(count - 1);
        if (num >= i)
        {
            do
            {
                _ = res.Append(initializer.Invoke(i));
                i++;
            }
            while (i != num + 1);
        }
        return res.ToString();
    }

    [Pure]
    public static bool IsCommon(this char c)
        => c == ' ';

    [Pure]
    public static bool IsDigit(this char c, in bool canAcceptMinusKey = false)
        => (canAcceptMinusKey && c == '-') || char.IsDigit(c);

    [Pure]
    public static bool IsDigitOrControl(this char key)
        => char.IsDigit(key) || char.IsControl(key);

    [Pure]
    public static bool IsEmpty([NotNullWhen(false)] in string? s)
        => s?.Length is 0;

    [Pure]
    public static bool IsEnglish(this char c)
        => c.IsCommon() || c is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z');

    public static bool IsEnglish(this string text)
        => CheckAllValidations(text, IsEnglish);

    public static bool IsEnglishOrNumber(this string text, bool canAcceptMinusKey = false)
        => CheckAllValidations(text, c => c.IsDigit(canAcceptMinusKey) || c.IsEnglish());

    public static bool IsInRange(string? text, bool ignoreCase, params string[] range)
        => IsInRange(text, false, ignoreCase, range);

    public static bool IsInRange(string? text, bool trimmed, bool ignoreCase, IEnumerable<string> range)
        => range.IndexOf(text, trimmed, ignoreCase) >= 0;

    public static bool IsInRange(string? text, bool trimmed, bool ignoreCase, params string[] range)
        => range.IndexOf(text, trimmed, ignoreCase) >= 0;

    public static bool IsInRange(string? text, params string[] range)
        => IsInRange(text, true, range);

    public static bool IsInString(this char c, in string text)
        => text?.Contains(c.ToString()) ?? false;

    public static bool IsInteger(this string text)
        => int.TryParse(text, out _);

    public static bool IsLetter(this char c)
        => c.IsEnglish() || IsPersian(c);

    public static bool IsLetterText(in string text)
        => CheckAllValidations(text, c => c.IsEnglish() || IsPersian(c));

    [Pure]
    [DebuggerStepThrough]
    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str)
        => str == null || str.Length == 0;

    public static bool IsNumber(in string text)
        => float.TryParse(text, out _);

    public static bool IsPersian(char c)
        => c.IsCommon() || PersianTools.Chars.Any(pc => pc == c) || PersianTools.SpecialChars.Any(pc => pc == c);

    public static bool IsPersian(in string text)
        => CheckAllValidations(text, IsPersian);

    public static bool IsPersianDigit(char c)
        => PersianTools.PersianDigits.Any(x => c == x);

    public static bool IsPersianOrNumber(in string text, bool canAcceptMinusKey)
        => CheckAllValidations(text, c => c.IsDigit(canAcceptMinusKey) || IsPersian(c));

    public static bool IsUnicode(this string str)
        => str.Any(c => c > 255);

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

    public static void Iterate(this string str, [DisallowNull] Action<char> action)
    {
        Check.IfArgumentNotNull(action);
        if (str.IsNullOrEmpty())
        {
            return;
        }
        var i = 0;
        var num = checked(str.Length - 1);
        if (num >= i)
        {
            do
            {
                action(str[i]);
                i++;
            }
            while (i != num + 1);
        }
    }

    public static string Merge(string quatStart, string quatEnd, string separator, params object[] array)
    {
        var result = array.Aggregate(string.Empty, (current, str) => current + quatStart + str + quatEnd + separator + " ").Trim();
        return result[..^1];
    }

    public static string Merge(string quat, string separator, params string[] array) => array.Merge(quat, separator);

    public static string Merge(this IEnumerable<string> array, in string separator)
        => string.Join(separator, array.ToArray());

    public static string Merge(this IEnumerable<string> array, in char separator)
        => string.Join(separator, array.ToArray());

    public static string Merge(this IEnumerable<string> array, string quat, string separator) => array.Aggregate(string.Empty, (current, str)
        => $"{current}{quat}{str}{quat}{separator} ").Trim()[..^1];

    public static string MergePair(this IEnumerable<(string, string)> splitPair, string keyValueSeparator = "=", string statementSeparator = ";")
        => string.Join(statementSeparator, splitPair.Select(pair => $"{pair.Item1}{keyValueSeparator}{pair.Item2}"));

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

    [return: NotNullIfNotNull(nameof(str))]
    public static string? Remove(this string? str, in string? value)
        => str is null ? null : value is null ? str : str.Replace(value, "");

    public static string RemoveEnd(this string str, in int count)
        => str.ArgumentNotNull(nameof(str)).Slice(0, str.Length - count);

    public static string RemoveEnd(this string str, in string oldValue, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
    {
        _ = str.ArgumentNotNull(nameof(str));
        return oldValue.IsNullOrEmpty()
            ? str
            : str.EndsWith(oldValue, comparison)
                ? str.Slice(0, str.Length - oldValue.Length)
                : str;
    }

    public static string RemoveStart(this string str, in string oldValue, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
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
        => s.Replace2(old.ToString(), replacement.ToString(), count);

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

    [return: NotNullIfNotNull(nameof(str))]
    public static string? Separate(this string? str, params char[] separators)
    {
        if (separators?.Any() is not true)
        {
            separators = new[] { '\0', '\n', '\r', '\t', '_', '-' };
        }
        if (str.IsNullOrEmpty())
        {
            return str;
        }

        var sb = new StringBuilder();
        for (var i = 0; i < str.Length; i++)
        {
            var (isSeparator, shouldIgnore) = determineSeparator(str[i], separators);
            if (i > 0 && isSeparator)
            {
                _ = sb.Append(' ');
            }
            if (!shouldIgnore)
            {
                _ = sb.Append(str[i]);
            }
        }

        return sb.ToString();

        static (bool IsSeparator, bool ShouldIgnore) determineSeparator(char c, char[] separators)
            => separators.Contains(c) ? (true, true) : (char.IsUpper(c), false);
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static string? SeparateCamelCase(this string? value) 
        => value.SplitCamelCase().Merge(" ");

    public static string? SetPhrase(this string str, int index, string newStr, char start, char end = default)
    {
        var startStr = str.Slice(0, index + 1);
        var phrase = str.GetPhrase(index, start, end);
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

    public static string Slice(this string s, in int start, in int? length = null)
    {
        ReadOnlySpan<char> span = s;
        var slice = length is { } l ? span.Slice(start, l) : span[start..];
        return new(slice);
    }

    public static string Space(in int count)
        => new(' ', count);

    public static IEnumerable<string> Split(this string @this, string separator)
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

    [Obsolete("Subject to delete", true)]
    public static IEnumerable<string> Split(this string value, int groupSize)
    {
        Check.IfArgumentNotNull(value);

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

        StringBuilder sb = new();
        foreach (var c in value)
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
            value = value.ArabicCharsToPersian();
        }

        return value;
    }

    public static string ToEnglishDigits(this string value)
        => value.ReplaceAll(PersianTools.Digits.Select(n => (n.Persian, n.English)));

    public static string ToHex(in string str)
    {
        StringBuilder sb = new();
        var bytes = str.IsUnicode() ? Encoding.Unicode.GetBytes(str) : Encoding.ASCII.GetBytes(str);
        foreach (var b in bytes)
        {
            _ = sb.Append(b.ToString("X"));
        }

        return sb.ToString();
    }

    public static IEnumerable<int> ToInt(in IEnumerable<string> array)
        => array.Where(str => IsNumber(str)).Select(str => str.Cast().ToInt());

    public static IEnumerable<string> ToLower(this IEnumerable<string> strings)
        => strings.Select(str => str.ToLower());

    public static string ToPersianDigits(this string value)
        => value.ReplaceAll(PersianTools.Digits.Select(n => (n.English, n.Persian)));

    public static string? ToUnicode(this string? str)
        => str is null ? null : Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(str));

    public static IEnumerable<string> Trim(this IEnumerable<string> strings)
        => strings.Where(item => !item.IsNullOrEmpty());

    public static IEnumerable<string> TrimAll(this IEnumerable<string> values)
        => values.Select(t => t.Trim());

    public static IEnumerable<string> TrimAll(this IEnumerable<string> values, params char[] trimChars)
        => values.Select(t => t.Trim(trimChars));

    public static string? Truncate(this string? value, in int length)
        => length > value?.Length ? value : value?[..^length];

    public static TryMethodResult<int> TryCountOf(this string str, char c, int index)
        => CatchFunc(() => str.CountOf(c, index)).Fluent().WithNew(x => TryMethodResult<int>.TryParseResult(x.Exception is null, x.Result));
}
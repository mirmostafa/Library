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
    private static readonly char[] _standardSeparators = new[] { '\0', '\n', '\r', '\t', '_', '-' };

    /// <summary>
    /// Adds a specified number of characters to a string, either before or after the string.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="count">The count.</param>
    /// <param name="add">The add.</param>
    /// <param name="before">if set to <c>true</c> [before].</param>
    /// <returns></returns>
    [Pure]
    [return: NotNullIfNotNull(nameof(s))]
    public static string? Add(this string? s, in int count, char add = ' ', bool before = false) =>
        count is 0 ? s : before ? s?.PadLeft(s.Length + count, add) : s?.PadRight(s.Length + count, add);

    /// <summary>
    /// Adds the given string to the end of the current string.
    /// </summary>
    [Pure]
    public static string AddEnd(this string s, in string s1) =>
        string.Concat(s, s1);

    /// <summary>
    /// Adds the given string to the start of the current string.
    /// </summary>
    [Pure]
    public static string AddStart(this string s, in string s1)
        => string.Concat(s1, s);

    /// <summary>
    /// Gets all indexes of a given string in another string.
    /// </summary>
    /// <param name="str">The string to search in.</param>
    /// <param name="value">The string to search for.</param>
    /// <param name="ignoreCase">Whether to ignore case when searching.</param>
    /// <returns>An enumerable of all indexes of the given string in the other string.</returns>
    public static IEnumerable<int> AllIndexesOf(this string str, string value, bool ignoreCase = false)
    {
        var buffer = ignoreCase ? str.ArgumentNotNull().ToLower(CultureInfo.CurrentCulture) : str.ArgumentNotNull();
        var stat = ignoreCase ? value.ArgumentNotNull().ToLower(CultureInfo.CurrentCulture) : value.ArgumentNotNull();
        var result = 0;
        int currentIndex;
        while ((currentIndex = buffer.IndexOf(stat, StringComparison.Ordinal)) != -1)
        {
            yield return result + currentIndex;
            result += currentIndex + stat.Length;
            buffer = buffer[(currentIndex + stat.Length)..];
        }
    }

    /// <summary>
    /// Checks if any character in the given string is present in the given range.
    /// </summary>
    [Pure]
    public static bool AnyCharInString(this string str, in string range) =>
        !string.IsNullOrEmpty(str) && range.Any(str.Contains);

    /// <summary>
    /// Replaces all invalid Arabic characters in the given string with their Persian equivalents.
    /// </summary>
    [Pure]
    [return: NotNullIfNotNull(nameof(value))]
    public static string ArabicCharsToPersian(this string value) =>
        value.IsNullOrEmpty() ? value : value.ReplaceAll(PersianTools.InvalidArabicCharPairs.Select(x => (x.Arabic, x.Persian)));

    /// <summary>
    /// Checks if all characters in the given string are valid according to the given validation function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CheckAllValidations(in string text, in Func<char, bool> regularValidate) =>
        text.All(regularValidate);

    /// <summary>
    /// Checks if any of the characters in the given string satisfy the given validation function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CheckAnyValidations(in string text, in Func<char, bool> regularValidate) =>
        text.Any(regularValidate);

    /// <summary>
    /// Compacts an array of strings by removing any empty strings.
    /// </summary>
    /// <param name="strings">The strings to compact.</param>
    /// <returns>The compacted array of strings.</returns>
    [Pure]
    [return: NotNull]
    public static string[] Compact(params string[] strings) =>
        strings.Where(item => !item.IsNullOrEmpty()).ToArray();

    /// <summary>
    /// Filters out null or empty strings from the given IEnumerable and returns a new IEnumerable
    /// with only non-null and non-empty strings.
    /// </summary>
    /// <param name="strings">The IEnumerable of strings to filter.</param>
    /// <returns>A new IEnumerable with only non-null and non-empty strings.</returns>
    [Pure]
    [return: NotNull]
    public static IEnumerable<string> Compact(this IEnumerable<string?>? strings) =>
        (strings?.Where(item => !item.IsNullOrEmpty()).Select(s => s!)) ?? Enumerable.Empty<string>();

    /// <summary>
    /// Compares two strings and returns an integer that indicates their relative position in the
    /// sort order.
    /// </summary>
    /// <param name="str1">The first string to compare.</param>
    /// <param name="str">The second string to compare.</param>
    /// <param name="ignoreCase">A Boolean value indicating a case-sensitive or insensitive comparison.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
    /// </returns>
    [Pure]
    public static int CompareTo(this string str1, in string str, bool ignoreCase = false)
        => string.Compare(str1, str, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);

    /// <summary>
    /// Concatenates all strings in the given enumerable, using the given separator.
    /// </summary>
    /// <param name="strings">The strings to concatenate.</param>
    /// <param name="sep">The separator to use.</param>
    /// <returns>The concatenated string.</returns>
    public static string ConcatAll(IEnumerable<string> strings, string sep)
        => (sep?.Length ?? 0) switch
        {
            0 => string.Concat(strings),
            _ => string.Join(sep, strings),
        };

    /// <summary>
    /// Concatenates the strings in the given IEnumerable and returns the result as a string.
    /// </summary>
    /// <param name="values">The strings to concatenate.</param>
    /// <returns>The concatenated string.</returns>
    [return: NotNull]
    public static string ConcatStrings(this IEnumerable<string> values) =>
        string.Concat(values.ToArray());

    /// <summary>
    /// Checks if a string contains a specified value, with an optional case-insensitive comparison.
    /// </summary>
    public static bool Contains(string str, in string value, bool ignoreCase = true)
        => str?.IndexOf(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) >= 0;

    /// <summary>
    /// Checks if a given string is present in an array of strings, with the option to ignore case.
    /// </summary>
    /// <param name="array">The array of strings to search in.</param>
    /// <param name="str">The string to search for.</param>
    /// <param name="ignoreCase">Whether to ignore case when searching.</param>
    /// <returns>True if the string is present in the array, false otherwise.</returns>
    public static bool Contains(this IEnumerable<string> array, string str, bool ignoreCase)
    {
        foreach (var s in array)
        {
            if (ignoreCase)
            {
                if (string.Equals(s, str, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            else
            {
                if (s == str)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if a string contains any of the strings in the given array.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <param name="array">The array of strings to check against.</param>
    /// <returns>True if the string contains any of the strings in the array, false otherwise.</returns>
    public static bool ContainsAny(this string str, in IEnumerable<string> array)
    {
        foreach (var item in array)
        {
            if (str.Contains(item))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if a string contains any of the characters in the given array.
    /// </summary>
    public static bool ContainsAny(this string str, IEnumerable<char> array)
        => array.Any(str.Contains);

    /// <summary>
    /// Checks if a string contains a target string, ignoring case.
    /// </summary>
    public static bool ContainsOf(this string str, in string target)
        => str.Contains(target.ToLower(), StringComparison.CurrentCultureIgnoreCase);

    /// <summary>
    /// Converts a string to Google Standard Encoding.
    /// </summary>
    /// <param name="inputString">The string to be converted.</param>
    /// <returns>The converted string.</returns>
    public static string ConvertToGoogleStandardEncoding(string inputString)
    {
        var bytes = Encoding.UTF8.GetBytes(inputString);
        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// Converts Arabic characters to Persian characters in a given string.
    /// </summary>
    public static string CorrectUnicodeProblem(in string text) =>
        ArabicCharsToPersian(text);

    /// <summary>
    /// Counts the number of occurrences of a specified character in a string, starting from a
    /// specified index.
    /// </summary>
    public static int CountOf(this string str, char c, int index = 0)
        => str?.Skip(index).Count(x => x == c) ?? 0;

    /// <summary>
    /// Checks if the string ends with any of the items in the given array.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <param name="array">The array of items to check against.</param>
    /// <returns>True if the string ends with any of the items in the array, false otherwise.</returns>
    [Pure]
    public static bool EndsWithAny(this string str, in IEnumerable<object> array)
        => array.Any(item => item is { } o && o?.ToString() is { } s && !s.IsNullOrEmpty() && str.EndsWith(s));

    /// <summary>
    /// Checks if the given string ends with any of the strings in the given IEnumerable.
    /// </summary>
    [Pure]
    public static bool EndsWithAny(this string str, in IEnumerable<string> values)
        => values.Compact().Any(str.EndsWith);

    /// <summary>
    /// Checks if the string ends with any of the elements in the specified array.
    /// </summary>
    [Pure]
    public static bool EndsWithAny(this string str, params object[] array)
        => str.EndsWithAny(array.AsEnumerable());

    /// <summary>
    /// Compares two strings and returns a boolean value indicating whether they are equal.
    /// </summary>
    [Pure]
    public static bool EqualsTo(this string str1, in string str, bool ignoreCase = true)
        => str1.CompareTo(str, ignoreCase) == 0;

    /// <summary>
    /// Checks if the given string is equal to any of the strings in the given array, with the given
    /// ignoreCase option.
    /// </summary>
    [Pure]
    public static bool EqualsToAny(this string str1, bool ignoreCase, params string[] array)
        => array.Any(s => str1.EqualsTo(s, ignoreCase));

    /// <summary>
    /// Checks if the given string is equal to any of the strings in the given array.
    /// </summary>
    [Pure]
    public static bool EqualsToAny(this string str1, params string[] array)
        => array.Any(s => str1.EqualsTo(s));

    /// <summary>
    /// Fixes the size of the given string to the specified maximum length, padding with the given
    /// gap character if necessary.
    /// </summary>
    [return: NotNull]
    public static string FixSize(in string? str, int maxLength, char gapChar = ' ')
        => str.IsNullOrEmpty()
                ? new string(gapChar, maxLength)
                : str.Length > maxLength ? str[..maxLength] : str.PadRight(maxLength, gapChar);

    /// <summary>
    /// Formats a string using the specified arguments.
    /// </summary>
    [Pure]
    public static string Format(this string format, params object[] args)
        => string.Format(format, args);

    /// <summary>
    /// Gets a sequence of key-value tuples from a string.
    /// </summary>
    /// <param name="keyValueStr">The string to parse.</param>
    /// <param name="keyValueSeparator">The character used to separate the key and value.</param>
    /// <param name="separator">The character used to separate the key-value pairs.</param>
    /// <returns>A sequence of key-value tuples.</returns>
    [Pure]
    public static IEnumerable<(string Key, string Value)> GetKeyValues(this string keyValueStr, char keyValueSeparator = '=', char separator = ';')
        => keyValueStr.Split(separator).Select(raw => raw.Split(keyValueSeparator)).Select(keyValue => (keyValue[0], keyValue[1]));

    /// <summary>
    /// Gets the phrase from the given string based on the given index, start and end characters.
    /// </summary>
    /// <param name="str">The string to search in.</param>
    /// <param name="index">The index of the phrase to get.</param>
    /// <param name="start">The start character of the phrase.</param>
    /// <param name="end">The end character of the phrase.</param>
    /// <returns>The phrase from the given string based on the given index, start and end characters.</returns>
    public static string? GetPhrase(this string? str, in int index, in char start, char end = default)
    {
        // Check if the string is null or empty
        if (str.IsNullOrEmpty())
        {
            return null;
        }

        // If the end character is not specified, set it to the same as the start character
        if (end == default(char))
        {
            end = start;
        }
        var buffer = str;

        // Loop through the string the specified number of times
        for (var i = 0; i < index; i++)
        {
            // Find the start and end indices of the phrase
            var (text, indexOfStart) = find(buffer, start, end);
            // If the start index is null or less than 0, or the text is null, return null
            if (indexOfStart is null or < 0 || text is null)
            {
                return null;
            }
            // Set the next index to the start index plus the length of the phrase plus two
            var nextIndex = indexOfStart.Value + text.Length + 2;
            // Set the buffer to the remaining string
            buffer = buffer[nextIndex..];
        }

        // Find the start and end indices of the phrase
        var (result, _) = find(buffer, start, end);
        return result;

        // Find the start and end indices of the phrase
        static (string? Text, int? StartIndex) find(string str, char start, char end)
        {
            // Find the start index of the phrase
            var indexOfStart = str.IndexOf(start);
            // If the start index is less than 0, return default values
            if (indexOfStart < 0)
            {
                return (default, default);
            }

            // Find the end index of the phrase
            var indexOfEnd = str.IndexOf(end, indexOfStart + 1);
            // If the end index is less than 0, return default values
            return indexOfEnd < 0 ? ((string? Text, int? StartIndex))(default, default) : ((string? Text, int? StartIndex))(str.Substring(indexOfStart + 1, indexOfEnd - indexOfStart - 1), indexOfStart);
        }
    }

    /// <summary>
    /// Gets a substring from a given string based on the start and end strings.
    /// </summary>
    /// <param name="text">The string to search in.</param>
    /// <param name="start">The start string.</param>
    /// <param name="end">The end string.</param>
    /// <returns>The substring between the start and end strings.</returns>
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

    /// <summary>
    /// Gets all phrases from a string that are delimited by a start and end character.
    /// </summary>
    /// <param name="str">The string to search.</param>
    /// <param name="start">The start character.</param>
    /// <param name="end">The end character.</param>
    /// <returns>An enumerable of all phrases found.</returns>
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

    /// <summary>
    /// Groups a sequence of characters into strings based on a separator character.
    /// </summary>
    /// <param name="chars">The sequence of characters to group.</param>
    /// <param name="separator">The separator character.</param>
    /// <returns>A sequence of strings containing the grouped characters.</returns>
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

    /// <summary>
    /// Groups a string by a given separator character.
    /// </summary>
    /// <param name="str">The string to group.</param>
    /// <param name="separator">The separator character.</param>
    /// <returns>An enumerable of strings grouped by the separator character.</returns>
    public static IEnumerable<string> GroupBy(string str, char separator)
    {
        // Create a new StringBuilder object
        StringBuilder buffer = new();

        // Iterate through each character in the string
        foreach (var c in str)
        {
            // If the character is the separator
            if (c == separator)
            {
                // Return the current buffer as a string
                yield return buffer.ToString();
                // Clear the buffer
                _ = buffer.Clear();
            }
            else
            {
                // Append the character to the buffer
                _ = buffer.Append(c);
            }
        }
    }

    /// <summary>
    /// This method checks if the given string contains any Persian characters.
    /// </summary>
    /// <param name="text">The string to check.</param>
    /// <returns>True if the string contains Persian characters, false otherwise.</returns>
    [Pure]
    public static bool HasPersian(in string text)
        => CheckAnyValidations(text, IsPersian);

    /// <summary>
    /// Returns the given string if not null, otherwise returns the given value.
    /// </summary>
    [Pure]
    public static string? IfNull(this string? s, in string? value)
        => s ?? value;

    /// <summary>
    /// Returns the given string if it is not null or empty, otherwise returns the default value.
    /// </summary>
    [Pure]
    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static string? IfNullOrEmpty(this string? str, string? defaultValue) =>
        string.IsNullOrEmpty(str) ? defaultValue : str;

    /// <summary>
    /// Gets the index of the specified item in the given enumerable of strings.
    /// </summary>
    /// <param name="items">The enumerable of strings.</param>
    /// <param name="item">The item to search for.</param>
    /// <param name="trimmed">Whether or not to trim the strings before comparison.</param>
    /// <param name="ignoreCase">Whether or not to ignore case when comparing strings.</param>
    /// <returns>The index of the item in the enumerable, or null if the item is not found.</returns>
    public static int? IndexOf([DisallowNull] this IEnumerable<string?> items, in string? item, bool trimmed = false, bool ignoreCase = true)
    {
        // Check if the argument is not null
        Check.MustBeArgumentNotNull(items, nameof(items));

        // Get the item and trim it if necessary
        var itm = get(item, trimmed);

        // Initialize the index
        var index = 0;

        // Get the enumerator
        var enumerator = items.GetEnumerator();

        // Iterate through the enumerator
        while (enumerator.MoveNext())
        {
            // Check if the current item is equal to the item
            if (equals(get(enumerator.Current, trimmed), itm, ignoreCase))
            {
                // Return the index if the items are equal
                return index;
            }

            // Increment the index
            index++;
        }

        // Return null if the item is not found
        return null;

        // Get the item and trim it if necessary
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string? get(string? current, bool trimmed) =>
            trimmed ? current?.Trim() : current;

        // Check if the items are equal
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool equals(string? current, string? item, bool ignoreCase) =>
            string.Compare(current, item, ignoreCase) == 0;
    }

    /// Checks if the given character is a common character. </summary>
    [Pure]
    public static bool IsCommon(this char c)
        => c == ' ';

    /// <summary>
    /// Checks if the given character is a digit or a minus key if specified.
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <param name="canAcceptMinusKey">Whether or not to accept the minus key.</param>
    /// <returns>True if the character is a digit or a minus key if specified, false otherwise.</returns>
    [Pure]
    public static bool IsDigit(this char c, in bool canAcceptMinusKey = false)
        => (canAcceptMinusKey && c == '-') || char.IsDigit(c);

    /// <summary>
    /// Checks if the given character is either a digit or a control character.
    /// </summary>
    [Pure]
    public static bool IsDigitOrControl(this char key)
        => char.IsDigit(key) || char.IsControl(key);

    /// <summary>
    /// Checks if the given string is empty.
    /// </summary>
    /// <param name="s">The string to check.</param>
    /// <returns>True if the string is empty, false otherwise.</returns>
    [Pure]
    public static bool IsEmpty([NotNullWhen(false)] in string? s)
        => s?.Length is 0;

    /// <summary>
    /// Checks if the given character is an English letter.
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns>True if the character is an English letter, false otherwise.</returns>
    [Pure]
    public static bool IsEnglish(this char c)
        => c.IsCommon() || c is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z');

    /// <summary>
    /// Checks if the given string is in English language.
    /// </summary>
    /// <param name="text">The string to be checked.</param>
    /// <returns>True if the string is in English, false otherwise.</returns>
    public static bool IsEnglish(this string text)
        => CheckAllValidations(text, IsEnglish);

    /// <summary>
    /// Checks if the given string is composed of only English characters or numbers.
    /// </summary>
    /// <param name="text">The string to be checked.</param>
    /// <param name="canAcceptMinusKey">Indicates if the minus key is accepted.</param>
    /// <returns>
    /// True if the string is composed of only English characters or numbers, false otherwise.
    /// </returns>
    public static bool IsEnglishOrNumber(this string text, bool canAcceptMinusKey = false)
        => CheckAllValidations(text, c => c.IsDigit(canAcceptMinusKey) || c.IsEnglish());

    /// <summary>
    /// Checks if the given string is in the given range, optionally ignoring case.
    /// </summary>
    /// <param name="text">The string to check.</param>
    /// <param name="ignoreCase">Whether to ignore case when checking.</param>
    /// <param name="range">The range of strings to check against.</param>
    /// <returns>True if the given string is in the given range, false otherwise.</returns>
    public static bool IsInRange(string? text, bool ignoreCase, params string[] range)
        => IsInRange(text, false, ignoreCase, range);

    /// <summary>
    /// Checks if the given text is in the given range.
    /// </summary>
    /// <param name="text">The text to check.</param>
    /// <param name="trimmed">Whether the text should be trimmed before checking.</param>
    /// <param name="ignoreCase">Whether the case should be ignored when checking.</param>
    /// <param name="range">The range to check against.</param>
    /// <returns>True if the text is in the range, false otherwise.</returns>
    public static bool IsInRange(string? text, bool trimmed, bool ignoreCase, IEnumerable<string> range)
        => range.IndexOf(text, trimmed, ignoreCase) >= 0;

    /// <summary>
    /// Checks if the given text is in the given range.
    /// </summary>
    /// <param name="text">The text to check.</param>
    /// <param name="trimmed">Whether to trim the text before checking.</param>
    /// <param name="ignoreCase">Whether to ignore case when checking.</param>
    /// <param name="range">The range to check against.</param>
    /// <returns>True if the text is in the range, false otherwise.</returns>
    public static bool IsInRange(string? text, bool trimmed, bool ignoreCase, params string[] range)
        => range.IndexOf(text, trimmed, ignoreCase) >= 0;

    /// <summary>
    /// Checks if the given string is in the given range of strings.
    /// </summary>
    /// <param name="text">The string to check.</param>
    /// <param name="range">The range of strings to check against.</param>
    /// <returns>True if the given string is in the given range, false otherwise.</returns>
    public static bool IsInRange(string? text, params string[] range)
        => IsInRange(text, true, range);

    /// <summary>
    /// Checks if a character is contained in a given string.
    /// </summary>
    public static bool IsInString(this char c, in string text)
        => text?.Contains(c.ToString()) ?? false;

    /// <summary>
    /// Checks if a given string can be parsed to an integer.
    /// </summary>
    public static bool IsInteger(this string text)
        => int.TryParse(text, out _);

    /// <summary>
    /// Checks if the given character is a letter (English or Persian).
    /// </summary>
    public static bool IsLetter(this char c)
        => c.IsEnglish() || IsPersian(c);

    /// <summary>
    /// Checks if the given string contains only letters (English or Persian).
    /// </summary>
    /// <param name="text">The string to check.</param>
    /// <returns>True if the given string contains only letters, false otherwise.</returns>
    public static bool IsLetterText(in string text)
        => CheckAllValidations(text, c => c.IsEnglish() || IsPersian(c));

    /// <summary>
    /// Checks if the given string is null or empty.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str)
        => str == null || str.Length == 0;

    /// <summary>
    /// Checks if the given string is a valid number.
    /// </summary>
    /// <param name="text">The string to check.</param>
    /// <returns>True if the string is a valid number, false otherwise.</returns>
    public static bool IsNumber(in string text)
        => float.TryParse(text, out _);

    /// <summary>
    /// Checks if the given character is a Persian character.
    /// </summary>
    public static bool IsPersian(char c)
        => c.IsCommon() || PersianTools.Chars.Any(pc => pc == c) || PersianTools.SpecialChars.Any(pc => pc == c);

    public static bool IsPersian(in string text)
        => CheckAllValidations(text, IsPersian);

    /// <summary>
    /// Checks if the given character is a Persian digit.
    /// </summary>
    public static bool IsPersianDigit(char c)
        => PersianTools.PersianDigits.Any(x => c == x);

    /// <summary>
    /// Checks if the given string is either a Persian character or a number.
    /// </summary>
    /// <param name="text">The string to check.</param>
    /// <param name="canAcceptMinusKey">Whether or not to accept the minus key.</param>
    /// <returns>True if the given string is either a Persian character or a number, false otherwise.</returns>
    public static bool IsPersianOrNumber(in string text, bool canAcceptMinusKey)
        => CheckAllValidations(text, c => c.IsDigit(canAcceptMinusKey) || IsPersian(c));

    /// <summary>
    /// Checks if a string contains any characters with a Unicode value greater than 255.
    /// </summary>
    public static bool IsUnicode(this string str)
        => str.Any(c => c > 255);

    /// <summary>
    /// Checks if the given input is a valid Iranian National Code.
    /// </summary>
    /// <param name="input">The input string to be checked.</param>
    /// <returns>True if the input is a valid Iranian National Code, false otherwise.</returns>
    /// <remarks>
    /// This code checks if a given string is a valid Iranian National Code. It first checks if the
    /// input string is 10 digits long using a regular expression. It then calculates the sum of the
    /// first 9 digits multiplied by their respective weights (10-i). The sum is then divided by 11
    /// and the remainder is compared to the last digit of the input string. If the remainder is
    /// less than 2, the last digit must be equal to the remainder. Otherwise, the last digit must
    /// be equal to 11 minus the remainder. If both conditions are met, the input string is a valid
    /// Iranian National Code.
    /// </remarks>
    public static bool IsValidIranianNationalCode(string nationalCode)
    {
        if (nationalCode.IsNullOrEmpty())
        {
            return false;
        }

        if (!Regex.IsMatch(nationalCode, @"^\d{10}$"))
        {
            return false;
        }

        var check = int.Parse(nationalCode[9].ToString());
        var sum = Enumerable.Range(0, 9)
            .Select(x => int.Parse(nationalCode[x].ToString()) * (10 - x))
            .Sum() % 11;

        return (sum < 2 && check == sum) || (sum >= 2 && check + sum == 11);
    }

    /// <summary>
    /// Iterates through each character of a string and performs an action on it.
    /// </summary>
    /// <param name="str">The string to iterate through.</param>
    /// <param name="action">The action to perform on each character.</param>
    public static void Iterate(this string str, [DisallowNull] Action<char> action)
    {
        Check.MustBeArgumentNotNull(action);
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

    /// <summary>
    /// Merges the given strings with the given separator and parameters.
    /// </summary>
    /// <param name="quatStart">The start of the quotation.</param>
    /// <param name="quatEnd">The end of the quotation.</param>
    /// <param name="separator">The separator to use.</param>
    /// <param name="array">The array of objects to merge.</param>
    /// <returns>The merged string.</returns>
    public static string Merge(string quatStart, string quatEnd, string separator, params object[] array)
    {
        var result = array.Aggregate(string.Empty, (current, str) => current + quatStart + str + quatEnd + separator + " ").TrimEnd();
        return result[..^1];
    }

    /// <summary>
    /// Merges the specified quat, separator and array into a single string.
    /// </summary>
    public static string Merge(string quat, string separator, params string[] array)
            => array.Merge(quat, separator);

    /// <summary> Merges the elements of an IEnumerable<string> into a single string, separated by
    /// the given separator. </summary>
    public static string Merge(this IEnumerable<string> array, in string separator)
        => string.Join(separator, array.ToArray());

    /// <summary> Merges the elements of an IEnumerable<string> into a single string, separated by
    /// the given separator. </summary>
    public static string Merge(this IEnumerable<string> array, in char separator)
        => string.Join(separator, array.ToArray());

    /// <summary> Merges the elements of an IEnumerable<string> array into a single string, using
    /// the specified quotation mark and separator. </summary> <param name="array">The array of
    /// strings to be merged.</param> <param name="quat">The quotation mark to be used.</param>
    /// <param name="separator">The separator to be used.</param> <returns>A string containing the
    /// merged elements of the array.</returns>
    public static string Merge(this IEnumerable<string> array, string quat, string separator)
        => array.Aggregate(string.Empty, (current, str) => $"{current}{quat}{str}{quat}{separator}").TrimEnd(separator.ToCharArray())[..^1];

    /// <summary>
    /// Merges a collection of tuples into a single string, separated by a key-value separator and
    /// statement separator.
    /// </summary>
    /// <param name="splitPair">The collection of tuples to merge.</param>
    /// <param name="keyValueSeparator">
    /// The separator to use between the key and value of each tuple.
    /// </param>
    /// <param name="statementSeparator">The separator to use between each tuple.</param>
    /// <returns>A single string containing the merged tuples.</returns>
    public static string MergePair(this IEnumerable<(string, string)> splitPair, string keyValueSeparator = "=", string statementSeparator = ";")
        => string.Join(statementSeparator, splitPair.Select(pair => $"{pair.Item1}{keyValueSeparator}{pair.Item2}"));

    /// <summary>
    /// Removes special characters from a string and returns the result.
    /// </summary>
    /// <param name="text">The string to be processed.</param>
    /// <returns>A string with all special characters removed.</returns>
    /// <remarks>
    /// This code takes a string as an input and returns a string with all special characters
    /// removed. It first calls the CorrectUnicodeProblem() method to correct any Unicode problems
    /// in the string. It then creates an array of special characters to be removed from the string.
    /// It then splits the string into an array of strings, removing any empty entries. Finally, it
    /// uses the Aggregate() method to combine the strings in the array into a single string and
    /// returns the result.
    /// </remarks>
    public static string PatternPolicy(in string text)
    {
        var result = string.Empty;
        var t = CorrectUnicodeProblem(text);

        char[] outChar =
        {
                '~', '`', '!', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '{', '[', '}', ']', ':', ';', '\'', '"', '<', ',', '>', '?', '|', '\\'
        };

        var arrayString = t.Split(outChar, StringSplitOptions.RemoveEmptyEntries);

        return arrayString.Aggregate(result, (current, s) => current + s);
    }

    /// <summary>
    /// Returns the plural form of the given string, or null if the string is null or empty.
    /// </summary>
    public static string? Pluralize(string? text)
        => text.IsNullOrEmpty() ? null : Pluralizer.Pluralize(text);

    /// <summary>
    /// Removes the specified value from the string.
    /// </summary>
    /// <param name="str">The string to remove the value from.</param>
    /// <param name="value">The value to remove.</param>
    /// <returns>The string with the value removed, or null if the string is null.</returns>
    [return: NotNullIfNotNull(nameof(str))]
    public static string? Remove(this string? str, in string? value)
        => value is null ? str : str?.Replace(value, "");

    /// <summary>
    /// Removes the specified number of characters from the end of the string.
    /// </summary>
    public static string RemoveEnd(this string str, in int count)
        => str.ArgumentNotNull(nameof(str)).Slice(0, str.Length - count);

    /// <summary>
    /// Removes the end of a string if it matches the specified value.
    /// </summary>
    /// <param name="str">The string to remove the end from.</param>
    /// <param name="oldValue">The value to remove from the end of the string.</param>
    /// <param name="comparison">The comparison type to use when comparing the end of the string.</param>
    /// <returns>The string with the end removed if it matches the specified value.</returns>
    public static string RemoveEnd(this string str, in string oldValue, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
    {
        _ = str.ArgumentNotNull(nameof(str));
        return oldValue.IsNullOrEmpty()
            ? str
            : str.EndsWith(oldValue, comparison)
                ? str.Slice(0, str.Length - oldValue.Length)
                : str;
    }

    /// <summary>
    /// Removes the start of a string if it matches the given value.
    /// </summary>
    /// <param name="str">The string to remove the start from.</param>
    /// <param name="oldValue">The value to remove from the start of the string.</param>
    /// <param name="comparison">The comparison type to use when comparing the start of the string.</param>
    /// <returns>The string with the start removed if it matches the given value.</returns>
    public static string RemoveStart(this string str, in string oldValue, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
    {
        _ = str.ArgumentNotNull(nameof(str));
        return oldValue.IsNullOrEmpty()
            ? str
            : str.StartsWith(oldValue, comparison) ? str.Slice(oldValue.Length, str.Length - oldValue.Length) : str;
    }

    /// <summary>
    /// Repeats a given string a specified number of times.
    /// </summary>
    /// <param name="text">The string to be repeated.</param>
    /// <param name="count">The number of times to repeat the string.</param>
    /// <returns>A string containing the repeated text.</returns>
    public static string Repeat(this string text, in int count) =>
        string.Concat(Enumerable.Repeat(text, count));

    /// <summary>
    /// Replaces a specified number of occurrences of a character in a string with a new character.
    /// </summary>
    public static string Replace2(this string s, char old, in char replacement, in int count = 1)
        => s.Replace2(old.ToString(), replacement.ToString(), count);

    /// <summary>
    /// Replaces a specified number of occurrences of a string with another string.
    /// </summary>
    public static string Replace2(this string s, in string old, in string replacement, in int count = 1)
        => new Regex(Regex.Escape(old)).Replace(s, replacement, count);

    /// <summary>
    /// Replaces all occurrences of a character in a string with a new character.
    /// </summary>
    public static string ReplaceAll(this string value, in IEnumerable<(char OldValue, char NewValue)> items)
        => items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));

    /// <summary>
    /// Replaces all occurrences of the specified old values with the specified new value in the
    /// given string.
    /// </summary>
    public static string ReplaceAll(this string value, in IEnumerable<string> oldValues, string newValue)
        => oldValues.Aggregate(value, (current, oldValue) => current.Replace(oldValue, newValue));

    /// <summary>
    /// Replaces all occurrences of the specified old values with the specified new values in the
    /// given string.
    /// </summary>
    public static string ReplaceAll(this string value, in IEnumerable<(string OldValue, string NewValue)> items)
        => items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));

    /// <summary>
    /// Replaces all occurrences of a character in a string with a new character.
    /// </summary>
    public static string ReplaceAll(this string value, params (char OldValue, char NewValue)[] items)
        => items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));

    /// <summary>
    /// Replaces all occurrences of the specified old values with the specified new values in the
    /// given string.
    /// </summary>
    public static string ReplaceAll(this string value, params (string OldValue, string NewValue)[] items)
        => items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));

    /// <summary>
    /// Separates a string into separate words based on the provided separators.
    /// </summary>
    /// <param name="str">The string to separate.</param>
    /// <param name="separators">The separators to use.</param>
    /// <returns>The separated string.</returns>
    [return: NotNullIfNotNull(nameof(str))]
    public static string? Separate(this string? str, params char[] separators)
    {
        //If no separators are provided, use default separators
        if (separators?.Any() is not true)
        {
            separators = _standardSeparators;
        }
        //If the string is empty, return it
        if (str.IsNullOrEmpty())
        {
            return str;
        }

        //Create a StringBuilder to store the new string
        var sb = new StringBuilder();
        //Loop through each character in the string
        for (var i = 0; i < str.Length; i++)
        {
            //Determine if the character is a separator and if it should be ignored
            var (isSeparator, shouldIgnore) = determineSeparator(str[i], separators);
            //If the character is a separator and it is not the first character, add a space
            if (i > 0 && isSeparator)
            {
                _ = sb.Append(' ');
            }
            //If the character should not be ignored, add it to the StringBuilder
            if (!shouldIgnore)
            {
                _ = sb.Append(str[i]);
            }
        }

        //Return the new string
        return sb.ToString();

        //Function to determine if the character is a separator and if it should be ignored
        static (bool IsSeparator, bool ShouldIgnore) determineSeparator(char c, char[] separators)
        => separators.Contains(c) ? (true, true) : (char.IsUpper(c), false);
    }

    /// <summary>
    /// Separates a camel case string into separate words.
    /// </summary>
    /// <param name="value">The string to separate.</param>
    /// <returns>The separated string, or null if the input was null.</returns>
    [return: NotNullIfNotNull(nameof(value))]
    public static string? SeparateCamelCase(this string? value)
        => value.SplitCamelCase().Merge(" ");

    /// <summary>
    /// Replaces a phrase in a string with a new string.
    /// </summary>
    /// <param name="str">The string to modify.</param>
    /// <param name="index">The index of the phrase to replace.</param>
    /// <param name="newStr">The new string to replace the phrase with.</param>
    /// <param name="start">The start character of the phrase.</param>
    /// <param name="end">The end character of the phrase.</param>
    /// <returns>The modified string, or null if the phrase was not found.</returns>
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

    /// <summary>
    /// Creates a new string from a slice of the given string.
    /// </summary>
    /// <param name="s">The string to slice.</param>
    /// <param name="start">The start index of the slice.</param>
    /// <param name="length">
    /// The length of the slice. If not specified, the slice will extend to the end of the string.
    /// </param>
    /// <returns>A new string from the sliced span.</returns>
    public static string Slice(this string s, in int start, in int? length = null)
    {
        // Create a read-only span of characters from the string
        ReadOnlySpan<char> span = s;

        // If a length is specified, slice the span from the start index to the length Otherwise,
        // slice the span from the start index to the end of the string
        var slice = length is { } l ? span.Slice(start, l) : span[start..];

        // Return a new string from the sliced span
        return new(slice);
    }

    /// <summary>
    /// Creates a string of a given length filled with spaces.
    /// </summary>
    /// <param name="count">The length of the string to create.</param>
    /// <returns>A string of the given length filled with spaces.</returns>
    public static string Space(in int count)
        => new(' ', count);

    /// <summary>
    /// Splits a string into substrings based on a separator.
    /// </summary>
    /// <param name="this">The string to split.</param>
    /// <param name="separator">The separator to use for splitting.</param>
    /// <returns>A collection of substrings from the original string.</returns>
    public static IEnumerable<string> Split(this string @this, string separator)
    {
        //Create a checkpoint variable to keep track of the current index
        var checkpoint = 0;
        //Find the index of the separator in the string
        var indexOfSeparator = @this.IndexOf(separator);
        //Loop until the separator is no longer found
        while (indexOfSeparator >= 0)
        {
            //Create a substring from the checkpoint to the index of the separator
            var result = @this[checkpoint..indexOfSeparator];
            //Yield the result
            yield return result;
            //Update the checkpoint to the index of the separator plus the length of the separator
            checkpoint = indexOfSeparator + separator.Length;
            //Find the next index of the separator, starting from the checkpoint
            indexOfSeparator = @this.IndexOf(separator, checkpoint);
        }
    }

    [Obsolete("Subject to delete", true)]
    public static IEnumerable<string> Split(this string value, int groupSize)
    {
        Check.MustBeArgumentNotNull(value);

        for (var x = 0; x < value.Length; x += groupSize)
        {
            yield return value.Slice(x, Math.Min(groupSize, value.Length - x));
        }
    }

    /// <summary>
    /// Splits a camel case string into separate words.
    /// </summary>
    /// <param name="value">The string to split.</param>
    /// <returns>An IEnumerable of strings.</returns>
    public static IEnumerable<string> SplitCamelCase(this string? value)
    //This code is used to split a camel case string into separate words.
    //It takes a string as an argument and returns an IEnumerable of strings.
    {
        //Check if the value is null
        if (value == null)
        {
            //If it is, return an empty IEnumerable
            yield break;
        }

        //Create a StringBuilder to store the words
        StringBuilder sb = new();
        //Loop through each character in the string
        foreach (var c in value)
        {
            //Check if the character is uppercase
            if (char.IsUpper(c))
            {
                //If the StringBuilder is not empty, add the word to the IEnumerable
                if (sb.Length > 0)
                {
                    yield return sb.ToString();
                }

                //Clear the StringBuilder
                _ = sb.Clear();
            }
            //Append the character to the StringBuilder
            _ = sb.Append(c);
        }
        //If the StringBuilder is not empty, add the word to the IEnumerable
        if (sb.Length > 0)
        {
            yield return sb.ToString();
        }
    }

    /// <summary>
    /// Splits a string into parts and merges them with start and end separators.
    /// </summary>
    /// <param name="str">The string to split and merge.</param>
    /// <param name="splitter">The character to split the string by.</param>
    /// <param name="startSeparator">The string to add to the start of each part.</param>
    /// <param name="endSeparator">The string to add to the end of each part.</param>
    /// <returns>The merged string.</returns>
    public static string SplitMerge(this string str, char splitter, in string startSeparator, in string endSeparator)
    {
        StringBuilder result = new();
        foreach (var part in str.Split(splitter))
        {
            _ = result.Append($"{startSeparator}{part}{endSeparator}");
        }

        return result.ToString();
    }

    /// <summary>
    /// Splits a string into a sequence of key-value pairs. (To be used in `ConnectionString`-like strings)
    /// </summary>
    /// <param name="str">The string to split.</param>
    /// <param name="keyValueSeparator">The separator used to separate the key and value.</param>
    /// <param name="statementSeparator">The separator used to separate the key-value pairs.</param>
    /// <returns>A sequence of key-value pairs.</returns>
    public static IEnumerable<(string Key, string Value)> SplitPair([DisallowNull] string str, [DisallowNull] string keyValueSeparator = "=", [DisallowNull] string statementSeparator = ";")
    {
        // Check if the arguments are not null
        Check.MustBeArgumentNotNull(str);
        Check.MustBeArgumentNotNull(keyValueSeparator);
        Check.MustBeArgumentNotNull(statementSeparator);

        // Split the string into key-value pairs
        var keyValuePairs = str.Split(statementSeparator).Compact();

        // Iterate through each key-value pair
        foreach (var keyValuePair in keyValuePairs)
        {
            // Split the key-value pair into key and value
            var keyValue = keyValuePair.Split(keyValueSeparator);

            // Return the key-value pair
            yield return (keyValue[0], keyValue[1]);
        }
    }

    /// <summary>
    /// Converts a string from SQL encoding to UTF-8 encoding.
    /// </summary>
    public static string SqlEncodingToUtf(this string obj)
        => Encoding.UTF8.GetString(Encoding.GetEncoding(1256).GetBytes(obj));

    /// <summary>
    /// Checks if the given string starts with any of the strings in the given IEnumerable.
    /// </summary>
    public static bool StartsWithAny(this string str, in IEnumerable<string> values)
        => values.Any(str.StartsWith);

    /// <summary>
    /// Checks if a string starts with any of the given values.
    /// </summary>
    public static bool StartsWithAny(this string str, params string[] values)
        => values.Any(str.StartsWith);

    /// <summary>
    /// Converts a string to a byte array using the specified encoding.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <param name="encoding">The encoding to use.</param>
    /// <returns>A byte array containing the converted string.</returns>
    public static byte[] ToBytes([DisallowNull] this string value, [DisallowNull] in Encoding encoding)
    {
        Check.MustBeArgumentNotNull(value);
        Check.MustBeArgumentNotNull(encoding);

        return encoding.GetBytes(value);
    }

    /// <summary>
    /// Converts a string to its hexadecimal representation.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    /// <returns>The hexadecimal representation of the string.</returns>
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

    /// <summary>
    /// Converts a collection of strings to a collection of integers.
    /// </summary>
    public static IEnumerable<int> ToInt(in IEnumerable<string> array)
        => array.Where(str => IsNumber(str)).Select(str => str.Cast().ToInt());

    /// <summary>
    /// Converts a collection of strings to lowercase.
    /// </summary>
    public static IEnumerable<string> ToLower(this IEnumerable<string> strings)
        => strings.Select(str => str.ToLower());

    /// <summary>
    /// Converts a string to Unicode encoding.
    /// </summary>
    public static string? ToUnicode(this string? str, Encoding? current = null)
        => str is null ? null : Encoding.Unicode.GetString((current ?? Encoding.Unicode).GetBytes(str));

    /// <summary>
    /// Trims all strings in the given IEnumerable.
    /// </summary>
    public static IEnumerable<string> TrimAll(this IEnumerable<string> values)
        => values.Select(t => t.Trim());

    /// <summary>
    /// Trims all strings in an IEnumerable using the specified characters.
    /// </summary>
    public static IEnumerable<string> TrimAll(this IEnumerable<string> values, params char[] trimChars)
        => values.Select(t => t.Trim(trimChars));

    [return: NotNullIfNotNull(nameof(s))]
    public static string? TrimEnd(this string? s, string trim) =>
        s == null ? null : s.EndsWith(trim) ? s[..^trim.Length] : s;

    /// <summary>
    /// This method tries to get the count of a given character in a string from a given index.
    /// </summary>
    public static TryMethodResult<int> TryCountOf(this string str, char c, int index)
        => CatchFunc(() => str.CountOf(c, index)).Fluent().WithNew(x => TryMethodResult<int>.TryParseResult(x.Exception is null, x.Result));
}
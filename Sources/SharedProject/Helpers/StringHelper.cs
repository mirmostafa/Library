// Last update on 2018/07/23 by Mohammad Mirmostafa 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Mohammad.Collections.Generic;
using Mohammad.Text;
using static Mohammad.Helpers.CodeHelper;

namespace Mohammad.Helpers
{
    /// <summary>
    ///     A utility to do some common tasks about strings
    /// </summary>
    public static class StringHelper
    {
        public static readonly char DecimalSeparator = '⁄';

        public static readonly IEnumerable<char> PersianChars = new[]
        {
            'آ', 'ا', 'ب', 'پ', 'ت', 'ث', 'ج', 'چ', 'ح', 'خ', 'د', 'ذ', 'ر', 'ز', 'ژ', 'س', 'ش', 'ص', 'ض', 'ط', 'ظ', 'ع', 'غ', 'ف', 'ق', 'ک',
            'گ', 'ل', 'م', 'ن', 'و', 'ه', 'ی'
        };

        private static readonly IEnumerable<(char Persian, char English)> _Numbers = new[]
        {
            ('۰', '0'), ('۱', '1'), ('۲', '2'), ('۳', '3'), ('۴', '4'), ('۵', '5'), ('۶', '6'), ('۷', '7'), ('۸', '8'), ('۹', '9')
        };

        public static readonly IReadOnlyList<char> PersianDigits = _Numbers.Select(n => n.Persian).ToList();

        public static readonly IEnumerable<char> PersianSpecialChars = new[]
        {
            'ة', 'ي', 'ئ', 'ژ', 'ؤ', 'إ', 'أ', 'ء', '؛', '،', 'ٍ', 'ٌ', 'ً', 'َ', 'ُ', 'ِ', 'ّ', 'ّ', 'ۀ', 'آ', 'ـ', 'ك'
        };

        public static readonly IEnumerable<char> SeparatorsChars = new[] {',', DecimalSeparator};

        [Obsolete("MOHAMMAD: Please use the other overload please.")]
        public static string GetPhrase(this string str, char start, char end, int index) => GetPhrase(str, index, start, end);

        public static string Add(this string s, string s1) => string.Concat(s, s1);

        public static string Add(this string s, int count, char add = ' ', bool before = false)
        {
            if (count == 0)
            {
                return s;
            }

            ArgHelper.AssertBiggerThan(count, 0, "count");
            return before ? string.Concat(add.ToString().Repeat(count), s) : string.Concat(s, add.ToString().Repeat(count));
        }

        public static string Enlarge(this string s, int count, char add = ' ') => s.Add(count - s.Length, add);

        public static IEnumerable<int> AllIndexesOf(this string str, string value, bool ignoreCase = false)
        {
            var buffer = ignoreCase ? str.ToLower() : str;
            var stat = ignoreCase ? value.ToLower() : value;
            var result = 0;
            int currentIndex;
            while ((currentIndex = buffer.IndexOf(stat, StringComparison.Ordinal)) != -1)
            {
                result += currentIndex;
                yield return result;
                result += stat.Length;
                buffer = buffer.Substring(currentIndex + stat.Length);
            }
        }

        public static bool AnyCharInString(this string str, string range) => range.Any(c => str.Contains(c.ToString()));

        public static IEnumerable<string> Compact(this IEnumerable<string> strings) => strings.Where(item => !IsNullOrEmpty(item));

        public static string[] Compact(params string[] strings) => strings.Where(item => !item.IsNullOrEmpty()).ToArray();

        public static int CompareTo(this string str1, string str, bool ignoreCase = false) => string.Compare(str1, str, ignoreCase);

        public static string ConcatStrings(this IEnumerable<string> values) => string.Concat(values.ToArray());

        public static bool Contains(this IEnumerable<string> array, string str, bool ignoreCase) => array.Any(s => s.CompareTo(str, ignoreCase) == 0);

        public static bool Contains(string str, string value, bool ignoreCase = true) => ignoreCase
            ? str.ToLowerInvariant().Contains(value.ToLowerInvariant())
            : str.Contains(value);

        public static bool ContainsAny(this string str, IEnumerable<object> array) => array.Any(item => str.Contains(item.ToString()));

        public static bool ContainsAny(this string str, params object[] array) => str.ContainsAny(array.AsEnumerable());

        public static bool ContainsOf(this string str, string target) => str.ToLower().Contains(target.ToLower());

        public static char ConvertPersianDigitToEnglishDigit(this char c)
        {
            for (var i = 0; i < 9; i++)
            {
                if (c == PersianDigits[i])
                {
                    return char.Parse(i.ToString());
                }
            }

            return c;
        }

        public static string ConvertPersianNumberToEnglishNumber(this string text)
        {
            var buffer = new StringBuilder(text.Length);
            foreach (var c in text)
            {
                _ = buffer.Append(ConvertPersianDigitToEnglishDigit(c));
            }

            return buffer.ToString();
        }

        public static string CorrectNumberFormat(string text)
        {
            var result = text;
            result = result.Replace(DecimalSeparator.ToString(), ".");
            result = SeparatorsChars.Aggregate(result, (current, separatorsChar) => current.Replace(separatorsChar.ToString(), ""));
            for (var i = 0; i < 9; i++)
            {
                result = result.Replace(PersianDigits[i].ToString(), i.ToString());
            }

            if (result.IndexOf('.') >= 0 && result.Substring(result.IndexOf('.') + 1).All(c => c == '0'))
            {
                result = result.Substring(0, result.IndexOf('.'));
            }

            return result;
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

        public static bool EndsWithAny(this string str, IEnumerable<object> array) => array.Any(item => str.EndsWith(item.ToString()));

        public static bool EndsWithAny(this string str, params object[] array) => str.EndsWithAny(array.AsEnumerable());

        public static bool EndsWithAny(this string str, IEnumerable<string> values) => values.Any(str.EndsWith);

        public static bool EqualsTo(this string str1, string str, bool ignoreCase = true) => str1.CompareTo(str, ignoreCase) == 0;

        public static bool EqualsToAny(this string str1, bool ignoreCase, params string[] array) => array.Any(s => str1.EqualsTo(s, ignoreCase));

        public static bool EqualsToAny(this string str1, params string[] array) => array.Any(s => str1.EqualsTo(s));

        public static IEnumerable<(string Key, string Value)> GetKeyValues(this string keyValueStr, char keyValueSeparator = '=', char separator = ';')
            => keyValueStr.Split(separator).Select(raw => raw.Split(keyValueSeparator)).Select(keyValue => (keyValue[0], keyValue[1]));

        public static string GetLongest(this string[] strings) => strings.Max();

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
            var (lenOfStart, succeed1) = str.TryLengthOf(start, index * 2);
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
                var (lenOfEnd, succeed2) = str.TryLengthOf(end, index * 2);
                if (!succeed2)
                {
                    return null;
                }

                endIndex = index != 0 ? lenOfEnd + 1 : str.IndexOf(end, 0) + 1;
                len = endIndex - startIndex;
            }

            var result = buffer.Substring(startIndex, len);
            return result;
        }

        public static IEnumerable<string> GetPhrases(this string str, char start, char end = default)
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

        public static bool HasPersian(this string text) => CheckAnyValidations(text, IsPersian);

        public static string IfNullOrEmpty(this string str, string defaultValue) => str.IsNullOrEmpty() ? defaultValue : str;

        public static int? IndexOf(this IEnumerable<string> array, string item, bool trimmed = false, bool ignoreCase = true)
        {
            var buffer = array.ToCollection();

            for (var counter = 0; counter != buffer.Count; counter++)
            {
                if (string.Compare(trimmed ? buffer[counter].Trim() : buffer[counter], trimmed ? item.Trim() : item, ignoreCase) == 0)
                {
                    return counter;
                }
            }

            return null;
        }

        public static bool IsCommon(this char c) => c == ' ';

        public static bool IsDigit(this char c, bool canAcceptMinusKey = false) => canAcceptMinusKey && c == '-' || char.IsDigit(c);

        public static bool IsDigitOrControl(this char key) => char.IsDigit(key) || char.IsControl(key);

        public static bool IsEnglish(this string text) => CheckAllValidations(text, IsEnglish);

        public static bool IsEnglish(this char c) => IsCommon(c) || char.ToUpper(c) >= 'A' && char.ToUpper(c) <= 'Z';

        public static bool IsEnglishOrNumber(this string text, bool canAcceptMinusKey = false)
            => CheckAllValidations(text, c => IsDigit(c, canAcceptMinusKey) || IsEnglish(c));

        public static bool IsInRange(this string text, params string[] range) => IsInRange(text, true, range);

        public static bool IsInRange(this string text, bool ignoreCase, params string[] range) => IsInRange(text, false, ignoreCase, range);

        public static bool IsInRange(this string text, bool trimmed, bool ignoreCase, params string[] range) => range.IndexOf(text, trimmed, ignoreCase) >= 0;

        public static bool IsInRange(this string text, bool trimmed, bool ignoreCase, IEnumerable<string> range) => range.IndexOf(text, trimmed, ignoreCase) >= 0;

        public static bool IsInString(this char c, string text) => text.Contains(c.ToString());

        public static bool IsInteger(this string text) => int.TryParse(text, out var _);

        public static bool IsLetter(this char c) => IsEnglish(c) || IsPersian(c);

        public static bool IsLetterText(this string text) => CheckAllValidations(text, c => IsEnglish(c) || IsPersian(c));

        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        public static bool IsNullOrEmpty(object o) => o?.ToString().IsNullOrEmpty() != false;

        public static bool IsNumber(this string text) => float.TryParse(text, out var _);

        public static bool IsPersian(this string text) => CheckAllValidations(text, IsPersian);

        public static bool IsPersian(this char c) => IsCommon(c) || PersianChars.Any(pc => pc == c) || PersianSpecialChars.Any(pc => pc == c);

        public static bool IsPersianDigit(this char c) => PersianDigits.Any(pd => c == pd);

        public static bool IsPersianOrNumber(this string text, bool canAcceptMinusKey) => CheckAllValidations(text, c => IsDigit(c, canAcceptMinusKey) || IsPersian(c));

        public static bool IsUnicode(this string str)
        {
            var unicodeBytes = Encoding.Unicode.GetBytes(str);
            for (var i = 1; i < unicodeBytes.Length; i += 2)
            {
                if (unicodeBytes[i] != 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static string JoinStrings(this IEnumerable<string> values, string separator)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return string.Join(separator, values.ToArray());
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

        public static int LengthOf(this string str, char c, int index)
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
            var result = array.Aggregate(string.Empty, (current, str) => current + (quatStart + str) + quatEnd + separator + " ").Trim();
            return result.Substring(0, result.Length - 1);
        }

        public static string Merge(this IEnumerable<string> array) => array.Merge(", ");

        public static string Merge(this IEnumerable<string> array, string separator) => string.Join(separator, array.ToArray());

        public static string Merge(string quat, string separator, params string[] array) => array.Merge(quat, separator);

        public static string Merge(this IEnumerable<string> array, string quat, string separator)
        {
            var result = array.Aggregate(string.Empty, (current, str) => $"{current}{quat}{str}{quat}{separator} ").Trim();
            return result.Substring(0, result.Length - 1);
        }

        public static string SplitMerge(this string str, char splitter, string startSeparator, string endSeparator)
        {
            var result = new StringBuilder();
            foreach (var part in str.Split(splitter))
            {
                _ = result.Append($"{startSeparator}{part}{endSeparator}");
            }

            return result.ToString();
        }

        public static string RemoveFromEnd(this string str, int count) => str.Substring(0, str.Length - count);

        public static string MergePair(this IEnumerable<PairValue<string, string>> splitPair, string keyValueSeparator = "=", string statementSeparator = ";")
        {
            var result = new StringBuilder();
            foreach (var pair in splitPair)
            {
                _ = result.Append(string.Concat(pair.Value1, keyValueSeparator, pair.Value2, statementSeparator));
            }

            return result.ToString();
        }

        public static bool NotEndsWithAny(this string str, IEnumerable<object> array) => array.Any(item => !str.EndsWith(item.ToString()));

        public static bool NotEndsWithAny(this string str, params object[] array) => str.NotEndsWithAny(array.AsEnumerable());

        public static string PatternPolicy(string text)
        {
            var result = string.Empty;
            var t = CorrectUnicodeProblem(text);

            char[] outChar =
            {
                '~', '`', '!', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '{', '[', '}', ']', ':', ';', '\'', '"', '<', ',', '>',
                '?', '|', '\\'
            };

            var arrayString = t.Split(outChar);

            return arrayString.Aggregate(result, (current, s) => current + s);
        }

        public static string Remove(this string str, string str1) => str.Replace(str1, "");

        public static string RemoveFromEnd(this string str, string oldValue)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }

            return str.EndsWith(oldValue) ? str.Substring(0, str.Length - oldValue.Length) : str;
        }

        public static string Repeat(this string text, int count)
        {
            ArgHelper.AssertBiggerThan(count, 0, "count");
            var result = new StringBuilder(string.Empty);
            for (var counter = 0; counter < count; counter++)
            {
                _ = result.Append(text);
            }

            return result.ToString();
        }

        public static string ReplaceEx(this string s, string old, string replacement, int count = 1) => new Regex(Regex.Escape(old)).Replace(s, replacement, count);

        public static string ReplaceEx(this string s, char old, char replacement, int count = 1) => ReplaceEx(s, old.ToString(), replacement.ToString(), count);

        public static string SeparateCamelCase(this string str)
        {
            var i = 1;
            while (i < str.Length)
            {
                if (char.IsUpper(str[i]) && (!char.IsUpper(str[i - 1]) || i < str.Length - 1 && !char.IsUpper(str[i + 1])))
                {
                    str = str.Insert(i, " ");
                    i += 1;
                }

                i += 1;
            }

            return str;
        }

        public static string SetPhrase(this string str, int index, string newStr, char start, char end = default)
        {
            var startStr = str.Substring(0, index + 1);
            var endEnd = str.Substring(index + GetPhrase(str, index, start, end).Length);
            var result = string.Concat(startStr, newStr, endEnd);
            return result;
        }

        public static string Space(int count) => "".Add(count);

        public static IEnumerable<string> Split(this string value, int groupSize)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            for (var x = 0; x < value.Length; x += groupSize)
            {
                yield return value.Substring(x, Math.Min(groupSize, value.Length - x));
            }
        }

        public static IEnumerable<string> SplitCamelCase(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var lastWord = 0;

            for (var x = lastWord + 1; x < value.Length; x++)
            {
                if (!char.IsUpper(value, x))
                {
                    continue;
                }

                yield return value.Substring(lastWord, x - lastWord);
                lastWord = x;
            }

            if (lastWord <= value.Length)
            {
                yield return value.Substring(lastWord, value.Length - lastWord);
            }
        }

        public static IEnumerable<PairValue<string, string>> SplitPair(string str, string keyValueSeparator = "=", string statementSeparator = ";")
        {
            var split = str.Split(new[] {keyValueSeparator, statementSeparator}, StringSplitOptions.None);
            for (var i = 0; i < split.Length; i += 2)
            {
                yield return new PairValue<string, string>(split[i], split[i + 1]);
            }
        }

        public static string SqlEncodingToUtf(this string obj) => Encoding.UTF8.GetString(Encoding.GetEncoding(1256).GetBytes(obj));

        public static bool StartsWithAny(this string str, IEnumerable<string> values) => values.Any(str.StartsWith);

        public static byte[] ToBytes(this string value, Encoding encoding)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            return encoding.GetBytes(value);
        }

        public static string ToHex(string str)
        {
            var sb = new StringBuilder();
            var bytes = IsUnicode(str) ? Encoding.Unicode.GetBytes(str) : Encoding.ASCII.GetBytes(str);
            foreach (var b in bytes)
            {
                _ = sb.Append(b.ToString("X"));
            }

            return sb.ToString();
        }

        public static IEnumerable<int> ToInt(this IEnumerable<string> array) => array.Where(str => str.IsNumber()).Select(str => str.ToInt());

        public static IEnumerable<string> ToLower(this IEnumerable<string> strings) => strings.ForEachFunc(str => str.ToLower());

        public static string ToUnicode(this string str) => Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(str));

        public static IEnumerable<string> Trim(this IEnumerable<string> strings) => strings.Where(item => !IsNullOrEmpty(item));

        public static IEnumerable<string> TrimAll(this IEnumerable<string> values, params char[] trimChars) => values.Select(t => t.Trim(trimChars));

        public static IEnumerable<string> TrimAll(this IEnumerable<string> values) => values.Select(t => t.Trim());

        public static string Truncate(this string value, int length) => value?.Substring(0, value.Length - 1);

        public static (int Result, bool Succeed) TryLengthOf(this string str, char c, int index)
        {
            var result = CatchFunc(() => LengthOf(str, c, index), out var ex);
            return (result, ex == null);
        }

        public static string ReplaceAll(this string value, in IEnumerable<(string OldValue, string NewValue)> items) =>
            items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));

        public static string ReplaceAll(this string value, in IEnumerable<(char OldValue, char NewValue)> items) =>
            items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));

        public static string ReplaceAll(this string value, params (char OldValue, char NewValue)[] items) =>
            items.Aggregate(value, (current, item) => current.Replace(item.OldValue, item.NewValue));

        public static string ToEnglishDigits(this string value) => value.ReplaceAll(_Numbers.Select(n => (n.Persian, n.English)));
        public static string ToPersianDigits(this string value) => value.ReplaceAll(_Numbers.Select(n => (n.English, n.Persian)));

        public static string ToCulturalNumber(this string value, in bool correctPersianChars, in NumberCulture toNumberCulture = NumberCulture.None)
        {
            value = toNumberCulture switch
            {
                NumberCulture.Persian => value.ToPersianDigits(),
                NumberCulture.English => value.ToEnglishDigits(),
                _ => value
            };

            if (correctPersianChars)
            {
                value = value.ReplaceAll(((char)1610, (char)1740), ((char)1603, (char)1705)); // ی, ک
            }

            return value;
        }

        public static string? IfNull(this string? s, in string? value) => string.IsNullOrWhiteSpace(s) ? value : s;
        public static string? NullIfEmpty(this string? s) => string.IsNullOrEmpty(s) ? null : s;
        internal static bool CheckAllValidations(in string text, in Func<char, bool> regularValidate) => text.All(regularValidate);
        internal static bool CheckAnyValidations(in string text, in Func<char, bool> regularValidate) => text.Any(regularValidate);
    }
}
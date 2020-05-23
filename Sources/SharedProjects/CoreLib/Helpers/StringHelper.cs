#region Code Identifications

// Created on     2018/03/11
// Last update on 2018/03/11 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mohammad.Collections.Generic;
using static Mohammad.Helpers.CodeHelper;

namespace Mohammad.Helpers
{
    /// <summary>
    ///     A utility to do some common tasks about strings
    /// </summary>
    public static class StringHelper
    {
        public static readonly char[] SeperatorsChars = { ',', DecimalSeperator };
        public static readonly char[] PersianDigits = { '٠', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩' };

        public static readonly char[] PersianSpecialChars =
        {
            'ة',
            'ي',
            'ئ',
            'ژ',
            'ؤ',
            'إ',
            'أ',
            'ء',
            '؛',
            '،',
            'ٍ',
            'ٌ',
            'ً',
            'َ',
            'ُ',
            'ِ',
            'ّ',
            'ّ',
            'ۀ',
            'آ',
            'ـ',
            'ك'
        };

        public static readonly char DecimalSeperator = '⁄';

        //‍‌‍58700687314605
        public static readonly char[] PersianChars =
        {
            'آ',
            'ا',
            'ب',
            'پ',
            'ت',
            'ث',
            'ج',
            'چ',
            'ح',
            'خ',
            'د',
            'ذ',
            'ر',
            'ز',
            'ژ',
            'س',
            'ش',
            'ص',
            'ض',
            'ط',
            'ظ',
            'ع',
            'غ',
            'ف',
            'ق',
            'ک',
            'گ',
            'ل',
            'م',
            'ن',
            'و',
            'ه',
            'ی'
        };

        public static bool AnyCharInString(this string str, string range) => range.Any(c => str.Contains(c.ToString()));
        public static IEnumerable<string> Compact(this IEnumerable<string> strings) => strings.Where(item => !IsNullOrEmpty(item));
        public static IEnumerable<string> Trim(this IEnumerable<string> strings) => strings.Where(item => !IsNullOrEmpty(item));
        public static string[] Compact(params string[] strings) => strings.Where(item => !item.IsNullOrEmpty()).ToArray();
        public static int CompareTo(this string str1, string str, bool ignoreCase = false) => string.Compare(str1, str, ignoreCase);
        public static bool ContainsAny(this string str, IEnumerable<object> array) => array.Any(item => str.Contains(item.ToString()));
        public static bool ContainsAny(this string str, params object[] array) => str.ContainsAny(array.AsEnumerable());
        public static bool EndsWithAny(this string str, IEnumerable<object> array) => array.Any(item => str.EndsWith(item.ToString()));
        public static bool EndsWithAny(this string str, params object[] array) => str.EndsWithAny(array.AsEnumerable());
        public static bool EqualsTo(this string str1, string str, bool ignoreCase = true) => str1.CompareTo(str, ignoreCase) == 0;
        public static bool EqualsToAny(this string str1, bool ignoreCase, params string[] array) => array.Any(s => str1.EqualsTo(s, ignoreCase));
        public static bool EqualsToAny(this string str1, params string[] array) => array.Any(s => str1.EqualsTo(s));
        public static bool ContainsOf(this string str, string target) => str.ToLower().Contains(target.ToLower());

        public static int? IndexOf(this IEnumerable<string> array, string item, bool trimmed = false, bool ignoreCase = true)
        {
            var buffer = array.ToCollection();

            for (var counter = 0; counter != buffer.Count; counter++)
                if (string.Compare(trimmed ? buffer[counter].Trim() : buffer[counter], trimmed ? item.Trim() : item, ignoreCase) == 0)
                    return counter;
            return null;
        }

        public static bool IsDigitOrControl(this char key) => char.IsDigit(key) || char.IsControl(key);

        public static string Merge(string quatStart, string quatEnd, string separator, params object[] array)
        {
            var result = array.Aggregate(string.Empty, (current, str) => current + (quatStart + str) + quatEnd + separator + " ").Trim();
            return result.Substring(0, result.Length - 1);
        }

        public static IEnumerable<string> Split(this string value, int groupSize)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            for (var x = 0; x < value.Length; x += groupSize)
                yield return value.Substring(x, Math.Min(groupSize, value.Length - x));
        }

        public static string JoinStrings(this IEnumerable<string> values, string separator)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return string.Join(separator, values.ToArray());
        }

        public static string ConcatStrings(this IEnumerable<string> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return string.Concat(values.ToArray());
        }

        public static IEnumerable<string> TrimAll(this IEnumerable<string> values, params char[] trimChars) => values.Select(t => t.Trim(trimChars));
        public static IEnumerable<string> TrimAll(this IEnumerable<string> values) => values.Select(t => t.Trim());

        public static string ToStringJoined<T>(this IEnumerable<T> values, string separator)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            var strings = values.Select(v => v.ToString());
            return string.Join(separator, strings.ToArray());
        }

        public static string ToStringConcat<T>(this IEnumerable<T> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            var strings = values.Select(v => v.ToString());
            return string.Concat(strings.ToArray());
        }

        public static string RemoveFromEnd(this string str, string oldValue)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            if (oldValue == null)
                throw new ArgumentNullException(nameof(oldValue));

            return str.EndsWith(oldValue) ? str.Substring(0, str.Length - oldValue.Length) : str;
        }

        public static IEnumerable<string> LazySplit(this string str, char separator)
        {
            var buffer = new StringBuilder();
            foreach (var c in str)
                if (c == separator)
                {
                    yield return buffer.ToString();
                    buffer.Clear();
                }
                else
                    buffer.Append(c);
        }

        public static IEnumerable<string> GroupBy(this IEnumerable<char> chars, char separator)
        {
            var buffer = new StringBuilder();
            foreach (var c in chars)
                if (c == separator)
                {
                    yield return buffer.ToString();
                    buffer.Clear();
                }
                else
                    buffer.Append(c);
        }

        public static IEnumerable<string> SplitCamelCase(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var lastWord = 0;

            for (var x = lastWord + 1; x < value.Length; x++)
            {
                if (!char.IsUpper(value, x))
                    continue;
                yield return value.Substring(lastWord, x - lastWord);
                lastWord = x;
            }

            if (lastWord <= value.Length)
                yield return value.Substring(lastWord, value.Length - lastWord);
        }

        public static byte[] ToBytes(this string value, Encoding encoding)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            return encoding.GetBytes(value);
        }

        public static string GetLongest(this string[] strings) => strings.Max();

        public static string AddSpace(this string s, int count)
        {
            ArgHelper.AssertBiggerThan(count, 0, "count");
            return string.Concat(s, " ".Repeat(count));
        }

        public static bool IsInRange(this string text, params string[] range) => IsInRange(text, true, range);
        public static bool IsInRange(this string text, bool ignoreCase, params string[] range) => IsInRange(text, false, ignoreCase, range);
        public static bool IsInRange(this string text, bool trimmed, bool ignoreCase, params string[] range) => range.IndexOf(text, trimmed, ignoreCase) >= 0;

        public static bool IsInRange(this string text, bool trimmed, bool ignoreCase, IEnumerable<string> range) => range.IndexOf(text, trimmed, ignoreCase) >= 0;

        public static bool IsInString(this char c, string text) => text.Contains(c.ToString());

        public static bool IsInteger(this string text) => int.TryParse(text, out int num);

        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
        public static string IfNullOrEmpty(this string str, string defaultValue) => str.IsNullOrEmpty() ? defaultValue : str;

        public static bool IsNumber(this string text) => float.TryParse(text, out float num);

        public static bool IsUnicode(this string str)
        {
            var unicodeBytes = Encoding.Unicode.GetBytes(str);
            for (var i = 1; i < unicodeBytes.Length; i += 2)
                if (unicodeBytes[i] != 0)
                    return true;
            return false;
        }

        public static string Merge(this IEnumerable<string> array) => array.Merge(", ");
        public static string Merge(this IEnumerable<string> array, string separator) => string.Join(separator, array.ToArray());
        public static string Merge(string quat, string separator, params string[] array) => array.Merge(quat, separator);

        public static string Merge(this IEnumerable<string> array, string quat, string separator)
        {
            var result = array.Aggregate(string.Empty, (current, str) => current + quat + str + quat + separator + " ").Trim();
            return result.Substring(0, result.Length - 1);
        }

        public static bool NotEndsWithAny(this string str, IEnumerable<object> array) => array.Any(item => !str.EndsWith(item.ToString()));
        public static bool NotEndsWithAny(this string str, params object[] array) => str.NotEndsWithAny(array.AsEnumerable());

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

        public static string PatternPolicy(string text)
        {
            var result = string.Empty;

            var t = EncorrectUnicodeProblem(text);

            //char[] outChar = { '~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '=', '{', '[', '}', ']', ':', ';', '\'', '"', '<', ',', '>', '?', '/', '|', '\\' };
            char[] outChar =
            {
                '~',
                '`',
                '!',
                '#',
                '$',
                '%',
                '^',
                '&',
                '*',
                '(',
                ')',
                '-',
                '+',
                '=',
                '{',
                '[',
                '}',
                ']',
                ':',
                ';',
                '\'',
                '"',
                '<',
                ',',
                '>',
                '?',
                '|',
                '\\'
            };

            var arrayString = t.Split(outChar);

            return arrayString.Aggregate(result, (current, s) => current + s);
        }

        public static string Repeat(this string text, int count)
        {
            ArgHelper.AssertBiggerThan(count, 0, "count");
            var result = new StringBuilder(string.Empty);
            for (var counter = 0; counter < count; counter++)
                result.Append(text);
            return result.ToString();
        }

        public static string EncorrectUnicodeProblem(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            text = text.Replace((char)1740, (char)1610);
            text = text.Replace((char)1705, (char)1603);
            return Encoding.UTF8.GetString(Encoding.Unicode.GetBytes(text));
        }

        public static string EncorrectNumberFormat(string text)
        {
            var result = text;
            result = result.Replace(DecimalSeperator.ToString(), ".");
            result = SeperatorsChars.Aggregate(result, (current, seperatorsChar) => current.Replace(seperatorsChar.ToString(), ""));
            for (var i = 0; i < 9; i++)
                result = result.Replace(PersianDigits[i].ToString(), i.ToString());
            if (result.IndexOf('.') >= 0 && result.Substring(result.IndexOf('.') + 1).All(c => c == '0'))
                result = result.Substring(0, result.IndexOf('.'));
            return result;
        }

        public static string SqlEncodingToUtf(this string obj) => Encoding.UTF8.GetString(Encoding.GetEncoding(1256).GetBytes(obj));

        public static string ToHex(string str)
        {
            var sb = new StringBuilder();
            var bytes = IsUnicode(str) ? Encoding.Unicode.GetBytes(str) : Encoding.ASCII.GetBytes(str);
            foreach (var b in bytes)
                sb.Append(b.ToString("X"));
            return sb.ToString();
        }

        public static IEnumerable<int> ToInt(IEnumerable<string> array) => array.Where(str => str.IsNumber()).Select(str => str.ToInt());
        public static string ToUnicode(this string str) => Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(str));
        public static bool EndsWithAny(this string str, IEnumerable<string> values) => values.Any(str.EndsWith);
        public static bool StartsWithAny(this string str, IEnumerable<string> values) => values.Any(str.StartsWith);

        public static IEnumerable<int> AllIndexesOf(this string str, string value, bool ignoreCase = false)
        {
            var buffer = ignoreCase ? str.ToLower() : str;
            var stat = ignoreCase ? value.ToLower() : value;
            var result = 0;
            int currIndex;
            while ((currIndex = buffer.IndexOf(stat, StringComparison.Ordinal)) != -1)
            {
                result += currIndex;
                yield return result;
                result += stat.Length;
                buffer = buffer.Substring(currIndex + stat.Length);
            }
        }

        public static bool Contains(this IEnumerable<string> array, string str, bool ignoreCase) => array.Any(s => s.CompareTo(str, ignoreCase) == 0);

        public static bool Contains(string str, string value, bool ignoreCase = true) => ignoreCase
            ? str.ToLowerInvariant().Contains(value.ToLowerInvariant())
            : str.Contains(value);

        public static string Remove(this string str, string str1) => str.Replace(str1, "");
        public static bool IsNullOrEmpty(object o) => o == null || o.ToString().IsNullOrEmpty();

        public static string MergePair(this IEnumerable<PairValue<string, string>> splitPair, string keyValueSparator = "=", string statementSparator = ";")
        {
            var result = new StringBuilder();
            foreach (var pair in splitPair)
                result.Append(string.Concat(pair.Value1, keyValueSparator, pair.Value2, statementSparator));
            return result.ToString();
        }

        public static IEnumerable<PairValue<string, string>> SplitPair(string str, string keyValueSparator = "=", string statementSparator = ";")
        {
            var split = str.Split(new[] { keyValueSparator, statementSparator }, StringSplitOptions.None);
            for (var i = 0; i < split.Length; i = i + 2)
                yield return new PairValue<string, string>(split[i], split[i + 1]);
        }

        public static IEnumerable<string> ToLower(this IEnumerable<string> strs) => strs.ForEachFunc(str => str.ToLower());

        public static string Truncate(this string value, int length) => value?.Substring(0, value.Length - 1);

        public static string Space(int count) => "".AddSpace(count);

        public static bool IsEnglishOrNumber(this string text, bool canAcceptMinusKey = false) => CheckAllValidations(text,
            c => IsDigit(c, canAcceptMinusKey) || IsEnglish(c));

        public static bool IsPersianOrNumber(this string text, bool canAcceptMinusKey) => CheckAllValidations(text,
            c => IsDigit(c, canAcceptMinusKey) || IsPersian(c));

        public static bool IsLetterText(this string text) => CheckAllValidations(text, c => IsEnglish(c) || IsPersian(c));

        public static bool IsEnglish(this string text) => CheckAllValidations(text, IsEnglish);

        public static bool IsPersian(this string text) => CheckAllValidations(text, IsPersian);

        public static bool HasPersian(this string text) => CheckAnyValidations(text, IsPersian);

        internal static bool CheckAllValidations(string text, Func<char, bool> regularValidate) => text.All(regularValidate);

        internal static bool CheckAnyValidations(string text, Func<char, bool> regularValidate) => text.Any(regularValidate);

        public static string ConvertPersianNumberToEnglishNumber(this string text)
        {
            var buffer = new StringBuilder(text.Length);
            foreach (var c in text)
                buffer.Append(ConvertPersianDigitToEnglishDigit(c));
            return buffer.ToString();
        }

        public static char ConvertPersianDigitToEnglishDigit(this char c)
        {
            for (var i = 0; i < 9; i++)
                if (c == PersianDigits[i])
                    return char.Parse(i.ToString());
            return c;
        }

        public static bool IsPersianDigit(this char c) => PersianDigits.Any(pd => c == pd);

        public static bool IsDigit(this char c, bool canAcceptMinusKey = false) => canAcceptMinusKey && c == '-' || char.IsDigit(c);

        public static bool IsLetter(this char c) => IsEnglish(c) || IsPersian(c);

        public static bool IsEnglish(this char c) => IsCommon(c) || char.ToUpper(c) >= 'A' && char.ToUpper(c) <= 'Z';

        public static bool IsPersian(this char c) => IsCommon(c) || PersianChars.Any(pc => pc == c) || PersianSpecialChars.Any(pc => pc == c);

        public static bool IsCommon(this char c) => c == ' ';

        [Obsolete("MOHAMMAD: Please use the other overload please.")]
        public static string GetPhrase(this string str, char start, char end, int index) => GetPhrase(str, index, start, end);

        public static string GetPhrase(this string str, int index, char start, char end = default(char))
        {
            if (end == default(char))
                end = start;
            var buffer = str;
            int startIndex;
            int endIndex;
            int len;
            var (succeed1, lenOfStart) = str.TryLengthOf(start, index * 2);
            if (!succeed1)
                return null;
            if (start == end)
            {
                startIndex = index != 0 ? lenOfStart + 1 : str.IndexOf(start, 0) + 1;
                endIndex = str.IndexOf(end, startIndex + 1);
                len = endIndex - startIndex;
            }
            else
            {
                startIndex = index != 0 ? lenOfStart + 1 : str.IndexOf(start, 0) + 1;
                var (succeed2, lenOfEnd) = str.TryLengthOf(end, index * 2);
                if (!succeed2)
                    return null;
                endIndex = index != 0 ? lenOfEnd + 1 : str.IndexOf(end, 0) + 1;
                len = endIndex - startIndex;
            }
            var result = buffer.Substring(startIndex, len);
            return result;
        }

        public static IEnumerable<(string Key, string Value)> GetKeyValues(this string keyvaluestr, char keyValueSeparator = '=', char separator = ';')
        {
            return keyvaluestr.Split(separator).Select(raw => raw.Split(keyValueSeparator)).Select(keyvalue => (keyvalue[0], keyvalue[1]));
        }

        public static IEnumerable<string> GetPhrases(this string str, char start, char end = default(char))
        {
            var index = 0;
            var result = GetPhrase(str, index, start, end);
            while (!result.IsNullOrEmpty())
            {
                yield return result;
                result = GetPhrase(str, ++index, start, end);
            }
        }

        public static string SetPhrase(this string str, int index, string newStr, char start, char end = default(char))
        {
            var startStr = str.Substring(0, index + 1);
            var endEnd = str.Substring(index + GetPhrase(str, index, start, end).Length);
            var result = string.Concat(startStr, newStr, endEnd);
            return result;
        }

        public static (bool Succeed, int Result) TryLengthOf(this string str, char c, int index)
        {
            var result = CatchFunc(() => LengthOf(str, c, index), out var ex);
            return (ex == null, result);
        }

        public static int LengthOf(this string str, char c, int index)
        {
            if (index == 0)
                return str.IndexOf(c);
            var foundCount = 0;
            index++;
            var i = 0;
            while (foundCount < index)
            {
                if (str[i] == c)
                    foundCount++;
                i++;
            }
            return i - 1;
        }
    }
}
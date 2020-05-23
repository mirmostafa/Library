#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
//using System.Web.UI;

namespace Library40.Helpers
{
	/// <summary>
	///     A utility to do some common tasks about strings
	/// </summary>
	public static class StringHelper
	{
		public static bool AnyCharInString(this string str, string range)
		{
			return range.Any(c => str.Contains(c.ToString()));
		}

		public static IEnumerable<string> Compact(this IEnumerable<string> strings)
		{
			return strings.Where(item => !IsNullOrEmpty(item));
		}

		public static string[] Compact(params string[] strings)
		{
			return strings.Where(item => !item.IsNullOrEmpty()).ToArray();
		}

		public static int CompareTo(this string str1, string str, bool ignoreCase = false)
		{
			return string.Compare(str1, str, ignoreCase);
		}

		public static bool ContainsAny(this string str, IEnumerable<object> array)
		{
			return array.Any(item => str.Contains(item.ToString()));
		}

		public static bool ContainsAny(this string str, params object[] array)
		{
			return str.ContainsAny(array.AsEnumerable());
		}

		public static bool EndsWithAny(this string str, IEnumerable<object> array)
		{
			return array.Any(item => str.EndsWith(item.ToString()));
		}

		public static bool EndsWithAny(this string str, params object[] array)
		{
			return str.EndsWithAny(array.AsEnumerable());
		}

		public static bool EqualsTo(this string str1, string str, bool ignoreCase = true)
		{
			return str1.CompareTo(str, ignoreCase) == 0;
		}

		public static string GetPhrase(this string str, char start, char end, int index)
		{
			var result = str;
			for (var counter = 0; counter != index + 1; counter++)
				if (result == (result = result.Remove(0, result.IndexOf(start) + 1)))
					return string.Empty;
			return result.Substring(0, result.IndexOf(end));
		}

		public static IEnumerable<string> GetPhrase(this string str, char start, char end)
		{
			var index = 0;
			var result = str.GetPhrase(start, end, index);
			while (!result.IsNullOrEmpty())
			{
				yield return result;
				result = str.GetPhrase(start, end, ++index);
			}
		}

		public static int? IndexOf(this IEnumerable<string> array, string item, bool trimmed = false, bool ignoreCase = true)
		{
			var buffer = array.ToCollection();

			for (var counter = 0; counter != buffer.Count; counter++)
				if (string.Compare((trimmed ? buffer[counter].Trim() : buffer[counter]), (trimmed ? item.Trim() : item), ignoreCase) == 0)
					return counter;
			return null;
		}

		public static bool IsInetegerOrControl(this char key)
		{
			return (char.IsDigit(key) || char.IsControl(key));
		}

		public static string Merge(string quatStart, string quatEnd, string separator, params object[] array)
		{
			var result = array.Aggregate(String.Empty, (current, str) => current + (quatStart + str + quatEnd + separator + " ")).Trim();
			return result.Substring(0, result.Length - 1);
		}

		//public static string ToString(this object source, string formatString)
		//{
		//    return FormatWith(formatString, null, source);
		//}

		//// Wrapper for James Newton-King's FormatWith()
		//public static string ToString(this object source, string formatString, IFormatProvider provider)
		//{
		//    return FormatWith(formatString, provider, source);
		//}

		// Copied from James Newton-King at:
		// http://james.newtonking.com/archive/2008/03/29/formatwith-2-0-string-formatting-with-named-variables.aspx
		//private static string FormatWith(string format, IFormatProvider provider, object source)
		//{
		//    if (format == null)
		//        throw new ArgumentNullException("format");

		//    var regex = new Regex(@"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+",
		//                          RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
		//    var values = new List<object>();

		//    var rewrittenFormat = regex.Replace(format, delegate(Match m)
		//    {
		//        var startGroup = m.Groups["start"];
		//        var propertyGroup = m.Groups["property"];
		//        var formatGroup = m.Groups["format"];
		//        var endGroup = m.Groups["end"];

		//        values.Add((propertyGroup.Value == "0") ? source : DataBinder.Eval(source, propertyGroup.Value));
		//        return new string('{', startGroup.Captures.Count) + (values.Count - 1) + formatGroup.Value +
		//               new string('}', endGroup.Captures.Count);
		//    });

		//    return string.Format(provider, rewrittenFormat, values.ToArray());
		//}

		public static IEnumerable<string> Split(this string value, int groupSize)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			for (var x = 0; x < value.Length; x += groupSize)
				yield return value.Substring(x, Math.Min(groupSize, value.Length - x));
		}

		public static string JoinStrings(this IEnumerable<string> values, string separator)
		{
			if (values == null)
				throw new ArgumentNullException("values");

			return string.Join(separator, values.ToArray());
		}

		public static string ConcatStrings(this IEnumerable<string> values)
		{
			if (values == null)
				throw new ArgumentNullException("values");

			return string.Concat(values.ToArray());
		}

		public static IEnumerable<string> TrimAll(this IEnumerable<string> values, params char[] trimChars)
		{
			return values.Select(t => t.Trim(trimChars));
		}

		public static IEnumerable<string> TrimAll(this IEnumerable<string> values)
		{
			return values.Select(t => t.Trim());
		}

		public static string ToStringJoined<T>(this IEnumerable<T> values, string separator)
		{
			if (values == null)
				throw new ArgumentNullException("values");

			var strings = values.Select(v => v.ToString());
			return string.Join(separator, strings.ToArray());
		}

		public static string ToStringConcat<T>(this IEnumerable<T> values)
		{
			if (values == null)
				throw new ArgumentNullException("values");

			var strings = values.Select(v => v.ToString());
			return string.Concat(strings.ToArray());
		}

		public static string RemoveFromEnd(this string str, string oldValue)
		{
			if (str == null)
				throw new ArgumentNullException("str");
			if (oldValue == null)
				throw new ArgumentNullException("oldValue");

			return str.EndsWith(oldValue) ? str.Substring(0, str.Length - oldValue.Length) : str;
		}

		public static IEnumerable<string> SplitCamelCase(this string value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			var lastWord = 0;

			for (var x = lastWord + 1; x < value.Length; x++)
			{
				if (!Char.IsUpper(value, x))
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
				throw new ArgumentNullException("value");
			if (encoding == null)
				throw new ArgumentNullException("encoding");

			return encoding.GetBytes(value);
		}

		public static string GetLongest(this string[] strings)
		{
			return strings.Max();
		}

		public static string AddSpace(this string s, int count)
		{
			Contract.Requires(count != 0);
			ArgHelper.AssertBiggerThan(count, 0, "count");
			return string.Concat(s, " ".Repeat(count));
		}

		public static bool IsInRange(this string text, params string[] range)
		{
			return IsInRange(text, true, range);
		}

		public static bool IsInRange(this string text, bool ignoreCase, params string[] range)
		{
			return IsInRange(text, false, ignoreCase, range);
		}

		public static bool IsInRange(this string text, bool trimmed, bool ignoreCase, params string[] range)
		{
			return range.IndexOf(text, trimmed, ignoreCase) >= 0;
		}

		public static bool IsInRange(this string text, bool trimmed, bool ignoreCase, IEnumerable<string> range)
		{
			return range.IndexOf(text, trimmed, ignoreCase) >= 0;
		}

		public static bool IsInString(this char c, string range)
		{
			return range.Contains(c.ToString());
		}

		public static bool IsInteger(this string text)
		{
			int num;
			return int.TryParse(text, out num);
		}

		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		public static string IsNull(this string str, string defaultValue)
		{
			return str.IsNullOrEmpty() ? defaultValue : str;
		}

		public static bool IsNumber(this string text)
		{
			float num;
			return float.TryParse(text, out num);
		}

		public static bool IsUnicode(this string str)
		{
			var unicodeBytes = Encoding.Unicode.GetBytes(str);
			for (var i = 1; i < unicodeBytes.Length; i += 2)
				if (unicodeBytes[i] != 0)
					return true;
			return false;
		}

		public static string Merge(this IEnumerable<string> array)
		{
			return array.Merge(", ");
		}

		public static string Merge(this IEnumerable<string> array, string separator)
		{
			return String.Join(separator, array.ToArray());
		}

		public static string Merge(string quat, string separator, params string[] array)
		{
			return array.Merge(quat, separator);
		}

		public static string Merge(this IEnumerable<string> array, string quat, string separator)
		{
			var result = array.Aggregate(String.Empty, (current, str) => current + (quat + str + quat + separator + " ")).Trim();

			return result.Substring(0, result.Length - 1);
		}

		public static bool NotEndsWithAny(this string str, IEnumerable<object> array)
		{
			return array.Any(item => !str.EndsWith(item.ToString()));
		}

		public static bool NotEndsWithAny(this string str, params object[] array)
		{
			return str.NotEndsWithAny(array.AsEnumerable());
		}

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

			var t = CorrectUnicodeProblem(text);

			//char[] outChar = { '~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '=', '{', '[', '}', ']', ':', ';', '\'', '"', '<', ',', '>', '?', '/', '|', '\\' };
			char[] outChar =
			{
				'~', '`', '!', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '{', '[', '}', ']', ':', ';', '\'', '"', '<', ',', '>', '?', '|', '\\'
			};

			var arrayString = t.Split(outChar);

			return arrayString.Aggregate(result, (current, s) => current + s);
		}

		public static string Repeat(this string text, int count)
		{
			ArgHelper.AssertBiggerThan(count, 0, "count");
			var result = new StringBuilder(String.Empty);
			for (var counter = 0; counter < count; counter++)
				result.Append(text);
			return result.ToString();
		}

		public static string CorrectUnicodeProblem(string expression)
		{
			if (string.IsNullOrEmpty(expression))
				return expression;
			expression = expression.Replace((char)1740, (char)1610);
			expression = expression.Replace((char)1705, (char)1603);
			return expression;
			/*
                        var result = expression;

                        var ch = expression.ToCharArray();

                        for (var i = 0; i < ch.Length; i++)
                        {
                            if (Strings.AscW(ch[i]) == 1740)
                                ch[i] = Strings.ChrW(1610);

                            if (Strings.AscW(ch[i]) == 1705)
                                ch[i] = Strings.ChrW(1603);
                        }

                        return new string(ch);
            */
		}

		public static string SqlEncodingToUtf(this string obj)
		{
			return Encoding.UTF8.GetString(Encoding.GetEncoding(1256).GetBytes(obj));
		}

		public static string ToHex(string str)
		{
			var sb = new StringBuilder();
			var bytes = IsUnicode(str) ? Encoding.Unicode.GetBytes(str) : Encoding.ASCII.GetBytes(str);
			for (var counter = 0; counter < bytes.Length; counter++)
				sb.Append(bytes[counter].ToString("X"));
			return sb.ToString();
		}

		public static IEnumerable<int> ToInt(IEnumerable<string> array)
		{
			return array.Where(str => str.IsNumber()).Select(str => str.ToInt());
		}

		public static string ToUnicode(this string str)
		{
			return Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(str));
		}

		public static bool EndsWithAny(this string str, IEnumerable<string> values)
		{
			return values.Any(str.EndsWith);
		}

		public static bool StartsWithAny(this string str, IEnumerable<string> values)
		{
			return values.Any(str.StartsWith);
		}

		public static IEnumerable<int> AllIndexesOf(this string str, string value, bool ignoreCase = false)
		{
			var buffer = ignoreCase ? str.ToLower() : str;
			var stat = ignoreCase ? value.ToLower() : value;
			var statIndex = 0;
			int currIndex;
			while ((currIndex = buffer.IndexOf(stat)) != -1)
			{
				statIndex += currIndex;
				yield return statIndex;
				statIndex += stat.Length;
				buffer = buffer.Substring(currIndex + stat.Length);
			}
		}

		public static bool Contains(this IEnumerable<string> array, string str, bool ignoreCase = false)
		{
			return array.Any(s => s.CompareTo(str, ignoreCase) == 0);
		}
	}
}
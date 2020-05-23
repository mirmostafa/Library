#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Library40.Globalization.Attributes;
using Library40.Helpers.Internals;

namespace Library40.Helpers
{
	/// <summary>
	///     A utility to do some common tasks about enumerations
	/// </summary>
	public static class EnumHelper
	{
		#region AddFlag
		/// <summary>
		/// </summary>
		/// <param name="enumeration"></param>
		/// <param name="item"></param>
		/// <typeparam name="TEnum"></typeparam>
		/// <returns></returns>
		public static TEnum AddFlag<TEnum>(this TEnum enumeration, TEnum item) where TEnum : struct
		{
			//(Enum)Enum.ToObject(typeof(test),(test.Item2|test.Item3).ToInt())
			return (TEnum)Enum.ToObject(typeof (TEnum), (enumeration.ToInt() | item.ToInt()));
		}
		#endregion

		#region Contains
		public static bool Contains<TEnum>(this Enum enumeration, TEnum item) where TEnum : struct
		{
			return ((item.ToInt() == 0) ? (enumeration.ToInt() == 0) : ((enumeration.ToInt() | item.ToInt()) == enumeration.ToInt()));
		}
		#endregion

		#region Convert
		/// <summary>
		/// </summary>
		/// <param name="enumValue"></param>
		/// <typeparam name="TSourceEnum"></typeparam>
		/// <typeparam name="TDestinationEnum"></typeparam>
		/// <returns></returns>
		public static TDestinationEnum Convert<TSourceEnum, TDestinationEnum>(TSourceEnum enumValue) where TDestinationEnum : struct
		{
			return (TDestinationEnum)Enum.Parse(typeof (TDestinationEnum), enumValue.ToString());
		}
		#endregion

		#region Convert
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <typeparam name="TEnum"></typeparam>
		/// <returns></returns>
		public static TEnum Convert<TEnum>(object value) where TEnum : struct
		{
			return Convert<object, TEnum>(value);
		}
		#endregion

		#region GetItemAttribute
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <typeparam name="TAttribute"></typeparam>
		/// <returns></returns>
		public static TAttribute GetItemAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
		{
			return value.GetItemAttributes<TAttribute>().FirstOrDefault();
		}

		public static IEnumerable<TAttribute> GetItemAttributes<TAttribute>(this Enum value) where TAttribute : Attribute
		{
			if (value == null)
				return EnumerableHelper.AsEnuemrable(default(TAttribute));
			var attributes = (TAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof (TAttribute), false);
			return ((attributes.Length > 0) ? attributes.AsEnumerable() : EnumerableHelper.AsEnuemrable(default(TAttribute)));
		}
		#endregion

		#region GetItemAttribute
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <typeparam name="TEnum"></typeparam>
		/// <typeparam name="TAttribute"></typeparam>
		/// <returns></returns>
		public static object GetItemAttribute<TEnum, TAttribute>(this TEnum value) where TAttribute : Attribute where TEnum : struct
		{
			var attributes = (TAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof (TAttribute), false);
			return ((attributes.Length > 0) ? attributes[0] : default(TAttribute));
		}
		#endregion

		#region GetItemDescription
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetItemDescription(this Enum value)
		{
			return value.GetItemDescription(true, true);
		}

		public static string GetItemDescription(this Enum value, string cultureName)
		{
			return value.GetItemDescription(true, cultureName, true);
		}
		#endregion

		#region GetItemDescription
		public static string GetItemDescription(this Enum value, bool localized, bool parseNameIfNoDescription)
		{
			return value.GetItemDescription(localized, Thread.CurrentThread.CurrentCulture.Name, parseNameIfNoDescription);
		}

		public static string GetItemDescription(this Enum value, bool localized, string cultureName, bool parseNameIfNoDescription)
		{
			if (localized)
			{
				var descriptions = value.GetItemAttributes<LocalizedDescriptionAttribute>();
				if (descriptions.Count() == 0)
					return value.ToString().SeparateCamelCase();
				var description = descriptions.Where(desc => cultureName.EqualsTo(desc.CultureName)).FirstOrDefault();
				return description == null ? value.ToString().SeparateCamelCase() : description.Description;
			}
			var descriptionAttribute = value.GetItemAttribute<DescriptionAttribute>();
			return ((descriptionAttribute == null) ? value.ToString().SeparateCamelCase() : descriptionAttribute.Description);
		}
		#endregion

		#region GetMembers
		/// <summary>
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <returns></returns>
		public static IEnumerable<MemberInfo<TEnum>> GetMembers<TEnum>() where TEnum : struct
		{
			var result = new Collection<MemberInfo<TEnum>>();
			var names = Enum.GetNames(typeof (TEnum));
			foreach (var member in from name in names
				let displayMember = ((Enum)Enum.Parse(typeof (TEnum), name)).GetItemDescription()
				let valueMember = (TEnum)Enum.Parse(typeof (TEnum), name)
				select new MemberInfo<TEnum>(displayMember, valueMember))
				result.Add(member);
			return result.AsEnumerable();
		}
		#endregion

		#region GetValues
		/// <summary>
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <typeparam name="TNumberType"></typeparam>
		/// <returns></returns>
		public static IEnumerable<TNumberType> GetValues<TEnum, TNumberType>() where TEnum : struct
		{
			var result = new Collection<TNumberType>();
			Enum.GetValues(typeof (TEnum)).Cast<TEnum>().ForEach(item => result.Add((TNumberType)item));
			return result.AsEnumerable();
		}
		#endregion

		#region IsMemberOf
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <typeparam name="TEnum"></typeparam>
		/// <returns></returns>
		public static bool IsMemberOf<TEnum>(object value) where TEnum : struct
		{
			int iValue;
			return int.TryParse(value.ToString(), out iValue) ? Enum.IsDefined(typeof (TEnum), iValue) : Enum.IsDefined(typeof (TEnum), value.Parse());
		}
		#endregion

		#region Merge
		/// <summary>
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <returns></returns>
		public static TEnum Merge<TEnum>() where TEnum : struct
		{
			var result = Enum.GetValues(typeof (TEnum)).Cast<int>().Aggregate(0, (current, item) => current | item);
			return (TEnum)Enum.Parse(typeof (TEnum), Enum.GetName(typeof (TEnum), result));
		}
		#endregion

		#region Parse
		private static object Parse(this object value)
		{
			return value is string ? value.ToString().Contains(".") ? value.ToString().Substring(value.ToString().LastIndexOf(".") + 1) : value : value;
		}
		#endregion

		#region RemoveFlag
		/// <summary>
		/// </summary>
		/// <param name="enumeration"></param>
		/// <param name="item"></param>
		/// <typeparam name="TEnum"></typeparam>
		/// <returns></returns>
		public static TEnum RemoveFlag<TEnum>(this TEnum enumeration, TEnum item) where TEnum : struct
		{
			return (TEnum)Enum.ToObject(typeof (TEnum), (enumeration.ToInt() & ~item.ToInt()));
		}
		#endregion

		#region TryParse
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="result"></param>
		/// <typeparam name="TEnum"></typeparam>
		/// <returns></returns>
		public static bool TryParse<TEnum>(object value, out TEnum result) where TEnum : struct
		{
			result = default(TEnum);
			if (!IsMemberOf<TEnum>(value))
				return false;
			result = Convert<TEnum>(value.Parse());
			return true;
		}

		public static IEnumerable<TEnum> ParseIf<TEnum>(IEnumerable source) where TEnum : struct
		{
			foreach (var item in source)
			{
				TEnum result;
				if (TryParse(item, out result))
					yield return result;
			}
		}
		#endregion

		public static bool IsEnumInRange<TEnum>(this TEnum value, params TEnum[] range) where TEnum : struct
		{
			return range.Contains(value);
		}

		public static IDictionary<T, string> GetValues<T>()
		{
			var type = typeof (T);

			if (!type.IsEnum)
				throw new ArgumentException("type must be an enumeration type.");

			var pairs = from T t in Enum.GetValues(type)
				select new
				       {
					       Value = (T)Enum.ToObject(type, t),
					       Text = Enum.GetName(type, t)
				       };

			return pairs.ToDictionary(t => t.Value, t => t.Text);
		}

		public static IEnumerable<TEnum> GetItems<TEnum>() where TEnum : struct
		{
			var descs = Enum.GetNames(typeof (TEnum)).ForEach(name =>
			                                                  {
				                                                  var result = (TEnum)Enum.Parse(typeof (TEnum), name, false);
				                                                  return result;
			                                                  });
			return descs;
		}

		public static IEnumerable<string> GetDescriptions<TEnum>(this IEnumerable<TEnum> items) where TEnum : struct
		{
			return items.ForEach(item => GetItemDescription(item as Enum));
		}

		public static IEnumerable<string> GetDescriptions<TEnum>(this IEnumerable<TEnum> items, string cultureName) where TEnum : struct
		{
			return items.ForEach(item => GetItemDescription(item as Enum, cultureName));
		}
	}
}
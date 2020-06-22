using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Mohammad.Globalization.Attributes;
using Mohammad.Helpers.Internals;

namespace Mohammad.Helpers
{
    /// <summary>
    ///     A utility to do some common tasks about enumerations
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="enumeration"></param>
        /// <param name="item"></param>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static TEnum AddFlag<TEnum>(TEnum enumeration, TEnum item) where TEnum : struct
            => (TEnum) Enum.ToObject(typeof(TEnum), enumeration.ToInt() | item.ToInt());

        public static bool Contains<TEnum>(Enum enumeration, TEnum item) where TEnum : struct
            => item.ToInt() == 0 ? enumeration.ToInt() == 0 : (enumeration.ToInt() | item.ToInt()) == enumeration.ToInt();

        /// <summary>
        /// </summary>
        /// <param name="enumValue"></param>
        /// <typeparam name="TSourceEnum"></typeparam>
        /// <typeparam name="TDestinationEnum"></typeparam>
        /// <returns></returns>
        public static TDestinationEnum Convert<TSourceEnum, TDestinationEnum>(TSourceEnum enumValue) where TDestinationEnum : struct
            => (TDestinationEnum) Enum.Parse(typeof(TDestinationEnum), enumValue.ToString());

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static TEnum Convert<TEnum>(object value) where TEnum : struct
        {
            return Convert<object, TEnum>(value);
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public static TAttribute GetItemAttribute<TAttribute>(Enum value) where TAttribute : Attribute
        {
            return GetItemAttributes<TAttribute>(value).FirstOrDefault();
        }

        public static IEnumerable<TAttribute> GetItemAttributes<TAttribute>(Enum value) where TAttribute : Attribute
        {
            if (value == null)
                return Enumerable.Empty<TAttribute>();
            var attributes = (TAttribute[]) value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(TAttribute), false);
            return attributes.Length > 0 ? attributes.AsEnumerable() : Enumerable.Empty<TAttribute>();
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public static object GetItemAttribute<TEnum, TAttribute>(TEnum value) where TAttribute : Attribute where TEnum : struct
        {
            var attributes = (TAttribute[]) value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(TAttribute), false);
            return attributes.Length > 0 ? attributes[0] : default(TAttribute);
        }

        public static string GetItemDescription(Enum value, bool localized = true, string cultureName = "en-US", bool parseNameIfNoDescription = true)
        {
            if (localized)
            {
                var descriptions = GetItemAttributes<LocalizedDescriptionAttribute>(value).ToArray();
                if (!descriptions.Any())
                    return value.ToString().SeparateCamelCase();
                var description = descriptions.FirstOrDefault(desc => cultureName.EqualsTo(desc.CultureName));
                return description == null ? value.ToString().SeparateCamelCase() : description.Description;
            }
            var descriptionAttribute = GetItemAttribute<DescriptionAttribute>(value);
            return descriptionAttribute == null ? value.ToString().SeparateCamelCase() : descriptionAttribute.Description;
        }

        /// <summary>
        /// </summary>
        /// <param name="localized"></param>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static IEnumerable<MemberInfo<TEnum>> GetMembers<TEnum>(bool localized = false) where TEnum : struct
        {
            var result = new Collection<MemberInfo<TEnum>>();
            var names = Enum.GetNames(typeof(TEnum));
            foreach (var member in from name in names
                                   let displayMember = GetItemDescription((Enum) Enum.Parse(typeof(TEnum), name), localized)
                                   let valueMember = (TEnum) Enum.Parse(typeof(TEnum), name)
                                   select new MemberInfo<TEnum>(displayMember, valueMember, (int) Enum.Parse(typeof(TEnum), name)))
                result.Add(member);
            return result.AsEnumerable();
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TNumberType"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TNumberType> GetValues<TEnum, TNumberType>() where TEnum : struct
        {
            var result = new Collection<TNumberType>();
            Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ForEach(item => result.Add((TNumberType) item));
            return result.AsEnumerable();
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static bool IsMemberOf<TEnum>(object value) where TEnum : struct
        {
            int iValue;
            return int.TryParse(value.ToString(), out iValue) ? Enum.IsDefined(typeof(TEnum), iValue) : Enum.IsDefined(typeof(TEnum), Parse(value));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static TEnum Merge<TEnum>() where TEnum : struct
        {
            var result = Enum.GetValues(typeof(TEnum)).Cast<int>().Aggregate(0, (current, item) => current | item);
            return (TEnum) Enum.Parse(typeof(TEnum), Enum.GetName(typeof(TEnum), result));
        }

        private static object Parse(object value)
        {
            return value is string ? value.ToString().Contains(".") ? value.ToString().Substring(value.ToString().LastIndexOf(".") + 1) : value : value;
        }

        /// <summary>
        /// </summary>
        /// <param name="enumeration"></param>
        /// <param name="item"></param>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static TEnum RemoveFlag<TEnum>(TEnum enumeration, TEnum item) where TEnum : struct
        {
            return (TEnum) Enum.ToObject(typeof(TEnum), enumeration.ToInt() & ~item.ToInt());
        }

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
            result = Convert<TEnum>(Parse(value));
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

        public static bool IsEnumInRange<TEnum>(TEnum value, params TEnum[] range) where TEnum : struct { return range.Contains(value); }

        public static IDictionary<T, string> GetValues<T>()
        {
            var type = typeof(T);

            if (!type.IsEnum)
                throw new ArgumentException("type must be an enumeration type.");

            var pairs = from T t in Enum.GetValues(type)
                        select new {Value = (T) Enum.ToObject(type, t), Text = Enum.GetName(type, t)};

            return pairs.ToDictionary(t => t.Value, t => t.Text);
        }

        public static IEnumerable<TEnum> GetItems<TEnum>() where TEnum : struct
        {
            var descs = Enum.GetNames(typeof(TEnum)).Select(name =>
            {
                var result = (TEnum) Enum.Parse(typeof(TEnum), name, false);
                return result;
            });
            return descs;
        }

        public static IEnumerable<string> GetDescriptions<TEnum>(IEnumerable<TEnum> items) where TEnum : struct
        {
            return items.Select(item => GetItemDescription(item as Enum));
        }

        public static IEnumerable<string> GetDescriptions<TEnum>(IEnumerable<TEnum> items, string cultureName) where TEnum : struct
        {
            return items.Select(item => GetItemDescription(item as Enum, cultureName: cultureName));
        }

        public static TEnum ToEnum<TEnum>(string value) where TEnum : struct { return Enum.Parse(typeof(TEnum), value).To<TEnum>(); }

        public static TEnum ToEnum<TEnum>(int value) => Enum.Parse(typeof(TEnum), Enum.GetName(typeof(TEnum), value)).To<TEnum>();
    }
}
#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Library40.Helpers
{
	/// <summary>
	/// </summary>
	public static class ObjectHelper
	{
		/// <summary>
		///     Copies the specified source.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <typeparam name="TDestination">The type of the destination.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		public static void Copy<TSource, TDestination>(TSource source, TDestination destination)
		{
			var props = from sourceProperty in typeof (TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public)
				join destProperty in typeof (TDestination).GetProperties(BindingFlags.Instance | BindingFlags.Public) on sourceProperty.Name equals destProperty.Name
				where sourceProperty.CanRead && destProperty.CanWrite
				select new
				       {
					       Value = sourceProperty.GetValue(source, null),
					       Destination = destProperty
				       };

			foreach (var p in props)
				p.Destination.SetValue(destination, p.Value, null);
			//props.ForEach(p => p.Destination.SetValue(destination, p.Value, null));
		}

		/// <summary>
		///     Copies the specified source.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="ignoreExceptions">
		///     if set to <c>true</c> any exception will be ignored while copying.
		/// </param>
		public static void Copy(object source, object destination, bool ignoreExceptions)
		{
			foreach (var srcProperty in source.GetType().GetProperties())
				foreach (var dstProperty in destination.GetType().GetProperties().Where(dstProperty => srcProperty.Name.CompareTo(dstProperty.Name) == 0))
				{
					if (ignoreExceptions)
						MethodHelper.Catch(() => dstProperty.SetValue(destination, srcProperty.GetValue(source, null), null));
					else
						dstProperty.SetValue(destination, srcProperty.GetValue(source, null), null);
					break;
				}
		}

		/// <summary>
		///     Copies the specified source.
		/// </summary>
		/// <typeparam name="TDestination">The type of the destination.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="ignoreExceptions">
		///     if set to <c>true</c> any exception will be ignored while copying.
		/// </param>
		/// <returns></returns>
		public static TDestination Copy<TDestination>(object source, bool ignoreExceptions) where TDestination : new()
		{
			var result = new TDestination();
			Copy(source, result, ignoreExceptions);
			return result;
		}

		/// <summary>
		///     Creates a new instance of TType.
		/// </summary>
		/// <typeparam name="TType">The type of the type.</typeparam>
		/// <returns></returns>
		public static TType CreateInstance<TType>()
		{
			return (TType)typeof (TType).GetConstructor(new Type[]
			                                            {
			                                            }).Invoke(null);
		}

		/// <summary>
		///     Creates a new instance of TType.
		/// </summary>
		/// <typeparam name="TType">The type of the type.</typeparam>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static TType CreateInstance<TType>(Type type)
		{
			return (TType)type.GetConstructor(new Type[]
			                                  {
			                                  }).Invoke(null);
		}

		/// <summary>
		///     Creates an new instance of TType.
		/// </summary>
		/// <typeparam name="TType">The type of the type.</typeparam>
		/// <param name="types">The types.</param>
		/// <param name="args">The constructor's arguments.</param>
		/// <returns></returns>
		public static TType CreateInstance<TType>(Type[] types, object[] args)
		{
			return (TType)typeof (TType).GetConstructor(types).Invoke(args);
		}

		public static TAttribute GetAttribute<TAttribute>(object value) where TAttribute : Attribute
		{
			return GetAttribute<TAttribute>(value, null, true);
		}

		public static TAttribute GetAttribute<TAttribute>(object value, TAttribute defaultValue) where TAttribute : Attribute
		{
			return GetAttribute(value, defaultValue, true);
		}

		public static TAttribute GetAttribute<TAttribute>(object value, TAttribute defaultValue, bool inherited) where TAttribute : Attribute
		{
			var attributes = value.GetType().GetCustomAttributes(typeof (TAttribute), inherited);
			return attributes.Length > 0 ? (TAttribute)attributes[0] : defaultValue;
		}

		public static TAttribute GetAttribute<TType, TAttribute>(TAttribute defaultValue, bool inherited) where TAttribute : Attribute
		{
			var attributes = typeof (TType).GetCustomAttributes(typeof (TAttribute), inherited);
			return attributes.Length > 0 ? (TAttribute)attributes[0] : defaultValue;
		}

		public static TAttribute GetAttribute<TType, TAttribute>() where TAttribute : Attribute
		{
			var attributes = typeof (TType).GetCustomAttributes(typeof (TAttribute), false);
			return attributes.Length > 0 ? (TAttribute)attributes[0] : default(TAttribute);
		}

		public static TAttribute GetAttribute<TAttribute>(object value, bool inherited) where TAttribute : Attribute
		{
			return GetAttribute<TAttribute>(value, null, inherited);
		}

		public static string GetDescription(object classInstance)
		{
			var descriptionAttribute = GetAttribute<DescriptionAttribute>(classInstance);
			return descriptionAttribute == null ? null : descriptionAttribute.Description;
		}

		public static TFieldType GetField<TFieldType>(object obj, string fieldName)
		{
			var field = obj.GetType().GetFields().FirstOrDefault(fld => fld.Name.CompareTo(fieldName) == 0);

			return field != null ? (TFieldType)field.GetValue(obj) : default(TFieldType);
		}

		public static IEnumerable<string> GetPropertiesName<TType>()
		{
			var properties = typeof (TType).IsGenericType ? typeof (TType).GetGenericArguments()[0].GetProperties() : typeof (TType).GetProperties();
			return properties.Select(property => property.Name);
		}

		public static TPropertyType GetProp<TPropertyType>(object obj, string propName)
		{
			return GetProp<TPropertyType>(obj, propName, false);
		}

		public static TPropertyType GetProp<TPropertyType>(object obj, string propName, int eventNoDefault)
		{
			if (eventNoDefault != 0)
				return GetProp<TPropertyType>(obj, propName);
			var property = obj.GetType().GetProperties().FirstOrDefault(prop => prop.Name.CompareTo(propName) == 0);

			return property != null ? (TPropertyType)property.GetValue(obj, null) : default(TPropertyType);
		}

		public static TPropertyType GetProp<TPropertyType>(object obj, string propName, bool serachPrivates)
		{
			var property =
				obj.GetType()
					.GetProperties(serachPrivates ? BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic : BindingFlags.Default)
					.FirstOrDefault(prop => prop.Name.CompareTo(propName) == 0);

			return property != null ? (TPropertyType)property.GetValue(obj, null) : default(TPropertyType);
		}

		public static TType Get<TType>(object value, TType defaultValue)
		{
			try
			{
				return value == null ? defaultValue : (TType)value;
			}
			catch
			{
				return defaultValue;
			}
		}

		public static TResult Iif<TResult>(this bool booleanExpr, TResult trueResult, TResult falseResult)
		{
			return booleanExpr ? trueResult : falseResult;
		}

		public static int? IndexOf<TSource>(TSource item, params TSource[] range)
		{
			var result = Array.IndexOf(range, item);
			return result == -1 ? default(int?) : result;
		}

		public static bool IsIn<TSource>(TSource item, params TSource[] range)
		{
			return range.Contains(item);
		}

		public static TType IsNot<TType>(this TType obj, TType matchValue, TType defaultValue) where TType : IComparable
		{
			return obj.CompareTo(matchValue) == 0 ? defaultValue : obj;
		}

		public static TType IsNull<TType>(TType obj, TType defaultValue) where TType : class
		{
			if (obj == null)
				return defaultValue;
			if (obj is string && String.IsNullOrEmpty(obj as string))
				return defaultValue;
			return obj;
		}

		public static bool IsNull(object obj, bool defaultValue)
		{
			bool result;
			return Boolean.TryParse(obj.ToString(), out result) ? result : defaultValue;
		}

		public static DateTime IsNull(object obj, DateTime defaultValue)
		{
			DateTime result;
			return DateTime.TryParse(obj.ToString(), out result) ? result : defaultValue;
		}

		public static long IsNull(object obj, long defaultValue)
		{
			long result;
			return Int64.TryParse(obj.ToString(), out result) ? result : defaultValue;
		}

		public static TType IsNull<TType>(TType? obj, object defaultValue) where TType : struct
		{
			return obj ?? (TType)defaultValue;
		}

		public static TType IsNull<TType>(this TType obj, TType matchValue, TType defaultValue) where TType : IComparable
		{
			return obj.CompareTo(matchValue) == 0 ? obj : defaultValue;
		}

		public static Dictionary<string, object> ReflectFields(object obj)
		{
			return ReflectFields(obj, false);
		}

		public static Dictionary<string, object> ReflectFields(object obj, bool privateFields)
		{
			var result = new Dictionary<string, object>();
			foreach (var field in
				obj.GetType()
					.GetFields(privateFields
						? BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
						: BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
				try
				{
					result.Add(field.Name, field.GetValue(obj));
				}
				catch
				{
				}
			return result;
		}

		public static Dictionary<string, object> ReflectProperties(object obj)
		{
			return ReflectProperties(obj, false);
		}

		public static Dictionary<string, object> ReflectProperties(object obj, bool privateProperties)
		{
			var result = new Dictionary<string, object>();
			foreach (var property in
				obj.GetType()
					.GetProperties(privateProperties
						? BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
						: BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
				try
				{
					result.Add(property.Name, property.GetValue(obj, null));
				}
				catch
				{
				}
			return result;
		}

		public static Collection<string> ReflectProperties(Type type, bool privateProperties)
		{
			var result = new Collection<string>();
			foreach (var property in
				type.GetProperties(privateProperties
					? BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
					: BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
				try
				{
					result.Add(property.Name);
				}
				catch
				{
				}
			return result;
		}

		public static void SetField(object obj, string fieldName, object value)
		{
			obj.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(obj, value);
		}

		public static void SetProperty(object obj, string propertyName, object value)
		{
			obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(obj, value, null);
		}

		public static TType As<TType>(this object obj) where TType : class
		{
			return obj as TType;
		}

		public static TType To<TType>(this object obj)
		{
			return (TType)obj;
		}

		public static TDelegate GetMethod<TDelegate>(object obj, string name) where TDelegate : class
		{
			var methodInfo = obj.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.Instance);
			return As<TDelegate>(Delegate.CreateDelegate(typeof (TDelegate), obj, methodInfo));
		}

		public static IEnumerable<T> AsEnumerable<T>(T obj)
		{
			yield return obj;
		}

		public static IEnumerable<T> ToEnumerable<T>(this T obj)
		{
			if (obj is IEnumerable<T>)
				return (obj as IEnumerable<T>);
			return new List<T>
			       {
				       obj
			       };
		}

		public static dynamic DynamicCopy(this object obj)
		{
			var result = new ExpandoObject();
			result.AddMany(obj.GetType().GetProperties().Select(prop => new KeyValuePair<string, object>(prop.Name, prop.GetValue(obj, null))));
			return result;
		}
	}
}
#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#endregion

namespace Library40.Helpers
{
	/// <summary>
	///     A utility to do some common tasks about properties
	/// </summary>
	public static class PropertyHelper
	{
		public static T[] GetArray<T>(ref T[] variable, int count)
		{
			return variable ?? (variable = new T[count]);
		}

		public static TReturnType GetValue<TTargetType, TReturnType>(TTargetType target, string propertyName)
		{
			var properties = typeof (TTargetType).GetProperties();
			foreach (var property in properties.Where(property => property.Name.CompareTo(propertyName) == 0))
				return (TReturnType)property.GetValue(target, null);

			return default(TReturnType);
		}

		public static TType Get<TType>(ref TType variable) where TType : class, new()
		{
			return Get(ref variable, () => new TType());
		}

		internal static TType Get<TType>(ref TType variable, TType defaultValue) where TType : class
		{
			return variable ?? (variable = defaultValue);
		}

		public static TType Get<TType>(ref TType variable, Func<TType> creator) where TType : class
		{
			return variable ?? (variable = creator());
		}

		public static IEnumerable<TType> Get<TType>(IEnumerable<TType> backingField, Func<IEnumerable<TType>> gatherItems, Action<IList<TType>> initiated)
		{
			if (backingField == null)
			{
				var result = new List<TType>();
				foreach (var item in gatherItems())
				{
					result.Add(item);
					yield return item;
				}
				initiated(result);
			}
			else
				foreach (var items in backingField)
					yield return items;
		}

		public static bool SetValue<TTargetType>(TTargetType target, string propertyName, object propertyValue)
		{
			return SetValue(target, propertyName, propertyValue, false);
		}

		public static bool SetValue<TTargetType>(TTargetType target, string propertyName, object propertyValue, bool searchAll)
		{
			var properties = searchAll
				? typeof (TTargetType).GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				: typeof (TTargetType).GetProperties();
			foreach (var property in properties.Where(property => property.Name.CompareTo(propertyName) == 0))
				try
				{
					property.SetValue(target, propertyValue, null);
					return true;
				}
				catch
				{
					return false;
				}
			return false;
		}
	}
}
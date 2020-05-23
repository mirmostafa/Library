#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Data;
using System.Reflection;

namespace Library40.Helpers
{
	public partial class AdoHelper
	{
		public static T ValueOf<T>(this DataRow row, string columnName)
		{
			return UnboxT<T>.Unbox(row[columnName]);
		}

		public static T ValueOf<T>(this DataRow row, DataColumn column)
		{
			return UnboxT<T>.Unbox(row[column]);
		}

		public static T ValueOf<T>(this DataRow row, int columnIndex)
		{
			return UnboxT<T>.Unbox(row[columnIndex]);
		}

		public static T ValueOf<T>(this DataRow row, int columnIndex, DataRowVersion version)
		{
			return UnboxT<T>.Unbox(row[columnIndex, version]);
		}

		public static T ValueOf<T>(this DataRow row, string columnName, DataRowVersion version)
		{
			return UnboxT<T>.Unbox(row[columnName, version]);
		}

		public static T ValueOf<T>(this DataRow row, DataColumn column, DataRowVersion version)
		{
			return UnboxT<T>.Unbox(row[column, version]);
		}

		public static void AddRange(this DataColumnCollection columns, params string[] colNames)
		{
			foreach (var colName in colNames)
				columns.Add(new DataColumn(colName));
		}

		#region Nested type: UnboxT
		private static class UnboxT<T>
		{
			internal static readonly Converter<object, T> Unbox = Create(typeof (T));

			private static Converter<object, T> Create(Type type)
			{
				if (type.IsValueType)
				{
					if (type.IsGenericType && !type.IsGenericTypeDefinition && (typeof (Nullable<>) == type.GetGenericTypeDefinition()))
						return
							(Converter<object, T>)
								Delegate.CreateDelegate(typeof (Converter<object, T>),
									typeof (UnboxT<T>).GetMethod("NullableField", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type.GetGenericArguments()[0]));
					return ValueField;
				}
				return ReferenceField;
			}

			private static T ReferenceField(object value)
			{
				return ((DBNull.Value == value) ? default(T) : (T)Convert.ChangeType(value, typeof (T)));
			}

			/// <exception cref="InvalidCastException"></exception>
			private static T ValueField(object value)
			{
				if (DBNull.Value == value)
					throw new InvalidCastException();
				return (T)Convert.ChangeType(value, typeof (T));
			}

			private static TElem? NullableField<TElem>(object value) where TElem : struct
			{
				if (DBNull.Value == value)
					return default(TElem?);
				return (TElem)Convert.ChangeType(value, typeof (TElem));
			}
		}
		#endregion
	}
}
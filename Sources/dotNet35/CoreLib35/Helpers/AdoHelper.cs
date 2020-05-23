#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

#region
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
#endregion

namespace Library35.Helpers
{
	/// <summary>
	///     A utility to do some common tasks about ADO
	///     arguments
	/// </summary>
	/// <author>Mohammad</author>
	public static class AdoHelper
	{
		/// <summary>
		///     Gets the columns data.
		/// </summary>
		/// <param name="row"> The row. </param>
		/// <returns> </returns>
		public static IEnumerable<Object> GetColumnsData(this DataRow row)
		{
			return row.ItemArray.Select((t, i) => row[i]);
		}

		/// <summary>
		///     Gets the column data.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="rows"> The rows. </param>
		/// <param name="columnName"> Name of the column. </param>
		/// <returns> </returns>
		public static IEnumerable<T> GetColumnData<T>(this DataRowCollection rows, string columnName) where T : class
		{
			return rows.Cast<DataRow>().Where(row => row != null).Select(row => row[columnName] as T);
		}

		/// <summary>
		///     Gets the column data.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="rows"> The rows. </param>
		/// <param name="columnIndex"> Index of the column. </param>
		/// <returns> </returns>
		public static IEnumerable<T> GetColumnData<T>(this DataRowCollection rows, int columnIndex = 0) where T : class
		{
			return rows.Cast<DataRow>().Where(row => row != null).Select(row => row[columnIndex] as T);
		}

		/// <summary>
		///     Selects the specified table.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="table"> The table. </param>
		/// <param name="columnTitle"> The column title. </param>
		/// <param name="defaultValue"> The default value. </param>
		/// <returns> </returns>
		public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, params T[] defaultValue)
		{
			var result = table.Select().Select(row => row[columnTitle]);
			return result.Any() ? result.Cast<T>() : defaultValue;
		}

		/// <summary>
		///     Selects the specified table.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="table"> The table. </param>
		/// <param name="columnTitle"> The column title. </param>
		/// <param name="convertor"> The convertor. </param>
		/// <returns> </returns>
		public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Converter<object, T> convertor)
		{
			return table.Select().Select(row => row[columnTitle]).Cast(convertor);
		}

		/// <summary>
		///     Selects the specified table according to de given value .
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="table"> The table. </param>
		/// <param name="columnTitle"> The column title. </param>
		/// <param name="convertor"> The convertor. </param>
		/// <param name="defaultValue"> The default value. </param>
		/// <returns> </returns>
		public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Converter<object, T> convertor, params T[] defaultValue)
		{
			var result = table.Select().Select(row => row[columnTitle]).Cast(convertor);
			return result.Any() ? result : defaultValue;
		}

		/// <summary>
		///     Determines whether the specified column in given row is null or empty.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="row"> The row. </param>
		/// <param name="columnTitle"> The column title. </param>
		/// <returns>
		///     <c>true</c> if the column is null or empty; otherwise, <c>false</c> .
		/// </returns>
		public static bool IsNullOrEmpty<T>(this DataRow row, string columnTitle)
		{
			return row.IsNullOrEmpty(columnTitle) || row[columnTitle].Equals(default(T));
		}

		/// <summary>
		///     Determines whether the specified column in given row is null or empty.
		/// </summary>
		/// <param name="row"> The row. </param>
		/// <param name="columnTitle"> The column title. </param>
		/// <returns>
		///     <c>true</c> if the column is null or empty; otherwise, <c>false</c> .
		/// </returns>
		public static bool IsNullOrEmpty(this DataRow row, string columnTitle)
		{
			return row[columnTitle] == null || row[columnTitle].ToString() == string.Empty || row[columnTitle] == DBNull.Value;
		}

		/// <summary>
		///     Returns the first row data in specific column of the specified table.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="table"> The table. </param>
		/// <param name="columnTitle"> The column title. </param>
		/// <returns> </returns>
		public static T FirstCol<T>(this DataTable table, string columnTitle)
		{
			return FirstCol(table, columnTitle, default(T));
		}

		/// <summary>
		///     Returns the first row data in specific column of the specified table in string format.
		/// </summary>
		/// <param name="table"> The table. </param>
		/// <param name="columnTitle"> The column title. </param>
		/// <returns> </returns>
		public static string FirstCol(this DataTable table, string columnTitle)
		{
			return FirstCol(table, columnTitle, obj => obj.ToString(), string.Empty);
		}

		/// <summary>
		///     Returns the first row data in specific column of the specified table.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="table"> The table. </param>
		/// <param name="columnTitle"> The column title. </param>
		/// <param name="defaultValue"> The default value. </param>
		/// <returns> </returns>
		public static T FirstCol<T>(this DataTable table, string columnTitle, T defaultValue)
		{
			return table.Select(columnTitle, defaultValue).First();
		}

		/// <summary>
		///     Returns the first row data in specific column of the specified table.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="table"> The table. </param>
		/// <param name="columnTitle"> The column title. </param>
		/// <param name="convertor"> The convertor. </param>
		/// <returns> </returns>
		public static T FirstCol<T>(this DataTable table, string columnTitle, Converter<object, T> convertor)
		{
			return table.Select(columnTitle, convertor).First();
		}

		/// <summary>
		///     first the specified table according to de given value.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="table"> The table. </param>
		/// <param name="columnTitle"> The column title. </param>
		/// <param name="convertor"> The convertor. </param>
		/// <param name="defaultValue"> The default value. </param>
		/// <returns> </returns>
		public static T FirstCol<T>(this DataTable table, string columnTitle, Converter<object, T> convertor, T defaultValue)
		{
			return table.Select(columnTitle, convertor, defaultValue).First();
		}

		/// <summary>
		///     Selects the specified table.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="table"> The table. </param>
		/// <param name="columnTitle"> The column title. </param>
		/// <param name="convertor"> The convertor. </param>
		/// <returns> </returns>
		public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Func<object, T> convertor)
		{
			return table.Select().Select(row => convertor(row[columnTitle]));
		}

		/// <summary>
		///     Selects the specified reader.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="reader"> The data reader. </param>
		/// <param name="convertor"> The convertor. </param>
		/// <returns> </returns>
		public static IEnumerable<T> Select<T>(this IDataReader reader, Func<object, T> convertor = null) where T : new()
		{
			var type = typeof (T);
			var properties = type.GetProperties();
			while (reader.Read())
			{
				var t = new T();
				foreach (var property in properties)
					property.SetValue(t,
						reader[property.Name],
						new object[]
						{
						});
				yield return t;
			}
		}

		/// <summary>
		///     Selects the specified table.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="table"> The table. </param>
		/// <param name="columnTitle"> The column title. </param>
		/// <param name="predicate"> The predicate. </param>
		/// <returns> </returns>
		public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Predicate<object> predicate)
		{
			return table.Select().Where(row => predicate != null ? predicate(row) : true).Select(row => row[columnTitle]).Cast<T>();
		}

		/// <summary>
		///     Selects the specified table.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="table"> The table. </param>
		/// <param name="columnTitle"> The column title. </param>
		/// <param name="convertor"> The convertor. </param>
		/// <param name="predicate"> The predicate. </param>
		/// <returns> </returns>
		public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Func<object, T> convertor, Predicate<object> predicate)
		{
			return table.Select().Where(row => predicate != null ? predicate(row) : true).Select(row => convertor(row[columnTitle]));
		}

		public static IEnumerable<DataRow> AsEnumerable(this DataTable table)
		{
			return table.Select().AsEnumerable();
		}

		public static IEnumerable<DataTable> GetTables(this DataSet ds)
		{
			return ds.Tables.Cast<DataTable>();
		}

		public static T Field<T>(this DataRow row, string columnName, Converter<object, T> converter)
		{
			return converter(row.Field<object>(columnName));
		}

		public static T Field<T>(this IDataReader reader, string columnName, Converter<object, T> converter)
		{
			return converter(reader[columnName]);
		}

		public static int ExecuteNonQuery(this SqlConnection connection, string sqlStatement, Action<Exception> catchAction = null, Action finallyAction = null)
		{
			var result = 0;
			using (var command = connection.CreateCommand())
			{
				command.CommandText = sqlStatement;
				command.CommandTimeout = connection.ConnectionTimeout;
				ExceptionHelper.ThrowException(() =>
				                               {
					                               connection.Open();
					                               result = command.ExecuteNonQuery();
				                               },
					catchAction,
					() =>
					{
						if (connection.State != ConnectionState.Closed)
							connection.Close();
					});
			}
			return result;
		}

		public static object ExecuteScalar(this SqlConnection connection, string sqlStatement)
		{
			object result = null;
			using (var command = connection.CreateCommand())
			{
				command.CommandText = sqlStatement;
				command.CommandTimeout = connection.ConnectionTimeout;
				try
				{
					connection.Open();
					result = command.ExecuteScalar();
				}
				finally
				{
					if (connection.State != ConnectionState.Closed)
						connection.Close();
				}
			}
			return result;
		}

		public static IEnumerable<T> Select<T>(this DataTable table) where T : new()
		{
			var type = typeof (T);
			var properties = type.GetProperties();
			foreach (var row in table.Select())
			{
				var t = new T();
				foreach (var property in properties)
					property.SetValue(t,
						row[property.Name],
						new object[]
						{
						});
				yield return t;
			}
		}

		public static IEnumerable<T> SelectByTableName<T>(this SqlConnection connection, string tableName) where T : new()
		{
			using (var command = connection.CreateCommand())
			{
				command.CommandText = string.Format("SELECT * FROM [{0}]", tableName);
				command.CommandTimeout = connection.ConnectionTimeout;
				connection.Open();
				return command.ExecuteReader(CommandBehavior.CloseConnection).Select<T>();
			}
		}

		public static IEnumerable<T> SelectByQuery<T>(this SqlConnection connection, string query) where T : new()
		{
			using (var command = connection.CreateCommand())
			{
				command.CommandText = query;
				command.CommandTimeout = connection.ConnectionTimeout;
				connection.Open();
				return command.ExecuteReader(CommandBehavior.CloseConnection).Select<T>();
			}
		}
	}
}
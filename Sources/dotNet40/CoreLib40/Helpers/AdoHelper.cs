#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

#region
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
#endregion

namespace Library40.Helpers
{
	/// <summary>
	///     A utility to do some common tasks about ADO
	///     arguments
	/// </summary>
	/// <author>Mohammad</author>
	public static partial class AdoHelper
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

		public static T To<T>(this DataRow row) where T : class, new()
		{
			var result = new T();
			foreach (var columnName in row.ItemArray.Select((t, i) => row[i].As<DataColumn>().ColumnName))
				result.GetType().GetProperties().First(prop => prop.Name.EqualsTo(columnName)).SetValue(result, row[columnName], null);
			return result;
		}

		public static IEnumerable<T> To<T>(this DataTable table) where T : class, new()
		{
			return table.Select().Select(row => row.To<T>());
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
				{
					var value = reader[property.Name];
					if (DBNull.Value.Equals(value))
						value = null;
					property.SetValue(t,
						value,
						new object[]
						{
						});
				}
				yield return t;
			}
		}

		public static IEnumerable<T> Select<T>(this SqlConnection connection, string sql, Func<SqlDataReader, T> rowFiller)
		{
			if (rowFiller == null)
				throw new ArgumentNullException("rowFiller");
			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText = sql;
				connection.Open();
				using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
					while (reader.Read())
						yield return rowFiller(reader);
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
			return table.Select().Where(row => predicate == null || predicate(row)).Select(row => row[columnTitle]).Cast<T>();
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
			return table.Select().Where(row => predicate == null || predicate(row)).Select(row => convertor(row[columnTitle]));
		}

		public static IEnumerable<DataTable> GetTables(this DataSet ds)
		{
			return ds.Tables.Cast<DataTable>();
		}

		public static T Field<T>(this IDataReader reader, string columnName, Converter<object, T> converter)
		{
			return converter(reader[columnName]);
		}

		public static T Field<T>(this DataRow row, string columnName, Converter<object, T> converter)
		{
			return converter(row.Field<object>(columnName));
		}

		public static int ExecuteNonQuery(this SqlConnection connection, string sql, Action<Exception> catchAction = null, Action finallyAction = null)
		{
			var result = 0;
			using (var command = connection.CreateCommand())
			{
				command.CommandText = sql;
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

		public static object ExecuteScalar(this SqlConnection connection, string sql, Action<SqlParameterCollection> fillParams = null)
		{
			object result;
			using (var command = connection.CreateCommand())
			{
				command.CommandText = sql;
				command.CommandTimeout = connection.ConnectionTimeout;
				try
				{
					if (fillParams != null)
						fillParams(command.Parameters);
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

		public static SqlDataReader ExecuteReader(this SqlConnection connection,
			string sql,
			Action<SqlParameterCollection> fillParams = null,
			CommandBehavior behavior = CommandBehavior.Default)
		{
			var cmd = connection.CreateCommand(sql);
			connection.Open();
			connection.StateChange += (sender, args) =>
			                          {
				                          if (args.CurrentState == ConnectionState.Closed)
					                          cmd.Dispose();
			                          };
			var result = cmd.ExecuteReader(behavior);

			return result;
		}

		public static SqlCommand CreateCommand(this SqlConnection connection, string commandText)
		{
			var result = connection.CreateCommand();
			result.CommandText = commandText;
			return result;
		}

		public static IEnumerable<T> Select<T>(this DataTable table) where T : new()
		{
			var type = typeof (T);
			var properties = type.GetProperties();
			var columnNames = table.Columns.Cast<DataColumn>().Select(col => col.ColumnName.ToLowerInvariant());
			foreach (var row in table.Select())
			{
				var t = new T();
				foreach (var property in properties)
				{
					if (!columnNames.Contains(property.Name.ToLowerInvariant()))
						continue;
					if (row[property.Name] != null && row[property.Name] != DBNull.Value)
						property.SetValue(t,
							row[property.Name],
							new object[]
							{
							});
				}
				yield return t;
			}
		}

		public static IEnumerable<T> SelectTable<T>(this SqlConnection connection, string tableName) where T : new()
		{
			using (var command = connection.CreateCommand())
			{
				command.CommandText = string.Format("SELECT * FROM [{0}]", tableName);
				command.CommandTimeout = connection.ConnectionTimeout;
				connection.Open();
				return command.ExecuteReader(CommandBehavior.CloseConnection).Select<T>();
			}
		}

		public static IEnumerable<T> SelectQuery<T>(this SqlConnection connection, string query) where T : new()
		{
			using (var command = connection.CreateCommand())
			{
				command.CommandText = query;
				command.CommandTimeout = connection.ConnectionTimeout;
				connection.Open();
				return command.ExecuteReader(CommandBehavior.CloseConnection).Select<T>();
			}
		}

		public static SqlCommand GetCommand(this SqlConnection connection, string sql)
		{
			return new SqlCommand(sql, connection);
		}

		public static DataTable FillDataTable(SqlConnection connection, string sql, Action<SqlParameterCollection> fillParams = null)
		{
			var result = new DataTable();
			using (var command = connection.GetCommand(sql))
			{
				if (fillParams != null)
					fillParams(command.Parameters);
				connection.Open();
				result.Load(command.ExecuteReader());
				connection.Close();
			}
			return result;
		}

		public static void TransactionalCommand(this SqlConnection connection, string sql, Action<SqlCommand> executer, Action<SqlParameterCollection> fillParams = null)
		{
			if (sql == null)
				throw new ArgumentNullException("sql");
			if (executer == null)
				throw new ArgumentNullException("executer");

			ExecuteTransactional(connection,
				transaction =>
				{
					using (var command = new SqlCommand(sql, connection, transaction))
					{
						if (fillParams != null)
							fillParams(command.Parameters);
						executer(command);
					}
				});
		}

		public static void ExecuteTransactional(this SqlConnection connection, Action executionBlock)
		{
			if (connection == null)
				throw new ArgumentNullException("connection");
			if (executionBlock == null)
				throw new ArgumentNullException("executionBlock");
			ExecuteTransactional(connection, tran => executionBlock());
		}

		public static void ExecuteTransactional(this SqlConnection connection, Action<SqlTransaction> executionBlock)
		{
			if (connection == null)
				throw new ArgumentNullException("connection");
			if (executionBlock == null)
				throw new ArgumentNullException("executionBlock");
			var leaveOpen = connection.State == ConnectionState.Open;
			if (!leaveOpen)
				connection.Open();
			var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
			try
			{
				executionBlock(transaction);
				transaction.Commit();
			}
			catch
			{
				transaction.Rollback();
				throw;
			}
			finally
			{
				if (!leaveOpen)
					connection.Close();
			}
		}

		public static DateTime ConvertToDateTime(object value)
		{
			return Convert.ToDateTime(value);
		}

		public static DateTime? ConvertToNullableDateTime(object value)
		{
			return (value == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(value));
		}

		public static DataSet FillDataSet(this SqlConnection connection, string sql)
		{
			var result = new DataSet();
			using (var da = new SqlDataAdapter(sql, connection))
				da.Fill(result);
			return result;
		}

		public static object ExecuteStoredProcedure(this SqlConnection connection, string spName, Action<SqlParameterCollection> fillParams = null, Action<string> logger = null)
		{
			object result = null;
			using (var cmd = connection.CreateCommand())
			{
				var cmdText = new StringBuilder(string.Format("Exec [{0}]", spName));
				if (fillParams != null)
				{
					fillParams(cmd.Parameters);
					for (var index = 0; index < cmd.Parameters.Count; index++)
					{
						var parameter = cmd.Parameters[index];
						cmdText.Append(string.Format("\t{2}{0} = '{1}'", parameter.ParameterName, parameter.Value, Environment.NewLine));
						if (index != cmd.Parameters.Count - 1)
							cmdText.Append(", ");
					}
				}
				if (logger != null)
					logger(cmdText.ToString());
				ExecuteTransactional(connection,
					trans =>
					{
						cmd.Transaction = trans;
						cmd.CommandText = cmdText.ToString();
						result = cmd.ExecuteScalar();
					});
			}
			return result;
		}
	}
}
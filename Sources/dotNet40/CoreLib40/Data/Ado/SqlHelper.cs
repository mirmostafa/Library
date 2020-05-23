#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Library40.Helpers;

namespace Library40.Data.Ado
{
	public class SqlHelper
	{
		public SqlHelper(string connectionstring)
		{
			this.ConnectionString = connectionstring;
		}

		public string ConnectionString { get; set; }

		public virtual DataSet FillByTableNames(params string[] tablenames)
		{
			var result = this.FillByQuery(tablenames.ForEach(t => SqlStatementBuilder.CreateSelect(t)).Merge(Environment.NewLine));
			for (var i = 0; i < tablenames.Length; i++)
				result.Tables[i].TableName = tablenames[i];
			return result;
		}

		public virtual SqlDataReader ExecuteReader(string query)
		{
			return new SqlConnection(this.ConnectionString).ExecuteReader(query, behavior: CommandBehavior.CloseConnection);
		}

		public virtual DataSet FillByQuery(string query)
		{
			using (var conn = new SqlConnection(this.ConnectionString))
				return conn.FillDataSet(query);
		}

		public virtual SqlCommand GetCommand(string sql)
		{
			var connection = new SqlConnection(this.ConnectionString);
			var result = connection.GetCommand(sql);
			result.Disposed += delegate
			                   {
				                   if (connection.State != ConnectionState.Closed)
					                   connection.Close();
				                   connection.Dispose();
			                   };
			return result;
		}

		public virtual void FillDataTable(string sql, DataTable result, Action<SqlParameterCollection> fillParams)
		{
			using (var command = this.GetCommand(sql))
			{
				if (fillParams != null)
					fillParams(command.Parameters);
				command.Connection.Open();
				result.Load(command.ExecuteReader());
				command.Connection.Close();
			}
		}

		public virtual DataTable FillDataTable(string sql)
		{
			return this.FillDataTable(sql, null);
		}

		public virtual DataTable FillDataTable(string sql, Action<SqlParameterCollection> fillParams)
		{
			var result = new DataTable();
			this.FillDataTable(sql, result, fillParams);
			return result;
		}

		public virtual object ExecuteScalar(string sql)
		{
			return this.ExecuteScalar(sql, null);
		}

		public virtual object ExecuteScalar(string sql, Action<SqlParameterCollection> fillParams)
		{
			object result = null;
			this.TransactionalCommand(sql, fillParams, cmd => result = cmd.ExecuteScalar());
			return result;
		}

		public virtual int ExecuteNonQuery(string sql)
		{
			return this.ExecuteNonQuery(sql, null);
		}

		public virtual int ExecuteNonQuery(string sql, Action<SqlParameterCollection> fillParams)
		{
			var result = 0;
			this.TransactionalCommand(sql, fillParams, cmd => result = cmd.ExecuteNonQuery());
			return result;
		}

		public virtual void TransactionalCommand(string sql, Action<SqlCommand> executer)
		{
			this.TransactionalCommand(sql, null, executer);
		}

		public virtual void TransactionalCommand(string sql, Action<SqlParameterCollection> fillParams, Action<SqlCommand> executer)
		{
			if (sql == null)
				throw new ArgumentNullException("sql");
			if (executer == null)
				throw new ArgumentNullException("executer");
			using (var connection = new SqlConnection(this.ConnectionString))
			{
				connection.Open();
				var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
				using (var command = new SqlCommand(sql, connection, transaction))
				{
					if (fillParams != null)
						fillParams(command.Parameters);
					try
					{
						executer(command);
						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
						throw;
					}
					finally
					{
						connection.Close();
					}
				}
			}
		}

		public virtual IEnumerable<object> ExecuteScalar(IDictionary<string, Action<SqlParameterCollection>> fills)
		{
			var result = new List<object>();
			var mgrs = fills.Select(fill => new CmdMgr
			                                {
				                                Fill = fill.Value,
				                                Sql = fill.Key,
				                                Exec = cmd => result.Add(cmd.ExecuteScalar())
			                                });
			this.TransactionalCommand(mgrs);
			return result.AsEnumerable();
		}

		protected virtual void TransactionalCommand(IEnumerable<CmdMgr> commandMgrs)
		{
			using (var connection = new SqlConnection(this.ConnectionString))
			{
				connection.Open();
				var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
				try
				{
					foreach (var mgr in commandMgrs)
						using (var cmd = new SqlCommand(mgr.Sql, connection, transaction))
						{
							if (mgr.Fill != null)
								mgr.Fill(cmd.Parameters);
							mgr.Exec(cmd);
						}
					transaction.Commit();
				}
				catch (Exception)
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public IEnumerable<T> Select<T>(string sql, Func<SqlDataReader, T> rowFiller)
		{
			using (var conn = new SqlConnection(this.ConnectionString))
				return conn.Select(sql, rowFiller);
		}

		public object ExecuteStoredProcedure(string spName, Action<SqlParameterCollection> fillParams = null)
		{
			using (var conn = new SqlConnection(this.ConnectionString))
				return conn.ExecuteStoredProcedure(spName, fillParams);
		}

		#region Nested type: CmdMgr
		protected sealed class CmdMgr
		{
			public Action<SqlCommand> Exec { get; set; }
			public string Sql { get; set; }
			public Action<SqlParameterCollection> Fill { get; set; }
		}
		#endregion
	}
}
#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Library35.Data.SqlServer.Collections;
using Library35.Helpers;

namespace Library35.Data.SqlServer
{
	public class StoredProcedure : SqlObject<StoredProcedure, Database>
	{
		private StoredProcedureParams _Params;

		public StoredProcedure(Database owner, string name)
			: base(owner, name)
		{
		}

		public string AssemblyName { get; set; }

		public string Body { get; set; }

		public DateTime CreateDate { get; set; }

		public long Id { get; set; }

		public bool IsSystemObject { get; set; }

		public DateTime LastModifiedDate { get; set; }

		public StoredProcedureParams Params
		{
			get
			{
				return (this._Params ??
				        (this._Params = new StoredProcedureParams((from row in (from row in this.Owner.AllParams where row.Field<string>("SpName").EqualsTo(this.Name) select row).ToList()
					        select new StoredProcedureParam(this, row.Field<string>("name"))
					               {
						               DefaultValue = row.Field("DefaultValue", Convert.ToString),
						               Id = row.Field("ID", Convert.ToInt64),
						               Length = row.Field("Length", Convert.ToInt32),
						               NumericPrecision = row.Field("NumericPrecision", Convert.ToInt32),
						               SqlDataType = row.Field("DataType", Convert.ToString),
						               ValueOnExecute = row.Field("DefaultValue", Convert.ToString),
					               }).ToList())));
			}
		}

		public string Schema { get; set; }

		public bool TryGatherResultSet(out Dictionary<string, string> prms, Action<Exception> exceptionHandler = null)
		{
			bool flag;
			prms = new Dictionary<string, string>();
			using (var connection = new SqlConnection(this.Owner.ConnectionString))
			using (var command = connection.CreateCommand())
			{
				var builder = new StringBuilder("SET FMTONLY OFF; SET FMTONLY ON;");
				builder.AppendLine();
				builder.Append("exec");
				builder.AppendFormat(" {0}.{1}.{2}", this.Owner.Name, this.Schema, this.Name);
				if (this.Params.Any())
				{
					builder.AppendLine();
					foreach (var param in this.Params)
						builder.AppendFormat(" {0}=NULL, ", param.Name);
					builder.Remove(builder.Length - 2, 2);
				}
				builder.AppendLine();
				builder.AppendLine("SET FMTONLY OFF;");
				command.CommandText = builder.ToString();
				try
				{
					connection.Open();
					using (var reader = command.ExecuteReader())
						if ((reader.FieldCount > 0))
							for (var i = 0; i < reader.FieldCount; i++)
								prms.Add(reader.GetName(i), reader.GetFieldType(i).ToString());
					flag = true;
				}
				catch (Exception exception)
				{
					if (exceptionHandler != null)
						exceptionHandler(exception);
					flag = false;
				}
				finally
				{
					if (connection.State != ConnectionState.Closed)
						connection.Close();
				}
			}
			return flag;
		}

		public SqlDataReader ExecuteReader()
		{
			var builder = new SqlConnectionStringBuilder(this.Owner.ConnectionString);
			var connection = new SqlConnection(this.Owner.ConnectionString);
			using (var command = connection.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = this.Name;
				command.CommandTimeout = builder.ConnectTimeout;
				foreach (var param in this.Params)
					command.Parameters.AddWithValue(param.Name, param.ValueOnExecute);
				connection.Open();
				Debug.WriteLine(string.Format("[{0}] EXECUTING '{1}'", DateTime.Now.ToString("HH:mm:ss.ffff"), command.CommandText));
				try
				{
					var result = command.ExecuteReader(CommandBehavior.CloseConnection);
					Debug.WriteLine(string.Format("[{0}] EXECUTED '{1}'", DateTime.Now.ToString("HH:mm:ss.ffff"), command.CommandText));
					return result;
				}

				catch (Exception exception)
				{
					Debug.WriteLine(string.Format("[{0}] Exception on '{1}': {2}", DateTime.Now.ToString("HH:mm:ss.ffff"), command.CommandText, exception));
					throw;
				}
			}
		}

		public DataTable ExecuteDataTable()
		{
			var result = new DataTable();
			var builder = new SqlConnectionStringBuilder(this.Owner.ConnectionString);
			var connection = new SqlConnection(this.Owner.ConnectionString);
			using (var command = connection.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = this.Name;
				command.CommandTimeout = builder.ConnectTimeout;
				command.Parameters.AddRange(this.Params.Select(p => new SqlParameter(p.Name, p.ValueOnExecute)).ToArray());
				connection.Open();
				Debug.WriteLine(string.Format("[{0}] EXECUTING '{1}'", DateTime.Now.ToString("HH:mm:ss.ffff"), command.CommandText));
				try
				{
					var dataAdapter = new SqlDataAdapter(command);
					dataAdapter.Fill(result);
					Debug.WriteLine(string.Format("[{0}] EXECUTED '{1}'", DateTime.Now.ToString("HH:mm:ss.ffff"), command.CommandText));
					return result;
				}

				catch (Exception exception)
				{
					Debug.WriteLine(string.Format("[{0}] Exception on '{1}': {2}", DateTime.Now.ToString("HH:mm:ss.ffff"), command.CommandText, exception));
					throw;
				}
			}
		}

		public void Execute()
		{
			Debug.WriteLine(string.Format("[{0}] EXECUTING '{1}'", DateTime.Now.ToString("HH:mm:ss.ffff"), this.Name));
			var builder = new SqlConnectionStringBuilder(this.Owner.ConnectionString);
			using (var connection = new SqlConnection(builder.ConnectionString))
			using (var command = connection.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = this.Name;
				command.CommandTimeout = builder.ConnectTimeout;
				command.Parameters.AddRange(this.Params.Select(p => new SqlParameter(p.Name, p.ValueOnExecute)).ToArray());
				try
				{
					connection.Open();
					command.ExecuteScalar();
					Debug.WriteLine(string.Format("[{0}] EXECUTED '{1}'", DateTime.Now.ToString("HH:mm:ss.ffff"), command.CommandText));
				}
				catch (Exception exception)
				{
					Debug.WriteLine(string.Format("[{0}] Exception on '{1}': {2}", DateTime.Now.ToString("HH:mm:ss.ffff"), command.CommandText, exception));
					throw;
				}
			}
		}
	}
}
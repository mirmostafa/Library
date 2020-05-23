#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Library35.Data.SqlServer.Collections;
using Library35.Helpers;

namespace Library35.Data.SqlServer
{
	public class Database : SqlObject<Database, Server>
	{
		private IEnumerable<DataRow> _AllParams;
		private StoredProcedures _StoredProcedures;
		private Tables _Tables;

		internal Database(Server server, string name)
			: base(server, name)
		{
		}

		internal IEnumerable<DataRow> AllParams
		{
			get
			{
				return (this._AllParams ??
				        (this._AllParams =
					        GetQuery(this.ConnectionString,
						        "\r\nexec sp_executesql N'SELECT\nsp.name AS [SpName],\nparam.name AS [Name],\nparam.parameter_id AS [ID],\nparam.default_value AS [DefaultValue],\nusrt.name AS [DataType],\nsparam.name AS [DataTypeSchema],\nISNULL(baset.name, N'''') AS [SystemType],\nCAST(CASE WHEN baset.name IN (N''nchar'', N''nvarchar'') AND param.max_length <> -1 THEN param.max_length/2 ELSE param.max_length END AS int) AS [Length],\nCAST(param.precision AS int) AS [NumericPrecision],\nCAST(param.scale AS int) AS [NumericScale],\nISNULL(xscparam.name, N'''') AS [XmlSchemaNamespace],\nISNULL(s2param.name, N'''') AS [XmlSchemaNamespaceSchema],\nISNULL( (case param.is_xml_document when 1 then 2 else 1 end), 0) AS [XmlDocumentConstraint],\nparam.is_output AS [IsOutputParameter],\nparam.is_cursor_ref AS [IsCursorParameter],\nsp.object_id AS [IDText],\ndb_name() AS [DatabaseName],\nparam.name AS [ParamName],\nCAST(\r\n case \r\n    when sp.is_ms_shipped = 1 then 1\r\n    when (\r\n        select \r\n            major_id \r\n        from \r\n            sys.extended_properties \r\n        where \r\n            major_id = sp.object_id and \r\n            minor_id = 0 and \r\n            class = 1 and \r\n            name = N''microsoft_database_tools_support'') \r\n        is not null then 1\r\n    else 0\r\nend          \r\n             AS bit) AS [ParentSysObj],\n1 AS [Number]\nFROM\nsys.all_objects AS sp\nINNER JOIN sys.all_parameters AS param ON param.object_id=sp.object_id\nLEFT OUTER JOIN sys.types AS usrt ON usrt.user_type_id = param.user_type_id\nLEFT OUTER JOIN sys.schemas AS sparam ON sparam.schema_id = usrt.schema_id\nLEFT OUTER JOIN sys.types AS baset ON (baset.user_type_id = param.system_type_id and baset.user_type_id = baset.system_type_id) \nLEFT OUTER JOIN sys.xml_schema_collections AS xscparam ON xscparam.xml_collection_id = param.xml_collection_id\nLEFT OUTER JOIN sys.schemas AS s2param ON s2param.schema_id = xscparam.schema_id\nWHERE\n((sp.type = @_msparam_1 OR sp.type = @_msparam_2 OR sp.type=@_msparam_3)and(SCHEMA_NAME(sp.schema_id)=@_msparam_5))',N'@_msparam_1 nvarchar(4000),@_msparam_2 nvarchar(4000),@_msparam_3 nvarchar(4000),@_msparam_5 nvarchar(4000)',@_msparam_1=N'P',@_msparam_2=N'RF',@_msparam_3=N'PC',@_msparam_5=N'dbo'")));
			}
		}

		public string CollationName { get; internal set; }

		public string ConnectionString
		{
			get
			{
				var builder = new SqlConnectionStringBuilder(this.Owner.ConnectionString)
				              {
					              InitialCatalog = this.Name
				              };
				return builder.ConnectionString;
			}
		}

		public DateTime CreateDate { get; internal set; }

		public long Id { get; internal set; }

		public bool IsBrokerEnabled { get; internal set; }

		public StoredProcedures StoredProcedures
		{
			get { return PropertyHelper.Get(ref this._StoredProcedures, this.GetStoredProcedures); }
		}

		public Tables Tables
		{
			get { return PropertyHelper.Get(ref this._Tables, this.GetTables); }
		}

		public static Database GetDatabase(string connectionstring, string name = null)
		{
			if (name.IsNullOrEmpty())
				name = new SqlConnectionStringBuilder(connectionstring).InitialCatalog;
			return GetDatabasesCore(connectionstring, string.Format("sys.databases where name = '{0}'", name)).FirstOrDefault();
		}

		public static Databases GetDatabases(string connectionstring)
		{
			return GetDatabasesCore(connectionstring);
		}

		protected static Databases GetDatabasesCore(string connectionstring, string databases = "sys.databases")
		{
			var items = new List<Database>();
			foreach (var row in GetItems(connectionstring, databases))
			{
				var instance = Server.GetInstance();
				instance.SetName(new SqlConnectionStringBuilder(connectionstring).DataSource);
				instance.ConnectionString = connectionstring;
				items.Add(new Database(instance, row.Field<string>("name"))
				          {
					          CollationName = row.Field<string>("collation_name"),
					          CreateDate = row.Field("create_date", Convert.ToDateTime),
					          Id = row.Field("database_id", Convert.ToInt64),
					          IsBrokerEnabled = row.Field("is_broker_enabled", Convert.ToBoolean)
				          });
			}
			return new Databases(items);
		}

		private StoredProcedures GetStoredProcedures()
		{
			return
				new StoredProcedures(
					from row in
						GetQuery(this.ConnectionString,
							"\r\nexec sp_executesql N'SELECT\r\nsp.name AS [Name],\r\nsp.object_id AS [ID],\r\nsp.create_date AS [CreateDate],\r\nsp.modify_date AS [DateLastModified],\r\nISNULL(ssp.name, N'''') AS [Owner],\r\nCAST(case when sp.principal_id is null then 1 else 0 end AS bit) AS [IsSchemaOwned],\r\nSCHEMA_NAME(sp.schema_id) AS [Schema],\r\nCAST(\r\n case \r\n    when sp.is_ms_shipped = 1 then 1\r\n    when (\r\n        select \r\n            major_id \r\n        from \r\n            sys.extended_properties \r\n        where \r\n            major_id = sp.object_id and \r\n            minor_id = 0 and \r\n            class = 1 and \r\n            name = N''microsoft_database_tools_support'') \r\n        is not null then 1\r\n    else 0\r\nend          \r\n             AS bit) AS [IsSystemObject],\r\nCAST(OBJECTPROPERTYEX(sp.object_id,N''ExecIsAnsiNullsOn'') AS bit) AS [AnsiNullsStatus],\r\nCAST(OBJECTPROPERTYEX(sp.object_id,N''ExecIsQuotedIdentOn'') AS bit) AS [QuotedIdentifierStatus],\r\nCAST(CASE WHEN ISNULL(smsp.definition, ssmsp.definition) IS NULL THEN 1 ELSE 0 END AS bit) AS [IsEncrypted],\r\nCAST(ISNULL(smsp.is_recompiled, ssmsp.is_recompiled) AS bit) AS [Recompile],\r\ncase when amsp.object_id is null then N'''' else asmblsp.name end AS [AssemblyName],\r\ncase when amsp.object_id is null then N'''' else amsp.assembly_class end AS [ClassName],\r\ncase when amsp.object_id is null then N'''' else amsp.assembly_method end AS [MethodName],\r\ncase when amsp.object_id is null then case isnull(smsp.execute_as_principal_id, -1) when -1 then 1 when -2 then 2 else 3 end else case isnull(amsp.execute_as_principal_id, -1) when -1 then 1 when -2 then 2 else 3 end end AS [ExecutionContext],\r\ncase when amsp.object_id is null then ISNULL(user_name(smsp.execute_as_principal_id),N'''') else user_name(amsp.execute_as_principal_id) end AS [ExecutionContextPrincipal],\r\nCAST(ISNULL(spp.is_auto_executed,0) AS bit) AS [Startup],\r\nCAST(CASE sp.type WHEN N''RF'' THEN 1 ELSE 0 END AS bit) AS [ForReplication],\r\nCASE WHEN sp.type = N''P'' THEN 1 WHEN sp.type = N''PC'' THEN 2 ELSE 1 END AS [ImplementationType]\r\nFROM\r\nsys.all_objects AS sp\r\nLEFT OUTER JOIN sys.database_principals AS ssp ON ssp.principal_id = ISNULL(sp.principal_id, (OBJECTPROPERTY(sp.object_id, ''OwnerId'')))\r\nLEFT OUTER JOIN sys.sql_modules AS smsp ON smsp.object_id = sp.object_id\r\nLEFT OUTER JOIN sys.system_sql_modules AS ssmsp ON ssmsp.object_id = sp.object_id\r\nLEFT OUTER JOIN sys.assembly_modules AS amsp ON amsp.object_id = sp.object_id\r\nLEFT OUTER JOIN sys.assemblies AS asmblsp ON asmblsp.assembly_id = amsp.assembly_id\r\nLEFT OUTER JOIN sys.procedures AS spp ON spp.object_id = sp.object_id\r\nWHERE\r\n(sp.type = @_msparam_0 OR sp.type = @_msparam_1 OR sp.type=@_msparam_2)and(SCHEMA_NAME(sp.schema_id)=@_msparam_4)',N'@_msparam_0 nvarchar(4000),@_msparam_1 nvarchar(4000),@_msparam_2 nvarchar(4000),@_msparam_4 nvarchar(4000)',@_msparam_0=N'P',@_msparam_1=N'RF',@_msparam_2=N'PC',@_msparam_4=N'dbo'\r\n")
					select new StoredProcedure(this, row.Field<string>("name"))
					       {
						       AssemblyName = row.Field("AssemblyName", Convert.ToString),
						       Id = row.Field("ID", Convert.ToInt64),
						       CreateDate = row.Field("CreateDate", Convert.ToDateTime),
						       Body =
							       GetQuery(this.ConnectionString,
								       string.Format(
									       "\r\nexec sp_executesql N'SELECT\r\nNULL AS [Text],\r\nISNULL(smsp.definition, ssmsp.definition) AS [Definition]\r\nFROM\r\nsys.all_objects AS sp\r\nLEFT OUTER JOIN sys.sql_modules AS smsp ON smsp.object_id = sp.object_id\r\nLEFT OUTER JOIN sys.system_sql_modules AS ssmsp ON ssmsp.object_id = sp.object_id\r\nWHERE\r\n(sp.type = @_msparam_0 OR sp.type = @_msparam_1 OR sp.type=@_msparam_2)and(sp.name=@_msparam_3 and SCHEMA_NAME(sp.schema_id)=@_msparam_4)',N'@_msparam_0 nvarchar(4000),@_msparam_1 nvarchar(4000),@_msparam_2 nvarchar(4000),@_msparam_3 nvarchar(4000),@_msparam_4 nvarchar(4000)',@_msparam_0=N'P',@_msparam_1=N'RF',@_msparam_2=N'PC',@_msparam_3=N'{0}',@_msparam_4=N'dbo'\r\n",
									       row.Field<string>("name"))).First().Field("Definition", Convert.ToString),
						       IsSystemObject = row.Field("IsSystemObject", Convert.ToBoolean),
						       LastModifiedDate = row.Field("DateLastModified", Convert.ToDateTime),
						       Schema = row.Field("Schema", Convert.ToString)
					       });
		}

		private Tables GetTables()
		{
			return new Tables((from row in GetItems(this.ConnectionString, "sys.tables")
				select new Table(this, row.Field<string>("name"))
				       {
					       CreateDate = row.Field("create_date", Convert.ToDateTime),
					       Id = row.Field("object_id", Convert.ToInt64),
					       ModifyDate = row.Field("modify_date", Convert.ToDateTime)
				       }).ToList());
		}
	}
}
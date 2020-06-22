using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Mohammad.Data.SqlServer.Dynamics.Collections;
using Mohammad.Helpers;

namespace Mohammad.Data.SqlServer.Dynamics
{
    public class Table : SqlObject<Table, Database>, IEnumerable
    {
        private Columns _Columns;
        public Columns Columns => PropertyHelper.Get(ref this._Columns, this.GetColumns);
        public DateTime CreateDate { get; set; }
        public long Id { get; set; }
        public DateTime ModifyDate { get; set; }

        public Table(Database owner, string name, string connectionstring)
            : base(owner, name, connectionstring) {}

        public static Tables GetByConnectionString(string connectionstring) => Database.GetDatabase(connectionstring).Tables;

        private Columns GetColumns()
        {
            var helper = new Sql(this.ConnectionString);
            var qColumns = string.Format(QueryBank.COLUMNS_WHERE_TABLE_NAME, this.Name);
            var qIdentities = string.Format(QueryBank.IDENTITIES_WHERE_TABLE_NAME, this.Name);
            var qForeignKeys = string.Format(QueryBank.FOREIGN_KEY_COLUMNS_WHERE_TABLE_NAME, this.Name);
            var query = new[] {qColumns, qIdentities, qForeignKeys}.Merge("; ");
            var columns = new List<Column>();
            using (var reader = helper.ExecuteReader(query))
            {
                while (reader.Read())
                    columns.Add(new Column(null, reader.Field("COLUMN_NAME", Convert.ToString), null)
                                {
                                    CollationName =
                                        reader.Field("COLLATION_NAME", Convert.ToString),
                                    IsNullable =
                                        reader.Field("IS_NULLABLE",
                                            str => str.ToString().EqualsTo("yes")),
                                    MaxLength =
                                        reader.Field("CHARACTER_MAXIMUM_LENGTH",
                                            v =>
                                                DBNull.Value.Equals(v) ? 0 : Convert.ToInt32(v)),
                                    Precision =
                                        reader.Field("NUMERIC_PRECISION",
                                            v =>
                                                DBNull.Value.Equals(v) ? 0 : Convert.ToInt32(v)),
                                    Position =
                                        reader.Field("ORDINAL_POSITION",
                                            v =>
                                                DBNull.Value.Equals(v) ? 0 : Convert.ToInt32(v)),
                                    DataType = reader.Field("DATA_TYPE", Convert.ToString)
                                });
                reader.NextResult();
                while (reader.Read())
                    columns.First(c => c.Name == reader.Field("column_name", Convert.ToString)).IsIdentity = true;
                reader.NextResult();
                while (reader.Read())
                {
                    var column = columns.First(c => c.Name == reader.Field("ParentColumn", Convert.ToString));
                    column.IsForeignKey = true;
                    column.ForeignKeyInfo = new ForeignKeyInfo
                                            {
                                                ReferencedColumn = reader.Field("ReferencedColumn", Convert.ToString),
                                                ReferencedTable = reader.Field("ReferencedTable", Convert.ToString)
                                            };
                }
            }
            return new Columns(columns);
        }

        public DataTable SelectDataTable(params string[] columns)
            => new Sql(this.Owner.ConnectionString).FillByQuery($"SELECT {(columns.Any() ? columns.Merge(",") : "*")} FROM {this.Name}").Tables[0];

        public DataReader SelectReader(params string[] columns)
            =>
                new DataReader(new Sql(this.Owner.ConnectionString).ExecuteReader($"SELECT {(columns.Any() ? columns.Merge(",") : "*")} FROM {this.Name}"),
                    this.Owner,
                    this.Name,
                    this.ConnectionString);

        public SqlDataReader SelectSqlDataReader(params string[] columns)
            => new Sql(this.Owner.ConnectionString).ExecuteReader($"SELECT {(columns.Any() ? columns.Merge(",") : "*")} FROM {this.Name}");

        public IEnumerable<Row> Select(params string[] columns)
        {
            if (!columns.Any())
                columns = this.Columns.Select(c => c.Name).ToArray();
            using (var reader = this.SelectReader(columns))
                while (reader.Read())
                    yield return new Row(this, columns.Select(column => new KeyValuePair<string, object>(column, reader[column])).ToList(), this.ConnectionString);
        }

        public SqlDataReader Where(string condition, params string[] columns)
            => new Sql(this.Owner.ConnectionString).ExecuteReader($"SELECT {(columns.Any() ? columns.Merge(",") : "*")} FROM {this.Name} WHERE {condition}");

        public DataTable WhereDataTable(string condition, params string[] columns)
            => new Sql(this.Owner.ConnectionString).FillByQuery($"SELECT {(columns.Any() ? columns.Merge(",") : "*")} FROM {this.Name} WHERE {condition}").Tables[0];

        public IEnumerator GetEnumerator() => this.SelectDataTable(this.Columns.Select(col => col.Name).ToArray()).Rows.GetEnumerator();
    }
}
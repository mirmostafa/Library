﻿#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Mohammad.Data.SqlServer.Dynamics.Collections;
using Mohammad.Helpers;
using static Mohammad.Data.SqlServer.SqlStatementBuilder;

namespace Mohammad.Data.SqlServer.Dynamics
{
    public class Table : SqlObject<Table, Database>, IEnumerable
    {
        private Columns? _Columns;

        public Table(Database owner, string name, string schema = null, string connectionString = null)
            : base(owner, name, schema, connectionString ?? owner.ConnectionString)
        {
        }

        public Row this[int index] => this.Rows.ElementAt(index);

        public Columns Columns => this._Columns ??= this.GetColumns();

        public DateTime CreateDate { get; set; }
        public long Id { get; set; }
        public DateTime ModifyDate { get; set; }

        public IEnumerable<Row> Rows
        {
            get
            {
                using var reader = this.ToSqlDataReader();
                while (reader.Read())
                {
                    yield return new Row(this, this.Columns.Select(c => new KeyValuePair<string, object>(c.Name, reader[c.Name])).ToDictionary());
                }
            }
        }

        public IEnumerator GetEnumerator() => this.ToDataTable(this.Columns.Select(col => col.Name).ToArray()).Rows.GetEnumerator();

        public static Tables GetByConnectionString(string connectionstring) => Database.GetDatabase(connectionstring).Tables;

        public IEnumerable<string> PrintFormatted(CancellationTokenSource cancellation = null,
            string columnsSeparator = "\t",
            string cellsSeparator = "\t",
            params string[] columns)
        {
            bool IsCancelled() => cancellation?.IsCancellationRequested ?? false;
            var table = this.ToDataTable(columns);
            if (!columns.Any())
            {
                columns = this.Columns.Select(c => c.Name).ToArray();
            }

            if (IsCancelled())
            {
                yield break;
            }

            var lengths = new Dictionary<string, int>();
            foreach (var column in columns)
            {
                if (IsCancelled())
                {
                    yield break;
                }

                var max = 0;
                foreach (DataRow row in table.Rows)
                {
                    if (IsCancelled())
                    {
                        yield break;
                    }

                    var length = row[column]?.ToString().Length;
                    if (length > max)
                    {
                        max = length.Value;
                    }

                    if (column.Length > max)
                    {
                        max = column.Length;
                    }
                }

                lengths.Add(column, max);
            }

            for (var index = 0; index < columns.Length; index++)
            {
                if (IsCancelled())
                {
                    yield break;
                }

                var column = columns[index];
                if (column.Length < lengths[column])
                {
                    var colLen = lengths[column] - column.Length;
                    var col = column.Add(colLen / 2, before: true).Add(colLen / 2).Add(" ");
                    yield return col;
                }
                else
                {
                    yield return column;
                }

                if (index != columns.Length - 1)
                {
                    yield return columnsSeparator;
                }
            }

            yield return Environment.NewLine;
            foreach (DataRow row in table.Rows)
            {
                if (IsCancelled())
                {
                    yield break;
                }

                for (var index = 0; index < columns.Length; index++)
                {
                    if (IsCancelled())
                    {
                        yield break;
                    }

                    var column = columns[index];
                    var data = row[column]?.ToString() ?? string.Empty;
                    if (data.Length < lengths[column])
                    {
                        data = data.Add(lengths[column] - data.Length);
                    }

                    yield return data;
                    if (index != columns.Length - 1)
                    {
                        yield return cellsSeparator;
                    }
                }

                yield return Environment.NewLine;
            }
        }

        public IEnumerable<Row> Select(params string[] columns)
        {
            if (!columns.Any())
            {
                columns = this.Columns.Select(c => c.Name).ToArray();
            }

            using var reader = this.ToReader(columns);
            while (reader.Read())
            {
                yield return new Row(this,
                    columns.Select(column => new KeyValuePair<string, object>(column, reader[column])).ToList(),
                    this.ConnectionString);
            }
        }

        public DataTable ToDataTable(params string[] columns) => this.GetSql().FillDataTable(CreateSelect(this.ToString(), columns));

        public DataTable ToDataTableWhere(string condition, params string[] columns) =>
            this.GetSql().FillDataTable($"{CreateSelect(this.Name, columns)} WHERE {condition}");

        public DataReader ToReader(params string[] columns) => new DataReader(
            this.GetSql().ExecuteReader(CreateSelect(this.Name, columns)),
            this.Owner,
            this.Name,
            this.ConnectionString);

        public SqlDataReader ToReaderWhere(string condition, params string[] columns) =>
            this.GetSql().ExecuteReader($"{CreateSelect(this.Name, columns)} WHERE {condition}");

        public SqlDataReader ToSqlDataReader(params string[] columns) => this.GetSql().ExecuteReader(CreateSelect(this.ToString(), columns));

        public SqlDataReader Where(string condition, params string[] columns) =>
            this.GetSql().ExecuteReader($"{CreateSelect(this.Name, columns)} WHERE {condition}");

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
                {
                    columns.Add(new Column(null, reader.Field("name", Convert.ToString), null)
                    {
                        CollationName = reader.Field("collation_name", Convert.ToString),
                        IsNullable = reader.Field("is_nullable", str => str.ToString().EqualsTo("1")),
                        MaxLength = reader.Field("max_length", v => DBNull.Value.Equals(v) ? 0 : Convert.ToInt32(v)),
                        Precision = reader.Field("precision", v => DBNull.Value.Equals(v) ? 0 : Convert.ToInt32(v)),
                        Position = reader.Field("column_id", v => DBNull.Value.Equals(v) ? 0 : Convert.ToInt32(v)),
                        DataType = reader.Field("system_type", Convert.ToString)
                    });
                }

                reader.NextResult();
                while (reader.Read())
                {
                    columns.First(c => c.Name == reader.Field("column_name", Convert.ToString)).IsIdentity = true;
                }

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
    }
}
﻿using System.Data.SqlClient;
using System.Dynamic;

namespace Library.Data.SqlServer.Dynamics
{
    public class DataReader : SqlObject<DataReader, Database>, IDisposable
    {
        public DataReader(SqlDataReader sqlDataReader, Database owner, string name, string connectionString)
            : base(owner, name, connectionString: connectionString) =>
            this.SqlDataReader = sqlDataReader ?? throw new ArgumentNullException(nameof(sqlDataReader));

        public object this[string index] => this.SqlDataReader[index];

        private SqlDataReader SqlDataReader { get; }

        public void Dispose()
        {
            this.SqlDataReader.Close();
            this.SqlDataReader.Dispose();
        }

        public bool Read() => this.SqlDataReader.Read();

        public IEnumerable<T> Select<T>()
            where T : new() => this.SqlDataReader.Select<T>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder is null)
            {
                throw new ArgumentNullException(nameof(binder));
            }

            result = this.SqlDataReader[binder.Name];
            return true;
        }
    }
}
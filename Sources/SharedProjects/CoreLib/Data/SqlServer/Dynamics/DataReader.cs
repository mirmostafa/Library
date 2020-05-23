using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using Mohammad.Helpers;

namespace Mohammad.Data.SqlServer.Dynamics
{
    public class DataReader : SqlObject<DataReader, Database>, IDisposable
    {
        private SqlDataReader SqlDataReader { get; }

        public object this[string index] => this.SqlDataReader[index];

        public DataReader(SqlDataReader sqlDataReader, Database owner, string name, string connectionstring)
            : base(owner, name, connectionstring)
        {
            if (sqlDataReader == null)
                throw new ArgumentNullException(nameof(sqlDataReader));
            this.SqlDataReader = sqlDataReader;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this.SqlDataReader[binder.Name];
            return true;
        }

        public bool Read() => this.SqlDataReader.Read();

        public IEnumerable<T> Select<T>() where T : new() => this.SqlDataReader.Select<T>();

        public void Dispose()
        {
            this.SqlDataReader.Close();
            this.SqlDataReader.Dispose();
        }
    }
}
using Microsoft.Data.SqlClient;
using System.Dynamic;

using Library.Validations;

namespace Library.Data.SqlServer.Dynamics;

public sealed class DataReader(SqlDataReader sqlDataReader, Database owner, string name, string connectionString) : SqlObject<DataReader, Database>(owner, name, connectionString: connectionString), IDisposable
{
    public object this[string index] => this.SqlDataReader[index];

    private SqlDataReader SqlDataReader { get; } = sqlDataReader ?? throw new ArgumentNullException(nameof(sqlDataReader));

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
        result = this.SqlDataReader[binder.NotNull().Name];
        return true;
    }
}

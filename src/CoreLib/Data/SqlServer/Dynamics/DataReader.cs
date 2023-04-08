using System.Data.SqlClient;
using System.Dynamic;

using Library.Validations;

namespace Library.Data.SqlServer.Dynamics;

public sealed class DataReader : SqlObject<DataReader, Database>, IDisposable
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
        result = this.SqlDataReader[binder.NotNull().Name];
        return true;
    }
}

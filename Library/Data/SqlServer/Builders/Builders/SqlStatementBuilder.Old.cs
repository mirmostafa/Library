using System.Diagnostics.CodeAnalysis;

namespace Library.Data.SqlServer;

//[Obsolete($"Use {typeof(Builders.SqlStatementBuilder)}, instead.")]
public partial class SqlStatementBuilder
{
    public static string CreateSelect([DisallowNull] string tablename, params string[] columns)
        => Select().Columns(columns).From(tablename).Build();
}

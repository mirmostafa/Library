namespace Library.Data.SqlServer;

public partial class SqlStatementBuilder
{
    [Obsolete("Old way. Use fluent methods instead.")]
    public static string CreateSelect([DisallowNull] string tablename, params string[] columns)
        => Select().Columns(columns).From(tablename).Build();
}

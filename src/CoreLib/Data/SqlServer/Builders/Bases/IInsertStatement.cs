namespace Library.Data.SqlServer.Builders.Bases;

public interface IInsertStatement : IInsertStatementBase
{
    IList<string> Values { get; }
}

public interface IInsertStatementBase : IStatementOnTable
{
    IList<(string ColumnName, bool IsString)> Columns { get; }
}

public interface IBulkInsertStatement : IInsertStatementBase
{
    IList<IList<string>> Values { get; }
}
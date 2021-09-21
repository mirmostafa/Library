namespace Library.Data.SqlServer.Builders.Bases;

public interface IUpdateStatement : IStatementOnTable, IWhereClause
{
    Dictionary<string, object> ColumnsValue { get; }
}

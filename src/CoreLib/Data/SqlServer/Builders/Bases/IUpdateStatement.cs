namespace Library.Data.SqlServer.Builders.Bases;

public interface IUpdateStatement : IStatementOnTable, IWhereClause, ICommandStatement
{
    Dictionary<string, object> ColumnsValue { get; }
}

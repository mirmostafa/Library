namespace Library.Data.SqlServer.Builders.Bases;

public interface ISelectStatement : IStatementOnTable, IWhereClause
{
    List<string> Columns { get; }
    string? OrderByColumn { get; set; }
    OrderByDirection OrderByDirection { get; set; }
}

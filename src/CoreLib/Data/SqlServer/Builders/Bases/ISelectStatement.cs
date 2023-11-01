namespace Library.Data.SqlServer.Builders.Bases;

public interface ISelectStatement : IStatementOnTable, IWhereClause
{
    IList<string> Columns { get; }

    int? TopCount { get; set; }

    string? OrderByColumn { get; set; }

    OrderByDirection OrderByDirection { get; set; }

    //! For .NET 8.0
    //? static implicit operator string(in ISelectStatement statement)
    //?     => statement.Build();
}
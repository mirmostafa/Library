namespace Library.Data.SqlServer.Builders.Bases;

public interface IWhereClause
{
    string? WhereClause { get; set; }
}

public interface ICommandStatement
{
    bool ForceFormatValues { get; set; }
}
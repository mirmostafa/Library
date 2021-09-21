namespace Library.Data.SqlServer.Builders.Bases;

public interface IStatementOnTable : ISqlStatement
{
    string TableName { get; set; }
}
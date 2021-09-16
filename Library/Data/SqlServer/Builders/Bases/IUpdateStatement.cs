namespace Library.Data.SqlServer.Builders.Bases;

public interface IUpdateStatement
{
    string TableName { get; set; }
    Dictionary<string, object> ColumnsValue { get; }
    string? WhereClause { get; set; }
}

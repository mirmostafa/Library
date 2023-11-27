namespace Library.Data.SqlServer.Builders.Bases;

public interface IInsertStatement : IStatementOnTable
{
    IDictionary<string, object> Columns { get; }

    bool ReturnId { get; set; }
}
using System.Collections.Generic;

namespace Library.Data.SqlServer.Builders.Bases;

public interface ISelectStatement
{
    string TableName { get; set; }
    List<string> Columns { get; }
    string? WhereClause { get; set; }
    string? OrderByColumn { get; set; }
    OrderByDirection OrderByDirection { get; set; }
}

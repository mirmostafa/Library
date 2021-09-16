using System.Collections.Generic;

namespace Library.Data.SqlServer.Builders.Bases;

public interface ISqlStatement
{
    List<string> Columns { get; }
    string? OrderByColumn { get; set; }
    OrderByDirection OrderByDirection { get; set; }
    string TableName { get; set; }
    string? WhereClause { get; set; }
}
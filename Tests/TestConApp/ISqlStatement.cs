using System.Collections.Generic;
using Library.Data.SqlServer.Builders.Bases;

namespace TestConApp;

public interface ISqlStatement
{
    List<string> Columns { get; }
    string? OrderByColumn { get; set; }
    OrderByDirection OrderByDirection { get; set; }
    string TableName { get; set; }
    string? WhereClause { get; set; }
}
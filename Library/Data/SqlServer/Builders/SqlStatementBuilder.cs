using System.Data.SqlClient;

namespace Library.Data.SqlServer.Builders;

public static partial class SqlStatementBuilder
{
    private static string AddBrackets(in string entity)
    {
        var entities = entity.Split('.');
        var result = entities.Select(e =>
        {
            var r = e.StartsWith("[") ? e : $"[{e}";
            r = r.EndsWith("]") ? r : $"{r}]";
            return r;
        }).Merge('.');
        return result;
    }
    private static string FormatValue(object value) => value switch
    {
        int i => i.ToString(),
        float f => f.ToString(),
        decimal d => d.ToString(),
        long l => l.ToString(),
        string and { Length: 0 } => "''",
        string s => $"'{s}'",
        null => DBNull.Value.ToString(),
        _ => $"'{value}'",
    };
    private static StringBuilder AddClause(in string clause, in string indent, in StringBuilder result)
        => result.AppendLine().Append($"{indent}{clause}");

    public static string CreateExecuteStoredProcedure(string spName, Action<List<SqlParameter>>? fillParams = null)
    {
        var cmdText = new StringBuilder($"Exec [{spName}]");
        if (fillParams == null)
        {
            return cmdText.ToString();
        }

        var parameters = new List<SqlParameter>();
        fillParams(parameters);
        for (var index = 0; index < parameters.Count; index++)
        {
            var parameter = parameters[index];
            _ = cmdText.Append($"\t{Environment.NewLine}{parameter.ParameterName} = '{parameter.Value}'");
            if (index != parameters.Count - 1)
            {
                cmdText.Append(", ");
            }
        }

        return cmdText.ToString();
    }
}

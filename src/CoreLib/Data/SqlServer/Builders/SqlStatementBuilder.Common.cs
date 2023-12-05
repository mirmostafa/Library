using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

using Library.CodeGeneration;
using Library.Data.SqlServer.Builders.Bases;
using Library.Validations;

namespace Library.Data.SqlServer;

public partial class SqlStatementBuilder
{
    public static TCommandStatement ForceFormatValues<TCommandStatement>([DisallowNull] this TCommandStatement statement, bool forceFormatValues = true)
        where TCommandStatement : ICommandStatement => statement.With(x => x.ForceFormatValues = forceFormatValues);

    public static TCommandStatement ReturnId<TCommandStatement>([DisallowNull] this TCommandStatement statement, bool returnId = true)
        where TCommandStatement : ICommandStatement => statement.With(x => x.ReturnId = returnId);

    private static (string? Schema, string Name, IEnumerable<(string Name, TypePath Type)> Columns) GetTable(Type type)
    {
        Check.MustBeArgumentNotNull(type);

        var tableAttribute = type.GetCustomAttribute<TableAttribute>();
        
        var schema = tableAttribute?.Schema;
        var tableName = tableAttribute?.Name ?? type.Name;
        var columns = type.GetProperties().Select(x =>
        {
            string name;
            TypePath type;
            var columnAttribute = x.GetCustomAttribute<ColumnAttribute>();
            if (columnAttribute is { } attrib)
            {
                name = attrib.Name ?? x.Name;
                type = attrib.TypeName ?? x!.DeclaringType!.FullName!;
            }
            else
            {
                name = x.Name;
                type = x.PropertyType;
            }
            return (name, type);
        });
        return (schema, tableName, columns);
    }
}
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
        var columns = type.GetProperties().Where(x => x.GetCustomAttribute<NotMappedAttribute>() == null).Select(x =>
        {
            string name;
            TypePath type;
            int order;
            var columnAttribute = x.GetCustomAttribute<ColumnAttribute>();
            if (columnAttribute is { } attrib)
            {
                name = attrib.Name ?? x.Name;
                type = attrib.TypeName ?? x!.DeclaringType!.FullName!;
                order = attrib.Order;
            }
            else
            {
                name = x.Name;
                type = x.PropertyType;
                order = 0;
            }
            return (name, type, order);
        }).OrderBy(x => x.order).Select(x => (x.name, x.type));
        return (schema, tableName, columns);
    }
}
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Data.SqlServer;

public static class EntityModelConverter
{
    public static IEnumerable<ColumnInfo> GetColumns<TEntityType>() => GetColumns(typeof(TEntityType));
    public static IEnumerable<ColumnInfo> GetColumns(Type entityType)
    {
        var props = entityType.GetProperties();
        foreach (var prop in props)
        {
            var dbGen = ObjectHelper.GetAttribute<DatabaseGeneratedAttribute>(prop);
            if (dbGen?.DatabaseGeneratedOption is DatabaseGeneratedOption.None or DatabaseGeneratedOption.Computed)
            {
                continue;
            }
            var colAttr = ObjectHelper.GetAttribute<ColumnAttribute>(prop);
            var fkAttr = ObjectHelper.GetAttribute<ForeignKeyAttribute>(prop);
            var keyAttr = ObjectHelper.GetAttribute<KeyAttribute>(prop);
            var name = colAttr?.Name is not null ? colAttr.Name : prop.Name;
            var isKey = keyAttr is not null || dbGen?.DatabaseGeneratedOption is DatabaseGeneratedOption.Identity;
            var isFk = fkAttr is not null;
            var fkName = fkAttr?.Name;
            var type = colAttr?.TypeName is not null ? SqlHelper.SqlTypeToNetType(colAttr.TypeName) : prop.PropertyType;
            var order = colAttr?.Order is not null ? colAttr.Order : int.MaxValue;
            yield return new ColumnInfo(name, type, isKey, isFk, fkName, order);
        }
    }
}

public readonly record struct ColumnInfo(string Name, Type Type, bool IsKey, bool IsForiegnKey, string? ForiegnKeyName, int Order);
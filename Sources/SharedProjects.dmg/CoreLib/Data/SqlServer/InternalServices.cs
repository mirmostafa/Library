using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mohammad.Collections.Generic;
using Mohammad.Data.SqlServer.ObjectModel;
using Mohammad.Helpers;

namespace Mohammad.Data.SqlServer
{
    internal static class InternalServices
    {
        internal static TableSchema RefactorCore(Type tableEntity, bool includeAutoIcreamentals, Func<PropertyInfo, object> getValue = null)
        {
            if (getValue == null)
                getValue = p => null;
            var tableAttr = tableEntity.GetCustomAttributes(typeof(SqlTableAttribute), true).Cast<SqlTableAttribute>().FirstOrDefault();
            var tablename = tableAttr != null ? (tableAttr.Name ?? tableEntity.Name) : tableEntity.Name;
            var columns = new List<PairValue<string, object>>();
            foreach (var property in
                tableEntity.GetProperties()
                    .Where(property => property.CustomAttributes.FirstOrDefault(attr => attr.AttributeType == typeof(SqlIgnoreAttribute)) == null))
            {
                if (!includeAutoIcreamentals)
                    if (property.CustomAttributes.FirstOrDefault(attr => attr.AttributeType == typeof(SqlAutoIcreamentalAttribute)) != null)
                        continue;
                var attrdata = property.CustomAttributes.FirstOrDefault(attr => attr.AttributeType == typeof(SqlFieldAttribute));
                if (attrdata == null)
                {
                    columns.Add(new PairValue<string, object>(string.Concat("[", property.Name, "]"), getValue(property)));
                }
                else
                {
                    var fieldAttr = attrdata.AttributeType;
                    columns.Add(new PairValue<string, object>(fieldAttr != null ? fieldAttr.Name.IfNullOrEmpty(property.Name) : property.Name, getValue(property)));
                }
            }
            return new TableSchema {TableName = tablename, Columns = columns.ToEnumerable()};
        }

        internal static TableSchema Refactor(Type tableEntity, bool includeAutoIcreamentals) { return RefactorCore(tableEntity, includeAutoIcreamentals); }

        internal static TableSchema Refactor(object obj, bool includeAutoIcreamentals)
        {
            return RefactorCore(obj.GetType(),
                includeAutoIcreamentals,
                p =>
                {
                    var value = p.GetValue(obj);
                    if (p.PropertyType == typeof(string) || p.PropertyType == typeof(Guid))
                        value = string.Format("N'{0}'", value);
                    else if (p.PropertyType == typeof(bool))
                        value = string.Format("{0}", (bool) value ? 1 : 0);
                    return value;
                });
        }

        internal static TableSchema Refactor(string tableName, IEnumerable<TripleValue<string, object, Type>> props)
        {
            var pairs = new List<PairValue<string, object>>(props.Count());
            foreach (var prop in props)
            {
                var item = new PairValue<string, object>();
                if (prop.Value3 == typeof(string) || prop.Value3 == typeof(Guid) || prop.Value3 == typeof(Guid?))
                    item.Value2 = string.Format("N'{0}'", prop.Value2);
                else if (prop.Value3 == typeof(bool) || prop.Value3 == typeof(bool?))
                    item.Value2 = string.Format("{0}", (bool) prop.Value2 ? 1 : 0);
                else
                    item.Value2 = string.Format("{0}", prop.Value2);
                item.Value1 = string.Format("[{0}]", prop.Value1);
                pairs.Add(item);
            }
            return new TableSchema {Columns = pairs, TableName = tableName};
        }

        internal static TableSchema Refactor(object obj)
        {
            return Refactor(obj.GetType().Name,
                obj.GetType().GetProperties().Select(prop => new TripleValue<string, object, Type>(prop.Name, prop.GetValue(obj), prop.PropertyType)));
        }

        internal class TableSchema
        {
            public string TableName { get; set; }
            public IEnumerable<PairValue<string, object>> Columns { get; set; }
            public IEnumerable<string> ColumnNames { get { return this.Columns.Select(c => c.Value1); } }
        }
    }
}
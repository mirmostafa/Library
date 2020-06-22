using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mohammad.BusinessModel.Classes;
using Mohammad.Data.Ado;
using Mohammad.Data.SqlServer.Dynamics;
using Mohammad.Helpers;

namespace Mohammad.BusinessModel
{
    public class CodeGenerator
    {
        protected readonly Database Database;
        public CodeGenerator(string connectionString) { this.Database = Database.GetDatabase(connectionString); }

        protected virtual IEnumerable<CodeTypeDeclaration> GetTableClass(IEnumerable<Table> tables, string baseClassName = null)
        {
            return tables.Select(table =>
            {
                var tableClass = CodeDomHelper.InitClass(table.Name, "", baseTypeNames: (baseClassName ?? string.Empty).Replace("%EntityName%", table.Name));
                var tableAttr = new CodeAttributeDeclaration("Table");
                tableClass.CustomAttributes.Add(tableAttr);
                tableClass.AddField(typeof(string), "DbTableName", $"\"{table.Name}\"", false, true, true);
                foreach (var column in table.Columns)
                    tableClass.AddField(typeof(string), string.Concat(column.Name, "ColumnName"), string.Concat("\"", column.Name, "\""), false, true, true);
                foreach (var column in table.Columns)
                {
                    var type = ConvertType.SqlTypeToType(column.DataType, false).Name;
                    var colType = $"Column<{type}>";
                    tableClass.AddField(typeof(ColumnStructure),
                        $"{column.Name}ColumnStructure",
                        $"new ColumnStructure(\"{column.Name}\",{column.IsNullable},{column.IsIdentity},{false},{column.IsForeignKey},\"{(column.IsForeignKey ? column.ForeignKeyInfo.ReferencedTable : string.Empty)}\",\"{(column.IsForeignKey ? column.ForeignKeyInfo.ReferencedColumn : string.Empty)}\")",
                        true,
                        false,
                        true);
                    tableClass.AddPropertyWithBackingField(colType,
                        column.Name,
                        $"new Column<{type}>(\"{column.Name}\", {column.IsNullable.ToString().ToLower()}) ",
                        true);
                }
                return tableClass;
            });
        }

        public string GetDataEntity(IEnumerable<string> tablenames = null, bool createModelClass = true, string modelClassName = null,
            string modelBaseClassName = null, string entitiesBaseClassName = null, string ns = null, IEnumerable<string> usings = null)
        {
            var tables = this.GetTables(tablenames).ToList();
            var codeNamespace = new CodeNamespace(ns);
            if (createModelClass)
            {
                var modelClass = CodeDomHelper.InitClass(modelClassName.IfNullOrEmpty(this.Database.Name), "", codeNamespace, modelBaseClassName);
                foreach (var table in tables)
                    modelClass.AddAutoField(string.Concat("List<", table.Name, ">"), string.Concat(table.Name.Replace("#", "_"), "s"));
            }
            codeNamespace.Types.AddRange(this.GetTableClass(tables, entitiesBaseClassName).ToArray());
            if (usings != null && usings.Any())
                codeNamespace.AddUsing(usings.ToArray());
            codeNamespace.AddUsing("Mohammad.Data.Ado.BusinessTools.Attributes");
            return this.Generate(codeNamespace);
        }

        protected virtual IEnumerable<Table> GetTables(IEnumerable<string> tablenames)
        {
            return tablenames != null && tablenames.Any() ? this.Database.Tables.Where(t => tablenames.Contains(t.Name, true)) : this.Database.Tables;
        }

        protected virtual string Generate(CodeNamespace ns)
        {
            var output = new StringBuilder();
            var codeCompileUnit = new CodeCompileUnit();
            codeCompileUnit.AddNamespace(ns);

            using (var writer = new StringWriter(output))
                codeCompileUnit.GenerateCSharpCode(writer);
            var result = output.Replace("{ get; set; };", "{ get; set; }").ToString();
            if (!ns.Name.IsNullOrEmpty())
                result = result.Substring(result.IndexOf(string.Concat("namespace ", ns.Name)));
            return result;
        }

        public string GetBusinessEntity(string ns, IEnumerable<string> tablenames = null, bool perTable = true, string className = null,
            string classesPrefix = "Entity", string baseClass = null, bool isBaseClassGeneric = true, bool addDmlStatements = false,
            IEnumerable<string> usings = null)
        {
            var tables = this.GetTables(tablenames).ToList();
            var codeNamespace = new CodeNamespace(ns);
            if (perTable)
            {
                foreach (var table in tables)
                {
                    CodeTypeDeclaration bizNttClass;
                    if (baseClass != null)
                    {
                        var bc = isBaseClassGeneric ? $"{baseClass}<{table.Name}>" : table.Name;
                        bizNttClass = CodeDomHelper.InitClass(string.Concat(table.Name, classesPrefix), "", codeNamespace, bc);
                    }
                    else
                    {
                        bizNttClass = CodeDomHelper.InitClass(string.Concat(table.Name, classesPrefix), "", codeNamespace);
                    }
                    this.PrepareBusinessClass(bizNttClass, table, addDmlStatements);
                }
            }
            else
            {
                if (className == null)
                    throw new ArgumentNullException(nameof(className));
                CodeTypeDeclaration bizNttClass;
                if (baseClass != null)
                {
                    var bc = isBaseClassGeneric ? $"{baseClass}<{className}>" : className;
                    bizNttClass = CodeDomHelper.InitClass(className, "", codeNamespace, bc);
                }
                else
                {
                    bizNttClass = CodeDomHelper.InitClass(className, "", codeNamespace);
                }
                this.PrepareBusinessClass(bizNttClass, addDmlStatements: addDmlStatements);
            }
            if (usings != null && usings.Any())
                codeNamespace.AddUsing(usings.ToArray());

            return this.Generate(codeNamespace);
        }

        protected virtual void PrepareBusinessClass(CodeTypeDeclaration bizNttClass, Table table = null, bool addDmlStatements = false)
        {
            if (!addDmlStatements)
                return;
            bizNttClass.AddMethod("Select");
            bizNttClass.AddMethod("Insert");
            bizNttClass.AddMethod("Update");
            bizNttClass.AddMethod("Delete");
        }
    }
}
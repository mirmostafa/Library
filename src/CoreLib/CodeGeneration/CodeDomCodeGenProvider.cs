using System.CodeDom;

using Library.CodeGeneration.Models;
using Library.Helpers.CodeGen;
using Library.Validations;

namespace Library.CodeGeneration;

public sealed class CodeDomCodeGenProvider : ICodeGenProvider
{
    public static CodeDomCodeGenProvider New()
        => new();

    public Codes GenerateBehindCode(in INameSpace nameSpace, in GenerateCodesParameters? arguments = default)
    {
        LibLogger.Debug("Generating Behind Code");
        var myNameSpace = nameSpace.ArgumentNotNull();
        var mainUnit = new CodeCompileUnit();
        var partUnit = new CodeCompileUnit();
        var mainNs = mainUnit.AddNewNameSpace(nameSpace.FullName).UseNameSpace(myNameSpace.UsingNameSpaces);
        var partNs = partUnit.AddNewNameSpace(nameSpace.FullName).UseNameSpace(myNameSpace.UsingNameSpaces);
        foreach (var type in nameSpace.CodeGenTypes)
        {
            var codeType = (type.IsPartial ? partNs : mainNs).UseNameSpace(type.UsingNameSpaces).AddNewClass(type.Name, type.BaseTypes, type.IsPartial);
            foreach (var member in type.Members)
            {
                switch (member)
                {
                    case FieldInfo field:
                        codeType = codeType.AddField(field.Type, field.Type, field.Comment, field.AccessModifier);
                        break;

                    case PropertyInfo property when property.HasBackingField:
                        codeType = addProperty(codeType, property);
                        break;

                    case PropertyInfo property:
                        codeType = addProperty(codeType, property);
                        break;
                }
            }
        }

        var mainCode = new Code("Main", Languages.CSharp, mainUnit.GenerateCode(), false);
        var partCode = new Code("Partial", Languages.CSharp, partUnit.GenerateCode(), true);
        var result = new Codes(mainCode, partCode);
        LibLogger.Debug("Generated Behind Code");
        return result;

        static CodeTypeDeclaration addProperty(in CodeTypeDeclaration codeType, in PropertyInfo property) => codeType.AddProperty(
            property.Type,
            property.Name,
            property.Comment,
            property.AccessModifier,
            property.Getter,
            property.Setter,
            property.InitCode,
            property.IsNullable);
    }
}
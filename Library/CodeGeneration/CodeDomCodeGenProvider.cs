using System.CodeDom;
using Library.CodeGeneration.Models;
using Library.Coding.Models;
using Library.Helpers.CodeGen;

namespace Library.CodeGeneration;

public sealed class CodeDomCodeGenProvider : ICodeGenProvider
{
    public Codes GenerateBehindCode(in INameSpace nameSpace, in GenerateCodesParameters arguments)
    {
        LibLogger.Debug("Generating Behind Code");
        var myNameSpace = nameSpace.ArgumentNotNull(nameof(nameSpace));
        var mainUnit = new CodeCompileUnit();
        var partUnit = new CodeCompileUnit();
        var mainNs = mainUnit.AddNewNameSpace(nameSpace.FullName).UseNameSpace(myNameSpace.UsingNameSpaces);
        var partNs = partUnit.AddNewNameSpace(nameSpace.FullName).UseNameSpace(myNameSpace.UsingNameSpaces);
        foreach (var type in nameSpace.CodeGenTypes)
        {
            var codeType = (type.IsPartial ? partNs : mainNs).UseNameSpace(type.UsingNameSpaces).NewClass(type.Name, type.BaseTypes, type.IsPartial);
            foreach (var member in type.Members)
            {
                if (member is FieldInfo field)
                {
                    codeType = codeType.AddField(field.Type, field.Type, field.Comment, field.AccessModifier);
                }
                else if (member is PropertyInfo property)
                {
                    if (property.HasBackingField)
                    { }
                    codeType = codeType.AddProperty(
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
        }

        var mainCode = new Code("Main", Languages.CSharp, mainUnit.GenerateCode(), false);
        var partCode = new Code("Partial", Languages.CSharp, partUnit.GenerateCode(), true);
        var result = new Codes(mainCode, partCode);
        LibLogger.Debug("Generated Behind Code");
        return result;
    }
}

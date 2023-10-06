//using System.CodeDom;

//using Library.CodeGeneration.Models;
//using Library.Helpers.CodeGen;
//using Library.Validations;

//namespace Library.CodeGeneration;

//public sealed class CodeDomCodeGenProvider : ICodeGenProvider
//{
//    public static CodeDomCodeGenProvider New()
//        => new();

//    public Codes GenerateCode(in ICodeGenNameSpace nameSpace)
//    {
//        Check.MustBeArgumentNotNull(nameSpace);

//        LibLogger.Debug("Generating Behind Code");
//        var mainUnit = new CodeCompileUnit();
//        var mainNs = mainUnit?.AddNewNameSpace(nameSpace.FullName).UseNameSpace(nameSpace.UsingNameSpaces);
//        CodeTypeDeclaration? codeTypeBuffer;
//        foreach (var type in nameSpace.CodeGenTypes)
//        {
//            codeTypeBuffer = mainNs?.UseNameSpace(type.UsingNameSpaces).AddNewClass(type.Name, type.BaseTypes, type.IsPartial);
//            if (codeTypeBuffer == null)
//            {
//                continue;
//            }

//            foreach (var member in type.Members)
//            {
//                _ = member switch
//                {
//                    FieldInfo field => addField(codeTypeBuffer, field),
//                    PropertyInfo property when property.HasBackingField => addProperty(codeTypeBuffer, property),
//                    PropertyInfo property => addProperty(codeTypeBuffer, property),

//                    _ => throw new NotImplementedException()
//                };
//            }
//        }

//        var result = new Code("Main", Languages.CSharp, mainUnit!.GenerateCode(), false).ToCodes();
//        LibLogger.Debug("Generated Behind Code");
//        return result;

//        static CodeTypeDeclaration addProperty(in CodeTypeDeclaration codeType, in PropertyInfo property) => codeType.AddProperty(
//            property.Type,
//            property.Name,
//            property.Comment,
//            property.AccessModifier,
//            property.Getter,
//            property.Setter,
//            property.InitCode,
//            property.IsNullable);

//        static CodeTypeDeclaration addField(CodeTypeDeclaration codeTypeBuffer, FieldInfo field) => 
//            codeTypeBuffer.AddField(field.Type, field.Type, field.Comment, field.AccessModifier);
//    }
//}
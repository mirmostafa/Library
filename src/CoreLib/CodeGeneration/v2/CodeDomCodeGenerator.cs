using System.CodeDom;
using System.Reflection;

using Library.CodeGeneration.Models;
using Library.CodeGeneration.v2.Back;
using Library.Helpers.CodeGen;
using Library.Results;
using Library.Validations;

namespace Library.CodeGeneration.v2;

public sealed class CodeDomCodeGenerator : ICodeGeneratorEngine
{
    public Result<string> Generate(INamespace nameSpace)
    {
        var codeUnit = CodeDomHelper.Begin();
        var domNameSpace = codeUnit.AddNewNameSpace(nameSpace.Name).UseNameSpace(nameSpace.UsingNamespaces);
        foreach (var type in nameSpace.Types)
        {
            var domClass = domNameSpace.AddNewClass(
                type.Name,
                type.BaseTypes.Compact().Select(x => x.Name.NotNull()),
                type.InheritanceModifier.Contains(InheritanceModifier.Partial),
                toTypeAttributes(type.AccessModifier));
            _ = domNameSpace.UseNameSpace(type.BaseTypes.Select(x => x.NameSpace).Compact());

            foreach (var member in type.Members)
            {
                useNameSpace(domNameSpace, member);
                var domMember = member switch
                {
                    IField field => createDomField(field),
                    IProperty prop => createDomProperty(prop),
                    IMethod method => createDomMethod(method),
                    _ => throw new NotImplementedException(),
                };
                _ = domClass.Members.Add(domMember);
            }
        }
        var code = codeUnit.GenerateCode();
        return Result<string>.CreateSuccess(code);

        static TypeAttributes toTypeAttributes(AccessModifier accessModifier) => throw new NotImplementedException();
        static CodeTypeMember createDomField(IField member) => throw new NotImplementedException();
        static CodeTypeMember createDomProperty(IProperty member) => throw new NotImplementedException();
        static CodeTypeMember createDomMethod(IMethod member) =>
            CodeDomHelper.NewMethod(member.Name,
                                    member.Body,
                                    member.Type,
                                    MemberAttributes.Public,
                                    member.InheritanceModifier.Contains(InheritanceModifier.Partial),
                                    member.Parameters.Select(x => (new TypePath(x.Type), x.Name)).ToArray());
        static void useNameSpace(CodeNamespace domNameSpace, IMember member)
        {
            if (!member.Type.NameSpace.IsNullOrEmpty())
            {
                _ = domNameSpace.UseNameSpace(member.Type.NameSpace);
            }
        }
    }
}
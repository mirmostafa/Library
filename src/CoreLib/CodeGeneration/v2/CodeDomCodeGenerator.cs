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
        Check.MustBeArgumentNotNull(nameSpace);
        if (!nameSpace.Validate().TryParse(out var vs))
        {
            return vs.WithValue(string.Empty);
        }

        var codeUnit = CodeDomHelper.Begin();
        var domNameSpace = codeUnit.AddNewNameSpace(nameSpace.Name).UseNameSpace(nameSpace.UsingNamespaces);
        foreach (var type in nameSpace.Types)
        {
            if (!type.Validate().TryParse(out vs))
            {
                return vs.WithValue(string.Empty);
            }

            var domClass = domNameSpace.AddNewClass(
                type.Name,
                type.BaseTypes.Compact().Select(x => x.Name.NotNull()),
                type.InheritanceModifier.Contains(InheritanceModifier.Partial),
                toTypeAttributes(domNameSpace, type.AccessModifier));
            _ = domNameSpace.UseNameSpace(type.BaseTypes.Select(x => x.NameSpace).Compact());

            foreach (var member in type.Members)
            {
                if (!member.Validate().TryParse(out vs))
                {
                    return vs.WithValue(string.Empty);
                }

                var domMember = member switch
                {
                    IField field => createDomField(domNameSpace, field),
                    IProperty prop => createDomProperty(domNameSpace, prop),
                    IMethod method => createDomMethod(domNameSpace, method),
                    _ => throw new NotImplementedException(),
                };
                _ = domClass.Members.Add(domMember);
            }
        }
        var code = codeUnit.GenerateCode();
        return Result<string>.CreateSuccess(code);

        static TypeAttributes toTypeAttributes(CodeNamespace domNameSpace, AccessModifier accessModifier) => throw new NotImplementedException();
        static CodeTypeMember createDomField(CodeNamespace domNameSpace, IField member) => throw new NotImplementedException();
        static CodeTypeMember createDomProperty(CodeNamespace domNameSpace, IProperty member) => throw new NotImplementedException();
        static CodeTypeMember createDomMethod(CodeNamespace domNameSpace, IMethod method)
        {
            useNameSpace(domNameSpace, method.ReturnType);
            method.Parameters.Select(x => x.Type).ForEach(x => useNameSpace(domNameSpace, x));
            return CodeDomHelper.NewMethod(method.Name,
                                    method.Body,
                                    method.ReturnType,
                                    MemberAttributes.Public,
                                    method.InheritanceModifier.Contains(InheritanceModifier.Partial),
                                    method.Parameters.Select(x => (new TypePath(x.Type), x.Name)).ToArray());
        }

        static void useNameSpace(CodeNamespace domNameSpace, TypePath? typePath)
        {
            if (!(typePath?.NameSpace.IsNullOrEmpty() ?? true))
            {
                _ = domNameSpace.UseNameSpace(typePath.NameSpace);
            }
        }
    }
}
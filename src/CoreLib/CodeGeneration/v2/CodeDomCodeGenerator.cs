using System.CodeDom;
using System.Reflection;

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
        if (!nameSpace.Validate().TryParse(out var vr1))
        {
            return vr1.WithValue(string.Empty);
        }

        var codeUnit = CodeDomHelper.Begin();
        var domNameSpace = codeUnit.AddNewNameSpace(nameSpace.Name).UseNameSpace(nameSpace.UsingNamespaces);
        foreach (var type in nameSpace.Types)
        {
            if (!type.Validate().TryParse(out var vr2))
            {
                return vr2.WithValue(string.Empty);
            }

            var domType = domNameSpace.AddNewType(
                type.Name,
                type.BaseTypes.Compact().Select(x => x.Name.NotNull()),
                isStatic: type.InheritanceModifier.Contains(InheritanceModifier.Static),
                isPartial: type.InheritanceModifier.Contains(InheritanceModifier.Partial),
                typeAttributes: toTypeAttributes(domNameSpace, type.AccessModifier, type.InheritanceModifier));

            _ = domNameSpace.UseNameSpace(type.BaseTypes.Select(x => x.NameSpace).Compact());

            foreach (var member in type.Members.Compact())
            {
                if (!member.Validate().TryParse(out var vr3))
                {
                    return vr3.WithValue(string.Empty);
                }

                var domMember = member switch
                {
                    IField field => createDomField(domNameSpace, field),
                    IProperty prop => createDomProperty(domNameSpace, prop),
                    IMethod method => createDomMethod(domNameSpace, method),
                    _ => throw new NotImplementedException(),
                };
                _ = domType.Members.Add(domMember);
            }
        }
        var code = SetStaticIfRequired(nameSpace, codeUnit.GenerateCode());
        return Result<string>.CreateSuccess(code);

        static TypeAttributes toTypeAttributes(CodeNamespace domNameSpace, AccessModifier accessModifier, InheritanceModifier inheritanceModifier)
        {
            var result = TypeAttributes.AutoLayout;
            if (accessModifier.Contains(AccessModifier.Public))
            {
                result |= TypeAttributes.Public;
            }

            if (inheritanceModifier.Contains(InheritanceModifier.Sealed))
            {
                result |= TypeAttributes.Sealed;
            }

            return result;
        }
        static CodeTypeMember createDomField(CodeNamespace domNameSpace, IField member) => throw new NotImplementedException();
        static CodeTypeMember createDomProperty(CodeNamespace domNameSpace, IProperty member) => throw new NotImplementedException();
        static CodeTypeMember createDomMethod(CodeNamespace domNameSpace, IMethod method)
        {
            useNameSpace(domNameSpace, method.ReturnType);
            method.Parameters.Select(x => x.Type).ForEach(x => useNameSpace(domNameSpace, x));

            var parameters = method.Parameters.ToArray();
            // Check if the method is an extension method
            if (method.IsExtension && parameters.Any())
            {
                // Add "this" to the type of the first parameter (extension method)
                parameters[0] = ($"this {parameters[0].Type}", parameters[0].Name);
            }

            return CodeDomHelper.NewMethod(method.Name,
                                        method.Body,
                                        method.ReturnType,
                                        MemberAttributes.Public,
                                        method.InheritanceModifier.Contains(InheritanceModifier.Partial),
                                        parameters);
        }
        static void useNameSpace(CodeNamespace domNameSpace, TypePath? typePath)
        {
            if (typePath?.NameSpace is not null and not "")
            {
                _ = domNameSpace.UseNameSpace(typePath.NameSpace);
            }
        }
        static string addStaticToMember(string code, in IEnumerable<string> memberNames)
        {
            var lines = code.Split(Environment.NewLine);
            var accessModifiers = new string[] { "public ", "private ", "protected ", "internal " };
            var result = new StringBuilder();
            string buffer;

            foreach (var line in lines)
            {
                buffer = line;
                if (line.ContainsAny(memberNames, out var member))
                {
                    if (line.ContainsAny(new[] { $"class {member}", $"{member}(", $"{member} (" }) && !line.Contains(" static"))
                    {
                        var rightPlace = line.IndexOfAny(out var accessModifier, accessModifiers);
                        buffer = rightPlace != -1 ? line.Insert(rightPlace + accessModifier!.Length, "static ") : line;
                    }
                }
                _ = result.AppendLine(buffer);
            }

            return result.ToString();
        }

        static string SetStaticIfRequired(INamespace nameSpace, string code)
        {
            var staticTypes = nameSpace.Types.Where(x => x.InheritanceModifier.Contains(InheritanceModifier.Static));
            if (staticTypes.Any())
            {
                foreach (var type in staticTypes)
                {
                    var things = Enumerable.Empty<string>().AddImmuted(type.Name).AddRangeImmuted(type.Members.Select(x => x.Name)).Distinct();
                    code = addStaticToMember(code, things);
                }
            }

            return code;
        }
    }
}
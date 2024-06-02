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
                typeAttributes: toTypeAttributes(type.AccessModifier, type.InheritanceModifier));

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

        var code = codeUnit.GenerateCode();
        var result = setStaticIfRequired(nameSpace, code);
        return Result.Success<string>(result);

        static TypeAttributes toTypeAttributes(AccessModifier accessModifier, InheritanceModifier inheritanceModifier)
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
        static MemberAttributes toMemberAttributes(AccessModifier accessModifier, InheritanceModifier inheritanceModifier)
        {
            var result = MemberAttributes.Final;
            if (accessModifier.Contains(AccessModifier.Public))
            {
                result |= MemberAttributes.Public;
            }
            else if (accessModifier.Contains(AccessModifier.Private))
            {
                result |= MemberAttributes.Private;
            }
            else if (accessModifier.Contains(AccessModifier.Protected))
            {
                result |= MemberAttributes.Family;
            }

            return result;
        }
        static CodeTypeMember createDomField(CodeNamespace domNameSpace, IField member) => throw new NotImplementedException();
        static CodeTypeMember createDomProperty(CodeNamespace domNameSpace, IProperty member) => throw new NotImplementedException();
        static CodeTypeMember createDomMethod(CodeNamespace domNameSpace, IMethod method)
        {
            useNameSpace(domNameSpace, method.ReturnType);
            method.Arguments.Select(x => x.Type).ForEach(x => useNameSpace(domNameSpace, x));

            var parameters = method.Arguments.ToArray();
            if (method.IsExtension)
            {
                parameters[0] = new Models.MethodArgument($"this {parameters[0].Type}", parameters[0].Name);
            }

            return CodeDomHelper.NewMethod(method.Name,
                                        method.Body,
                                        method.ReturnType,
                                        toMemberAttributes(method.AccessModifier, method.InheritanceModifier),
                                        method.InheritanceModifier.Contains(InheritanceModifier.Partial),
                                        parameters.Select(x => (x.Type.FullName, x.Name)));
        }
        static void useNameSpace(CodeNamespace domNameSpace, TypePath? typePath)
        {
            if (typePath?.NameSpace is not null and not "")
            {
                _ = domNameSpace.UseNameSpace(typePath.NameSpace);
            }
        }
        static string setStaticIfRequired(INamespace nameSpace, string code)
        {
            var staticTypes = nameSpace.Types.Where(x => x.InheritanceModifier.Contains(InheritanceModifier.Static));
            if (staticTypes.Any())
            {
                foreach (var type in staticTypes)
                {
                    var allMembers = Enumerable.Empty<string>().AddImmuted(type.Name).AddRangeImmuted(type.Members.Select(x => x.Name)).Distinct().ToArray();
                    code = addPrefixToMember(code, "static", allMembers);
                }
            }
            var staticMembers = nameSpace.Types
                .Except(staticTypes)
                .Select(x => x.Members).SelectAll()
                .Where(x => x.InheritanceModifier.Contains(InheritanceModifier.Static)).Select(x => x.Name);
            if (staticMembers.Any())
            {
                _ = staticMembers.Aggregate((string member, string code) => addPrefixToMember(code, "static", member), code);
            }

            return code;
            static string addPrefixToMember(string code, string prefix, params string[] memberNames)
            {
                var prfx = $"{prefix.Trim()} ";
                var accessModifiers = new string[] { "public ", "private ", "protected ", "internal " };
                var result = new StringBuilder();
                string buffer;

                code.ReadLines().ForEach(line =>
                {
                    buffer = line;
                    buffer.FindEach(memberNames).ForEach(member =>
                    {
                        if (!buffer.ContainsAny(new[] { $"class {member}", $"{member}(", $"{member} (" }) || buffer.Contains(prfx))
                        {
                            return;
                        }
                        var rightPlace = buffer.IndexOfAny(out var accessModifier, accessModifiers);
                        buffer = rightPlace != -1 ? buffer.Insert(rightPlace + accessModifier!.Length, "static ") : buffer;
                    });
                    _ = result.AppendLine(buffer);
                });

                return result.ToString();
            }
        }
    }
}
using Library.CodeGeneration.v2.Back;
using Library.Helpers.CodeGen;
using Library.Results;
using Library.Validations;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Library.CodeGeneration.v2;

public sealed class RoslynCodeGenerator : ICodeGeneratorEngine
{
    public Result<string> Generate(INamespace nameSpace)
    {
        Check.MustBeArgumentNotNull(nameSpace);

        var root = RoslynHelper.CreateRoot();
        if (!nameSpace.Validate().TryParse(out var vr1))
        {
            return vr1.WithValue(string.Empty);
        }
        var rosNameSpace = RoslynHelper.CreateNamespace(nameSpace.Name);
        foreach (var type in nameSpace.Types)
        {
            if (!type.Validate().TryParse(out var vr2))
            {
                return vr2.WithValue(string.Empty);
            }

            var modifiers = GeneratorHelper.ToModifiers(type.AccessModifier, type.InheritanceModifier);
            rosNameSpace = rosNameSpace.AddType(type.Name, out var rosType, modifiers);
            foreach (var baseType in type.BaseTypes)
            {
                if (!baseType.NameSpace.IsNullOrEmpty())
                {
                    rosNameSpace = rosNameSpace.AddUsingNameSpace(baseType.NameSpace);
                }

                rosType = rosType.AddBase(baseType.Name);
            }
            foreach (var member in type.Members.Compact())
            {
                if (!member.Validate().TryParse(out var vr3))
                {
                    return vr3.WithValue(string.Empty);
                }
                var ros = member switch
                {
                    IField field => createRosField(root, field),
                    IProperty prop => createRosProperty(root, prop),
                    IMethod method => createRosMethod(root, method),
                    _ => throw new NotImplementedException(),
                };
                (root, rosType) = (ros.Root, rosType.AddMembers(ros.Member));
            }
        }

        nameSpace.UsingNamespaces.ForEach(x => root = root.AddUsingNameSpace(x));
        root = root.AddNameSpace(rosNameSpace);
        return Result<string>.CreateSuccess(root.GenerateCode());

        static (MemberDeclarationSyntax Member, CompilationUnitSyntax Root) createRosField(CompilationUnitSyntax root, IField member) => throw new NotImplementedException();
        static (MemberDeclarationSyntax Member, CompilationUnitSyntax Root) createRosProperty(CompilationUnitSyntax root, IProperty member) => throw new NotImplementedException();
        static (MemberDeclarationSyntax Member, CompilationUnitSyntax Root) createRosMethod(CompilationUnitSyntax root, IMethod method) => throw new NotImplementedException();
    }
}

internal static class GeneratorHelper
{
    internal static List<SyntaxKind> ToModifiers(AccessModifier accessModifier, InheritanceModifier inheritanceModifier) => throw new NotImplementedException();
}
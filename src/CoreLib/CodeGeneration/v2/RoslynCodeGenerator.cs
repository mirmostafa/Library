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
            var rosType = RoslynHelper.CreateType(TypePath.New(type.Name), modifiers);
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
                root = ros.Root;
                rosType = rosType.AddMembers(ros.Member);
            }
            rosNameSpace = rosNameSpace.AddType(rosType);
        }

        nameSpace.UsingNamespaces.ForEach(x => root = root.AddUsingNameSpace(x));
        root = root.AddNameSpace(rosNameSpace);
        return Result<string>.CreateSuccess(root.GenerateCode());

        static (MemberDeclarationSyntax Member, CompilationUnitSyntax Root) createRosField(CompilationUnitSyntax root, IField member) => throw new NotImplementedException();
        static (MemberDeclarationSyntax Member, CompilationUnitSyntax Root) createRosProperty(CompilationUnitSyntax root, IProperty member) => throw new NotImplementedException();
        static (MemberDeclarationSyntax Member, CompilationUnitSyntax Root) createRosMethod(CompilationUnitSyntax root, IMethod method)
        {
            var result = RoslynHelper.CreateMethod(new(GeneratorHelper.ToModifiers(method.AccessModifier, method.InheritanceModifier)
                , method.ReturnType, method.Name, method.Parameters, method.Body, method.IsExtension));
            method.GetNameSpaces().ForEach(x => root = root.AddUsingNameSpace(x));
            return (result, root);
        }
    }
}

internal static class GeneratorHelper
{
    internal static IEnumerable<SyntaxKind> ToModifiers(AccessModifier access, InheritanceModifier inheritance)
    {
        var result = new List<SyntaxKind>();
        updateModifier(access, result, AccessModifier.Public, SyntaxKind.PublicKeyword);
        updateModifier(access, result, AccessModifier.Private, SyntaxKind.PrivateKeyword);
        updateModifier(access, result, AccessModifier.Internal, SyntaxKind.InternalKeyword);
        updateModifier(access, result, AccessModifier.Protected, SyntaxKind.ProtectedKeyword);

        updateInheritance(inheritance, result, InheritanceModifier.Sealed, SyntaxKind.SealedKeyword);
        updateInheritance(inheritance, result, InheritanceModifier.New, SyntaxKind.NewKeyword);
        updateInheritance(inheritance, result, InheritanceModifier.Override, SyntaxKind.OverrideKeyword);
        updateInheritance(inheritance, result, InheritanceModifier.Abstract, SyntaxKind.AbstractKeyword);
        updateInheritance(inheritance, result, InheritanceModifier.Const, SyntaxKind.ConstKeyword);
        updateInheritance(inheritance, result, InheritanceModifier.Partial, SyntaxKind.PartialKeyword);
        updateInheritance(inheritance, result, InheritanceModifier.Static, SyntaxKind.StaticKeyword);
        updateInheritance(inheritance, result, InheritanceModifier.Virtual, SyntaxKind.VirtualKeyword);

        return result;

        static void updateModifier(AccessModifier access, List<SyntaxKind> result, AccessModifier referral, SyntaxKind kind)
        {
            if (access.Contains(referral))
            {
                result.Add(kind);
            }
        }

        static void updateInheritance(InheritanceModifier inheritance, List<SyntaxKind> result, InheritanceModifier referral, SyntaxKind kind)
        {
            if (inheritance.Contains(referral))
            {
                result.Add(kind);
            }
        }
    }
}
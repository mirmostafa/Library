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
        if (!nameSpace.Validate().TryParse(out var vr))
        {
            return vr.WithValue(string.Empty);
        }
        var rosNameSpace = RoslynHelper.CreateNamespace(nameSpace.Name);
        foreach (var type in nameSpace.Types)
        {
            var modifiers = GeneratorHelper.ToModifiers(type.AccessModifier, type.InheritanceModifier);
            var rosType = RoslynHelper.CreateType(TypePath.New(type.Name), modifiers);
            foreach (var baseType in type.BaseTypes)
            {
                rosType = rosType.AddBase(baseType.FullName);
                root = baseType.GetNameSpaces().SelectImmutable((ns, r) => r.AddUsingNameSpace(ns), root);
            }
            foreach (var member in type.Members.Compact())
            {
                (var codeMember, root) = member switch
                {
                    IField field => createRosField(root, field),
                    IProperty prop => createRosProperty(root, prop),
                    IMethod method => createRosMethod(root, method, type.Name),
                    _ => throw new NotImplementedException(),
                };
                rosType = rosType.AddMembers(codeMember);
            }
            rosNameSpace = rosNameSpace.AddType(rosType);
        }

        nameSpace.UsingNamespaces.ForEach(x => root = root.AddUsingNameSpace(x));

        root = root.AddNameSpace(rosNameSpace);
        return Result<string>.CreateSuccess(root.GenerateCode());

        static (MemberDeclarationSyntax Member, CompilationUnitSyntax Root) createRosField(CompilationUnitSyntax root, IField member)
        {
            var modifiers = GeneratorHelper.ToModifiers(member.AccessModifier, member.InheritanceModifier);
            var result = RoslynHelper.CreateField(new(member.Name, member.Type, modifiers));
            member.Type.GetNameSpaces().ForEach(x => root = root.AddUsingNameSpace(x));
            return (result, root);
        }
        static (MemberDeclarationSyntax Member, CompilationUnitSyntax Root) createRosProperty(CompilationUnitSyntax root, IProperty member)
        {
            var result = RoslynHelper.CreateProperty(new(member.Name, member.Type, (IEnumerable<SyntaxKind>?)GeneratorHelper.ToModifiers(member.AccessModifier, member.InheritanceModifier),
                (member.Getter is not null, GeneratorHelper.ToModifiers(member.Getter?.AccessModifier, null)),
                (member.Setter is not null, GeneratorHelper.ToModifiers(member.Setter?.AccessModifier, null))
                ));
            member.Type.GetNameSpaces().ForEach(x => root = root.AddUsingNameSpace(x));
            return (result, root);
        }

        static (MemberDeclarationSyntax Member, CompilationUnitSyntax Root) createRosMethod(CompilationUnitSyntax root, IMethod method, string className)
        {
            var modifiers = GeneratorHelper.ToModifiers(method.AccessModifier, method.InheritanceModifier);
            var result = method.IsConstructor
                ? RoslynHelper.CreateConstructor(TypePath.GetName(className), modifiers, method.Parameters, method.Body)
                : RoslynHelper.CreateMethod(new(modifiers, method.ReturnType, method.Name, method.Parameters, method.Body, method.IsExtension));
            method.GetNameSpaces().ForEach(x => root = root.AddUsingNameSpace(x));
            return (result, root);
        }
    }
}

internal static class GeneratorHelper
{
    internal static IEnumerable<SyntaxKind> ToModifiers(AccessModifier? access, InheritanceModifier? inheritance)
    {
        var result = new List<SyntaxKind>();

        var modifier = access == null ? AccessModifier.None : access.Value;
        updateModifier(modifier, result, AccessModifier.Public, SyntaxKind.PublicKeyword);
        updateModifier(modifier, result, AccessModifier.Private, SyntaxKind.PrivateKeyword);
        updateModifier(modifier, result, AccessModifier.Internal, SyntaxKind.InternalKeyword);
        updateModifier(modifier, result, AccessModifier.Protected, SyntaxKind.ProtectedKeyword);
        updateModifier(modifier, result, AccessModifier.ReadOnly, SyntaxKind.ReadOnlyKeyword);

        var inherit = inheritance == null ? InheritanceModifier.None : inheritance.Value;
        updateInheritance(inherit, result, InheritanceModifier.Sealed, SyntaxKind.SealedKeyword);
        updateInheritance(inherit, result, InheritanceModifier.New, SyntaxKind.NewKeyword);
        updateInheritance(inherit, result, InheritanceModifier.Override, SyntaxKind.OverrideKeyword);
        updateInheritance(inherit, result, InheritanceModifier.Abstract, SyntaxKind.AbstractKeyword);
        updateInheritance(inherit, result, InheritanceModifier.Const, SyntaxKind.ConstKeyword);
        updateInheritance(inherit, result, InheritanceModifier.Partial, SyntaxKind.PartialKeyword);
        updateInheritance(inherit, result, InheritanceModifier.Static, SyntaxKind.StaticKeyword);
        updateInheritance(inherit, result, InheritanceModifier.Virtual, SyntaxKind.VirtualKeyword);

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
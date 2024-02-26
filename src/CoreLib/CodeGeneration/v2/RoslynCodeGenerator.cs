using Library.CodeGeneration.v2.Back;
using Library.Helpers.CodeGen;
using Library.Results;
using Library.Validations;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Library.CodeGeneration.v2;

public sealed class RoslynCodeGenerator : ICodeGeneratorEngine
{
    public string Generate(IProperty property)
    {
        Check.MustBeArgumentNotNull(property);

        var prop = CreateRosProperty(RoslynHelper.CreateRoot(), property);
        return prop.Root.AddMembers(prop.Member).GenerateCode();
    }

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
            foreach (var attribute in type.Attributes.Compact())
            {
                var attributeType = TypePath.New(attribute.Name);
                foreach (var ns in attributeType.GetNameSpaces().Compact())
                {
                    root = root.AddUsingNameSpace(ns);
                }
                rosType = rosType.AddAttribute(attributeType.Name, attribute.Properties.Select(x => (x.Name, x.Value)));
            }
            foreach (var member in type.Members.Compact())
            {
                (var codeMember, root) = member switch
                {
                    IField field => CreateRosField(root, field),
                    IMethod method => CreateRosMethod(root, method, type.Name),
                    IProperty prop => CreateRosProperty(root, prop),
                    _ => throw new NotImplementedException(),
                };
                //foreach (var attribute in member.Attributes)
                //{
                //    var attributeType = TypePath.New(attribute.Name);
                //    foreach (var ns in attributeType.GetNameSpaces().Compact())
                //    {
                //        root = root.AddUsingNameSpace(ns);
                //    }
                //    codeMember = codeMember.AddAttribute(attributeType.Name, attribute.Properties.Select(x => (x.Name, x.Value)));
                //}
                rosType = rosType.AddMembers(codeMember);
            }
            rosNameSpace = rosNameSpace.AddType(rosType);
        }

        nameSpace.UsingNamespaces.ForEach(x => root = root.AddUsingNameSpace(x));

        root = root!.AddNameSpace(rosNameSpace);

        var distinctUsings = root.DescendantNodes().OfType<UsingDirectiveSyntax>()
            .GroupBy(u => u.Name?.ToString()).Select(g => g.First())
            .Compact();
        var finalRoot = root.WithUsings(List(distinctUsings));

        return Result.Success<string>(finalRoot.GenerateCode());
    }

    private static (MemberDeclarationSyntax Member, CompilationUnitSyntax Root) AddAttributes(PropertyDeclarationSyntax prop, CompilationUnitSyntax root, IMember member)
    {
        if (member.Attributes.Any())
        {
            foreach (var attribute in member.Attributes)
            {
                var attr = Attribute(ParseName(attribute.Name.FullName));
                var attributeList = AttributeList(SingletonSeparatedList(attr));
                prop = prop.AddAttributeLists(attributeList);

                attribute.Name.GetNameSpaces().ForEach(x => root = root.AddUsingNameSpace(x));
            }
        }
        return (prop, root);
    }

    private static (MemberDeclarationSyntax Member, CompilationUnitSyntax Root) CreateRosField(CompilationUnitSyntax root, IField member)
    {
        var modifiers = GeneratorHelper.ToModifiers(member.AccessModifier, member.InheritanceModifier);
        var result = RoslynHelper.CreateField(new(member.Name, member.Type, modifiers));
        member.Type.GetNameSpaces().ForEach(x => root = root.AddUsingNameSpace(x));
        return (result, root);
    }

    private static (MemberDeclarationSyntax Member, CompilationUnitSyntax Root) CreateRosMethod(CompilationUnitSyntax root, IMethod method, string className)
    {
        var modifiers = GeneratorHelper.ToModifiers(method.AccessModifier, method.InheritanceModifier);
        var result = method.IsConstructor
            ? RoslynHelper.CreateConstructor(TypePath.GetName(className), modifiers, method.Parameters, method.Body)
            : RoslynHelper.CreateMethod(new(modifiers, method.ReturnType, method.Name, method.Parameters, method.Body, method.IsExtension));
        method.GetNameSpaces().ForEach(x => root = root.AddUsingNameSpace(x));
        return (result, root);
    }

    private static (MemberDeclarationSyntax Member, CompilationUnitSyntax Root) CreateRosProperty(CompilationUnitSyntax root, IProperty member)
    {
        var propertyInfo = new RosPropertyInfo(member.Name, member.Type, (IEnumerable<SyntaxKind>?)GeneratorHelper.ToModifiers(member.AccessModifier, member.InheritanceModifier), (member.Getter is not null, GeneratorHelper.ToModifiers(member.Getter?.AccessModifier, null)), (member.Setter is not null, GeneratorHelper.ToModifiers(member.Setter?.AccessModifier, null)));
        var prop = member.BackingFieldName.IsNullOrEmpty()
            ? RoslynHelper.CreateProperty(propertyInfo)
            : RoslynHelper.CreatePropertyWithBackingField(propertyInfo, new RosFieldInfo(member.BackingFieldName, member.Type)).Property;
        member.Type.GetNameSpaces().ForEach(x => root = root.AddUsingNameSpace(x));
        var result = AddAttributes(prop, root, member);
        return result;
    }
}